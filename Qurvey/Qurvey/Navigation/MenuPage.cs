using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Qurvey.Navigation
{
	/// <summary>
	/// The MenuPage is the Master Page of the MasterDetailPage.
	/// It contains the menu and gets colored correctly
	/// </summary>
	public class MenuPage : ContentPage
	{
		//public MenuListView Menu { get; set; }
		public GroupedMenuListView Menu { get; set; }

		//public MenuListView CoursesMenu { get; set; }

		public static ObservableCollection<NavigationGroup> MenuItems = new MenuListGroupedData ();

		public static void ClearMenu ()
		{
			MenuItems [1].Clear ();
		}

		public static void AddCourseToMenu (string cid, string title)
		{
			NavigationGroup group = MenuItems [1];

			if (group.ContainsCID (cid)) {
				return;
			}
			// Add the Course to the Menu
			NavMenuItem newCourse = new NavMenuItem ();
			newCourse.CID = cid;
			newCourse.Icon = "l2plogo.png";
			newCourse.TargetType = typeof(pages.CourseRoomPage);
			newCourse.Title = title;
			group.Add (newCourse);

		}

		public static int NumberOfCourses {
			get{ return MenuItems [1].Count; }
		}

		public static bool HasCourses {
			get { return NumberOfCourses >= 2; }
		}

		public MenuPage ()
		{
			Icon = "list.png";
			Title = "Navigation"; // The Title property must be set.
			// Use RWTH-Blue for Background
			BackgroundColor = Color.FromHex ("00549F");

			//Menu = new MenuListView (new MenuListData ());
			//CoursesMenu = new MenuListView (new CoursesListData ());

			var menuLabel = new ContentView {
				// Allow a bit padding to provide a better view
				//Padding = new Thickness (10, 5, 0, 5),
				Padding = new Thickness (10, 36, 0, 5),
				Content = new Label {
					// This color looks good on the Blue background
					TextColor = Color.FromHex ("DCDCDC"),
					Text = "Main Menu",
					FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label)),
					FontAttributes = FontAttributes.Bold
				}
			};

			MenuItems = new MenuListGroupedData ();
			Menu = new GroupedMenuListView ();
			Menu.ItemsSource = MenuItems;

			// Create the Layout of the page
			var layout = new StackLayout {
				Spacing = 0,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			layout.Children.Add (menuLabel);
			layout.Children.Add (Menu);

			Content = layout;
		}
	}
}