using Organizers.Common.Adapters;
using Organizers.Main.Adapters.Sections;

using System;
using System.Collections.ObjectModel;

namespace Organizers.Main.Adapters.Sections
{
	public class OrganizerContainerViewModel : BaseViewModel, INotificationHandler
	{
		public ObservableCollection<SectionViewModel> Sections { get; set; } = new ObservableCollection<SectionViewModel>();
		public SectionViewModel SelectedSection { get; set; }
		protected ISectionsFactory SectionsFactory { get; set; }
		public string StatusMessage { get; set; }

		public string ErrorMessage { get; set; }

		public bool HasError { get; set; }

		public OrganizerContainerViewModel(ISectionsFactory sectionsFactory)
		{
			SectionsFactory = sectionsFactory;
			SectionsFactory.ErrorHandler = this;
		}

		public void NotifyError(Exception ex)
		{
			ErrorMessage = ex.ToString();
		}

		public void NotifyError(string error)
		{
			ErrorMessage = error;
		}

		public void NotifyStatusMessage(string message)
		{
			StatusMessage = message;
		}

		public void NotifyWaitMessage()
		{
			StatusMessage = "Processing, please wait...";
		}
	}
}