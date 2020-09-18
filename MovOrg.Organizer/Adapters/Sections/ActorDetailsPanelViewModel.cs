using Common.Adapters;

using MovOrg.Organizer.Adapters.Items;
using MovOrg.Organizer.UseCases;

using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovOrg.Organizer.Adapters.Sections
{
	public class ActorDetailsPanelViewModel : BaseViewModel
	{
		public ActorViewModel SelectedActor { get; set; }
		public ICommand UpdateActorCommand { get => new AsyncCommand(UpdateCurrentActorAsync, parent.NotificationsHandler); }
		public ICommand WikipediaCommand { get => new SyncCommand(GoToWikipedia); }
		public ICommand IMDbCommand { get => new SyncCommand(GoToIMDbPage); }
		public ICommand IsFavoriteCommand { get => new AsyncCommand(UpdateFavoriteStatus); }

		private ActorsSectionViewModel parent;

		private IActorsService service;

		public ActorDetailsPanelViewModel(IActorsService service, ActorsSectionViewModel parent)
		{
			this.parent = parent;
			this.service = service;
		}

		private async Task UpdateFavoriteStatus()
		{
			var response = await service.UpdateFavoriteStatus(SelectedActor.Id, SelectedActor.IsFavorite);
			if (response.HasError)
				parent.HandleError(response.Error);

			parent.SelectedActor.IsFavorite = SelectedActor.IsFavorite;
		}

		private async Task UpdateCurrentActorAsync()
		{
			await parent.ShowSelectedActorInfoAsync(true);
		}

		private void GoToWikipedia()
		{
			Process.Start("explorer.exe", SelectedActor.WikipediaUrl);
		}

		private void GoToIMDbPage()
		{
			Process.Start("explorer.exe", "https://www.imdb.com/title/" + SelectedActor.Id + "/");
		}
	}
}