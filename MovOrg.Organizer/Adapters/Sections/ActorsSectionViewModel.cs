using Common.Adapters;
using Common.Extensions;
using Common.Setup;

using MovOrg.Organizer.Adapters.Items;
using MovOrg.Organizer.UseCases;
using MovOrg.Organizer.UseCases.Actors.DTO;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace MovOrg.Organizer.Adapters.Sections
{
	public class ActorsSectionViewModel : SectionViewModel
	{
		public ObservableCollection<ActorViewModel> Actors { get; } = new ObservableCollection<ActorViewModel>();

		public ActorViewModel SelectedActor { get; set; }

		public ActorDetailsPanelViewModel ActorDetailsPanel { get; private set; }

		public bool AreDetailsShowing { get => SelectedActor != null; }

		#region Commands

		public ICommand SearchCommand { get => new AsyncCommand(SearchActorsWithChosenNameAsync, NotificationsHandler); }
		public ICommand ClearSearchCommand { get => new AsyncCommand(ClearSearchAsync, NotificationsHandler); }
		public ICommand SortAlphabeticallyCommand { get => new SyncCommand(SortAlphabetically); }

		#endregion Commands

		#region Filtering Properties

		public bool OnlyFavorites { get; set; }
		private string SuggestedNameFilter { get; set; }

		#endregion Filtering Properties

		#region Data

		public string SuggestedName { get; set; }

		#endregion Data

		#region Private Fields

		private ICollectionView actorsCollectionView;

		private IActorsService actorsService;
		private IAutoMapper mapper;
		private IDispatcher dispatcher;

		#endregion Private Fields

		public ActorsSectionViewModel()
		{
			SectionName = "Actors";
			actorsCollectionView = CollectionViewSource.GetDefaultView(Actors);
			actorsCollectionView.Filter = new Predicate<object>(x => ActorFiltering(x as ActorViewModel));
			Actors.CollectionChanged += Actors_CollectionChanged;
		}

		public ActorsSectionViewModel(IActorsService actorsService, IAutoMapper autoMapper, IDispatcher dispatcher) : this()
		{
			mapper = autoMapper;
			this.dispatcher = dispatcher;
			this.actorsService = actorsService;
			ActorDetailsPanel = new ActorDetailsPanelViewModel(actorsService, this);
			GetAllActorsFromLocalOnStartup();
		}

		private void GetAllActorsFromLocalOnStartup()
		{
			dispatcher.BeginInvoke(async () =>
			{
				var response = await ExecuteCommandTaskAsync(() => actorsService.GetAllActorsFromLocal(), "Loaded actors from local");
				if (!response.HasError)
					ResetAndUpdateActorList(response.Data);
			});
		}

		#region Command Methods

		private async Task ClearSearchAsync()
		{
			SuggestedNameFilter = string.Empty;
			var moviesResponse = await ExecuteCommandTaskAsync(() => actorsService.GetAllActorsFromLocal(), "Loaded movies from local");

			if (!moviesResponse.HasError)
				ResetAndUpdateActorList(moviesResponse.Data);
		}

		private async Task SearchActorsWithChosenNameAsync()
		{
			if (string.IsNullOrEmpty(SuggestedName))
			{
				await ClearSearchAsync();
				return;
			}

			var suggestedNameResponse = await ExecuteCommandTaskAsync(() => actorsService.GetActorsFromSuggestedNameAsync(SuggestedName), "");

			if (!suggestedNameResponse.HasError)
			{
				ResetAndUpdateActorList(suggestedNameResponse.Data);
				NotifyStatus("Found " + suggestedNameResponse.Data.Count() + " actors");
			}
			else
			{
				Actors.Clear();
			}
		}

		#endregion Command Methods

		#region Filtering and Sorting Methods

		private bool ActorFiltering(ActorViewModel actor)
		{
			bool conditions = true;

			if (OnlyFavorites)
				conditions &= actor.IsFavorite;
			if (!string.IsNullOrEmpty(SuggestedNameFilter) && !string.IsNullOrEmpty(actor.Name))
			{
				var titleContainsFilter = actor.Name.Contains(SuggestedNameFilter);
				conditions &= titleContainsFilter;
			}

			return conditions;
		}

		private void SortAlphabetically()
		{
			if (actorsCollectionView.SortDescriptions.Count == 0)
				actorsCollectionView.SortDescriptions.Add(new SortDescription(nameof(SelectedActor.Name), ListSortDirection.Ascending));
			else
				actorsCollectionView.SortDescriptions.Clear();
		}

		private void RefreshFilter()
		{
			actorsCollectionView.Refresh();
			NotifyStatus("Filtered actors: " + actorsCollectionView.Count());
		}

		#endregion Filtering and Sorting Methods

		#region Events

#pragma warning disable IDE0051 // Quitar miembros privados no utilizados

		private void Actors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			NotifyItemTotal(Actors.Count);
		}

		private void OnOnlyFavoritesChanged()
		{
			RefreshFilter();
		}

		private void OnSelectedActorChanged()
		{
			ShowSelectedActorInfoAsync().FireAndForgetSafeAsync(NotificationsHandler);
		}

		public async Task ShowSelectedActorInfoAsync(bool forceUpdate = false)
		{
			if (SelectedActor == null) return;

			var response = await ExecuteCommandTaskAsync(() => actorsService.GetActorWithId(SelectedActor.Id, forceUpdate), "");
			if (!response.HasError)
			{
				ActorDetailsPanel.SelectedActor = MapActor(response.Data);
				NotifyStatus("Loaded info for actor " + SelectedActor.Name);
			}
		}

#pragma warning restore IDE0051 // Quitar miembros privados no utilizados

		#endregion Events

		#region Rendering

		private void ResetAndUpdateActorList(IEnumerable<ActorListItemDto> actors)
		{
			Actors.Clear();
			if (actors == null) return;

			var actorVm = mapper.Map<IEnumerable<ActorViewModel>>(actors);

			foreach (var item in actorVm)
			{
				Actors.Add(item);
			}
		}

		public void HandleError(string errorMessage)
		{
			Debug.Write(errorMessage);
			NotifyError(errorMessage);
		}

		private ActorViewModel MapActor(ActorWithDetailsDto movie)
		{
			var actorVm = mapper.Map<ActorViewModel>(movie);
			return actorVm;
		}

		#endregion Rendering
	}
}