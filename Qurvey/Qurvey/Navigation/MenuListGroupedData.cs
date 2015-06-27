using System;
using System.Collections.ObjectModel;

namespace Qurvey.Navigation
{
	public class MenuListGroupedData : ObservableCollection<NavigationGroup>
	{
		public MenuListGroupedData ()
		{
			NavigationGroup mainGroup = new NavigationGroup ("Main Menu");

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

			/*NavMenuItem NavCoursePage = new NavMenuItem () {
				Title = "Course Page",
				Icon = "l2plogo.png",
				CID = "14ws-44149",
				TargetType = typeof(pages.CourseRoomPage)
			};*/

			mainGroup.Add (NavConfig);
			mainGroup.Add (NavAuthorize);
			//courseGroup.Add (NavCoursePage);

			this.Add (mainGroup);
			//this.Add (courseGroup);
		}

		public bool HasCoursesGroup { get { return this.Count > 1; } }

		public NavigationGroup CourseGroup {
			get { 
				if (!HasCoursesGroup)
					AddCoursesGroup ();

				return this [1]; 
			} 
		}

		protected void AddCoursesGroup ()
		{
			if (!HasCoursesGroup) {
				NavigationGroup courseGroup = new NavigationGroup ("Courses");
				this.Add (courseGroup);
			}
		}
	}
}