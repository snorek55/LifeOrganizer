﻿using Common.Adapters;

namespace MovOrg.Organizer.Adapters.Items
{
	public class BoxOfficeViewModel : BaseViewModel
	{
		public string Budget { get; set; }

		public string OpeningWeekendUSA { get; set; }

		public string GrossUsa { get; set; }

		public string CumulativeWorldwideGross { get; set; }
	}
}