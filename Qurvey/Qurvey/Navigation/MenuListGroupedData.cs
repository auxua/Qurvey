using System;
using System.Collections.ObjectModel;

namespace Qurvey.Navigation
{
	public class MenuListGroupedData : ObservableCollection<NavigationGroup>
	{
		public MenuListGroupedData ()
		{
			NavigationGroup mainGroup = new NavigationGroup ("Main Menu");
			NavigationGroup courseGroup = new NavigationGroup ("Courses");

			NavMenuItem NavConfig = new NavMenuItem () {
				Title = "Welcome",
				Icon = "info.png",
				TargetType = typeof(pages.WelcomePage)
			};

			NavMenuItem NavAuthorize = new NavMenuItem () {
				Title = "Config",
				Icon = "info.png",
				TargetType = typeof(pages.ConfigPage)
			};

			NavMenuItem NavCoursePage = new NavMenuItem () {
				Title = "Course Page",
				Icon = "l2plogo.png",
				CID = "14ws-44149",
				TargetType = typeof(pages.CourseRoomPage)
			};

			mainGroup.Add (NavConfig);
			mainGroup.Add (NavAuthorize);
			courseGroup.Add (NavCoursePage);

			this.Add (mainGroup);
			this.Add (courseGroup);
		}
	}
}