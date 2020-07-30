using Organizers.Common.Adapters;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Organizers.Main.Adapters.Sections
{
	public class OrganizerContainerViewModel : BaseViewModel, INotificationHandler
	{
		public ObservableCollection<SectionViewModel> Sections { get; set; } = new ObservableCollection<SectionViewModel>();
		public SectionViewModel SelectedSection { get; set; }
		protected ISectionsFactory SectionsFactory { get; set; }

		public int TotalItemCount { get; set; }

		public string StatusMessage { get; set; }

		public string ErrorMessage { get; set; }

		public bool HasError { get; set; }

		public ICommand ClearErrorsCommand { get => new SyncCommand(() => ClearAllInfo()); }

		public OrganizerContainerViewModel(ISectionsFactory sectionsFactory)
		{
			SectionsFactory = sectionsFactory;
			SectionsFactory.ErrorHandler = this;
		}

		public void NotifyError(Exception ex)
		{
			NotifyError(ex.ToString());
		}

		public void NotifyError(string error)
		{
			ErrorMessage = error;
			Debug.WriteLine(ErrorMessage);
		}

		public void NotifyStatus(string message)
		{
			StatusMessage = message;
		}

		public void NotifyWait()
		{
			StatusMessage = "Processing, please wait...";
		}

		public void NotifyItemCount(int count)
		{
			TotalItemCount = count;
		}

		public void ClearAllInfo()
		{
			ErrorMessage = string.Empty;
			StatusMessage = string.Empty;
		}
	}
}