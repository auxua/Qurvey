using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Qurvey
{
	public class RootPage : MasterDetailPage
	{
		/// <summary>
		/// Create the Root Page
		/// Define master and Detail as wenn as the default Detail page
		/// </summary>
		public RootPage ()
		{
			var menuPage = new MenuPage ();
			// The effective navigation gets assigned
			menuPage.Menu.ItemSelected += (sender, e) =>
            {
                // Before Navigating, recolor the cell
                if (menuPage.Menu.selected != null)
                {
                    menuPage.Menu.selected.SetColors(true);
                }

                // Select new
                menuPage.Menu.selected = (menuPage.Menu.SelectedItem as NavMenuItem);
                menuPage.Menu.selected.SetColors(false);

                NavigateTo(e.SelectedItem as NavMenuItem);
            };

			Master = menuPage;
			// Set default Detail page
			Detail = new NavigationPage (new pages.WelcomePage ());
		}


		/// <summary>
		/// Depending on the selected item, go to the corresponding page
		/// </summary>
		/// <param name="menu">the menuitem, that was selected</param>
		void NavigateTo (NavMenuItem menu)
		{

			try {
                /*if (Detail is pages.CourseRoomPage)
                {
                    ((pages.CourseRoomPage)Detail).Navigation.PopToRootAsync();
                }*/

                // If there is a Navigation inside the Page, Pop back to allow a working Navigation Stack
                if (Detail.Navigation != null)
                    Detail.Navigation.PopToRootAsync();
                
                if (menu.TargetType == typeof(pages.CourseRoomPage))
                {
                    //Application.Current.Properties["currentCID"] = menu.CID;
                    Detail = new NavigationPage(new pages.CourseRoomPage(menu.CID,menu.Title));
                    IsPresented = false;
                    return;
                }
				Page displayPage = (Page)Activator.CreateInstance (menu.TargetType);
				Detail = new NavigationPage (displayPage);

				IsPresented = false;
			} catch (Exception e) {
				Device.BeginInvokeOnMainThread (() => {
					DisplayAlert ("Error", "Could not load Page: " + e.Message, "Damn!");
				});
			}

		}
	}

	/// <summary>
	/// The MenuPage is the Master Page of the MasterDetailPage.
	/// It contains the menu and gets colored correctly
	/// </summary>
	public class MenuPage : ContentPage
	{
		//public MenuListView Menu { get; set; }
        public GroupedMenuListView Menu { get; set; }

		//public MenuListView CoursesMenu { get; set; }

        public static ObservableCollection<NavigationGroup> MenuItems = new MenuListGroupedData();

        public static void ClearMenu()
        {
            MenuItems[1].Clear();
        }

        public static void AddCourseToMenu(string cid, string title)
        {
            NavigationGroup group = MenuItems[1];

            if (group.ContainsCID(cid))
            {
                return;
            }
            // Add the Course to the Menu
            NavMenuItem newCourse = new NavMenuItem();
            newCourse.CID = cid;
            newCourse.Icon = "l2plogo.png";
            newCourse.TargetType = typeof(pages.CourseRoomPage);
            newCourse.Title = title;
            group.Add(newCourse);
            
        }

        public static int NumberOfCourses
        {
			get{return MenuItems [1].Count;}
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
                Padding = new Thickness(10, 36, 0, 5),
				Content = new Label {
					// This color looks good on the Blue background
					TextColor = Color.FromHex ("DCDCDC"),
					Text = "Main Menu",
					FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label)),
					FontAttributes = FontAttributes.Bold
				}
			};

            MenuItems = new MenuListGroupedData();
            Menu = new GroupedMenuListView();
            Menu.ItemsSource = MenuItems;

			// Create the Layout of the page
			var layout = new StackLayout {
				Spacing = 0,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			layout.Children.Add (menuLabel);
            layout.Children.Add(Menu);

			Content = layout;
		}
	}

	/// <summary>
	/// The MenuItem represents the menuitems and stores the corresponding target page.
	/// It can be extended by an Icon, e.g.
	/// </summary>
	public class NavMenuItem : INotifyPropertyChanged
	{
		/// <summary>
		/// The Title that is shown
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// The Icon besides the Menu item title
		/// </summary>
		public string Icon { get; set; }

		/// <summary>
		/// The page that should be opened when selecting this item
		/// </summary>
		public Type TargetType { get; set; }

        /// <summary>
        /// The Course room ID - only used for course rooms
        /// </summary>
        public string CID { get; set; }

		// below: Event(handling) for (de)selecting the cell.
		// This is used for creating consistency in color schemes between different platforms

		public event PropertyChangedEventHandler PropertyChanged;

		private Color _backgroundColor;

		public Color BackgroundColor {
			get { return _backgroundColor; }
			set {
				_backgroundColor = value;

				if (PropertyChanged != null) {
					PropertyChanged (this, new PropertyChangedEventArgs ("BackgroundColor"));
				}
			}
		}

		public void SetColors (bool isSelected)
		{
			if (isSelected) {
				// Use RWTH-Blue
				BackgroundColor = Color.FromHex ("00549F");
			} else {
				// Use RWTH light-blue
				BackgroundColor = Color.FromHex ("8EBAE5");
			}
		}
	}

	/// <summary>
	/// The view (cell) used in the menu for the menu item.
	/// Inherits from ViewCell the get access to the Layout
	/// (inherited from Imagecell in the past for possibility of images in some time)
	/// </summary>
	public class MenuImageCell : ViewCell
	{

		public MenuImageCell ()
			: base ()
		{
			Label Text = new Label {
				TextColor = Color.FromHex ("DCDCDC"),
				XAlign = TextAlignment.Start,
				YAlign = TextAlignment.Center
			};
			Text.SetBinding (Label.TextProperty, "Title");

			Label pad = new Label {
				Text = " "
			};

			Image image = new Image {
				// Backup: Default icon to set
				//Source = "info.png",
				HeightRequest = 30,
			};
			//image.SetBinding (Image.SourceProperty, "Icon");

			var layout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Center,
				Children = { pad, image, pad, Text }
			};
			layout.SetBinding (Layout.BackgroundColorProperty, new Binding ("BackgroundColor"));

			if (Device.OS == TargetPlatform.WinPhone)
				layout.HeightRequest = 50;

			View = layout;
		}
	}

    /// <summary>
    /// Simple Representation of the Groups for Listview-Group-Binding
    /// </summary>
    public class NavigationGroup : ObservableCollection<NavMenuItem>
    {
        public string GroupName { get; private set; }

        public bool ContainsCID(string cid)
        {
            foreach (NavMenuItem item in this.Items)
            {
                if (item.CID == cid)
                    return true;
            }
            return false;
        }
        
        public NavigationGroup(string gTitle)
        {
            GroupName = gTitle;
        }
    }

	/// <summary>
	/// The menu is created using a adapted Listview.
	/// By that most of the stuff is already done by default.
    /// Supports Groups
	/// </summary>
    public class GroupedMenuListView : ListView
    {
        public NavMenuItem selected { get; set; }

        public GroupedMenuListView ()
        {
            IsGroupingEnabled = true;
            //GroupDisplayBinding = new Binding("GroupName");
            //GroupShortNameBinding = new Binding("GroupName");
            GroupHeaderTemplate = new DataTemplate(typeof(HeaderCell));

            //ItemsSource = data;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.Transparent;

            // Set Bindings
            var cell = new DataTemplate(typeof(MenuImageCell));
            cell.SetBinding(TextCell.TextProperty, "Title");
            // Image Binding possible in here..
            // cell.SetBinding(ImageCell.ImageSourceProperty, "IconSource");

            ItemTemplate = cell;
            // The first item is selected - the lines 2 and 3 are only for the color-fix...
            //SelectedItem = data[0];
            //data[0][0].SetColors(false);
            //selected = data[0][0];
        }

        public void SetItemSource(ObservableCollection<NavigationGroup> source)
        {
            this.ItemsSource = source;
            source[0][0].SetColors(false);
            selected = source[0][0];
        }
    }

    public class MenuListGroupedData : ObservableCollection<NavigationGroup>
    {
        public MenuListGroupedData()
        {
            NavigationGroup mainGroup = new NavigationGroup("Main Menu");
            NavigationGroup courseGroup = new NavigationGroup("Courses");

            NavMenuItem NavConfig = new NavMenuItem()
            {
                Title = "Welcome",
                Icon = "info.png",
                TargetType = typeof(pages.WelcomePage)
            };

            NavMenuItem NavAuthorize = new NavMenuItem()
            {
                Title = "Config",
                Icon = "info.png",
                TargetType = typeof(pages.ConfigPage)
            };

            NavMenuItem NavCoursePage = new NavMenuItem()
            {
                Title = "Course Page",
                Icon = "l2plogo.png",
                CID = "14ws-44149",
                TargetType = typeof(pages.CourseRoomPage)
            };

            mainGroup.Add(NavConfig);
            mainGroup.Add(NavAuthorize);
            courseGroup.Add(NavCoursePage);

            this.Add(mainGroup);
            this.Add(courseGroup);
        }
    }

    public class HeaderCell : ViewCell
    {
        public HeaderCell()
        {
            Height = 25;

            var title = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, this),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center
            };

            title.SetBinding(Label.TextProperty, "GroupName");

            View = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 25,
                BackgroundColor = Color.FromRgb(52, 152, 218),
                Padding = 5,
                Orientation = StackOrientation.Horizontal,
                Children = { title }
            };
        }
    }

}
