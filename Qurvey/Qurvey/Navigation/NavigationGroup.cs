using System;
using System.Collections.ObjectModel;

namespace Qurvey.Navigation
{
	/// <summary>
	/// Simple Representation of the Groups for Listview-Group-Binding
	/// </summary>
	public class NavigationGroup : ObservableCollection<NavMenuItem>
	{
		public string GroupName { get; private set; }

		public bool ContainsCID (string cid)
		{
			foreach (NavMenuItem item in this.Items) {
				if (item.CID == cid)
					return true;
			}
			return false;
		}

		public NavigationGroup (string gTitle)
		{
			GroupName = gTitle;
		}

		public void AddCourse (string cid, string title)
		{
			if (ContainsCID (cid)) {
				return;
			}
			// Add the Course to the Menu
			NavMenuItem newCourse = new NavMenuItem ();
			newCourse.CID = cid;
			newCourse.Icon = "l2plogo.png";
			newCourse.TargetType = typeof(pages.CourseRoomPage);
			newCourse.Title = title;
			Add (newCourse);
		}
	}
}