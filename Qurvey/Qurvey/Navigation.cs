using System;
using System.Collections.Generic;
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
        public RootPage()
        {
            var menuPage = new MenuPage();
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
            Detail = new NavigationPage(new pages.WelcomePage());
        }

        /// <summary>
        /// Depending on the selected item, go to the corresponding page
        /// </summary>
        /// <param name="menu">the menuitem, that was selected</param>
        void NavigateTo(NavMenuItem menu)
        {

            try
            {
                Page displayPage = (Page)Activator.CreateInstance(menu.TargetType);
                Detail = new NavigationPage(displayPage);

                IsPresented = false;
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(() => { DisplayAlert("Error", "Could not load Page: "+e.Message, "Damn!"); });
            }

        }
    }


    /// <summary>
    /// The MenuPage is the Master Page of the MasterDetailPage.
    /// It contains the menu and gets colored correctly
    /// </summary>
    public class MenuPage : ContentPage
    {
        public MenuListView Menu { get; set; }

        public MenuPage()
        {
            Icon = "list.png";
            Title = "Navigation"; // The Title property must be set.
            // Use RWTH-Blue for Background
            BackgroundColor = Color.FromHex("00549F");

            Menu = new MenuListView();

            var menuLabel = new ContentView
            {
                // Allow a bit padding to provide a better view
                Padding = new Thickness(10, 36, 0, 5),
                Content = new Label
                {
                    // This color looks good on the Blue background
                    TextColor = Color.FromHex("DCDCDC"),
                    Text = "Menu",
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                    FontAttributes = FontAttributes.Bold
                }
            };

            // Create the Layout of the page
            var layout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            layout.Children.Add(menuLabel);
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

        // below: Event(handling) for (de)selecting the cell.
        // This is used for creating consistency in color schemes between different platforms

        public event PropertyChangedEventHandler PropertyChanged;

        private Color _backgroundColor;

        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("BackgroundColor"));
                }
            }
        }

        public void SetColors(bool isSelected)
        {
            if (isSelected)
            {
                // Use RWTH-Blue
                BackgroundColor = Color.FromHex("00549F");
            }
            else
            {
                // Use RWTH light-blue
                BackgroundColor = Color.FromHex("8EBAE5");
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

        public MenuImageCell()
            : base()
        {
            Label Text = new Label
            {
                TextColor = Color.FromHex("DCDCDC"),
                XAlign = TextAlignment.Start,
                YAlign = TextAlignment.Center
            };
            Text.SetBinding(Label.TextProperty, "Title");

            Label pad = new Label
            {
                Text = " "
            };

            Image image = new Image
            {
                // Backup: Default icon to set
                //Source = "info.png",
                HeightRequest = 30,
            };
            image.SetBinding(Image.SourceProperty, "Icon");

            var layout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                Children = { pad, image, pad, Text }
            };
            layout.SetBinding(Layout.BackgroundColorProperty, new Binding("BackgroundColor"));

            if (Device.OS == TargetPlatform.WinPhone)
                layout.HeightRequest = 50;

            View = layout;
        }
    }

    /// <summary>
    /// The menu is created using a adapted Listview.
    /// By that most of the stuff is already done by default ;)
    /// </summary>
    public class MenuListView : ListView
    {
        public NavMenuItem selected { get; set; }

        public MenuListView()
        {
            List<NavMenuItem> data = new MenuListData();

            ItemsSource = data;
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
            data[0].SetColors(false);
            selected = data[0];
        }
    }

    /// <summary>
    /// Here, we provide the technical navigation.
    /// The Title and corresponding pages are added to the menu.
    /// </summary>
    public class MenuListData : List<NavMenuItem>
    {
        public MenuListData()
        {
            this.Add(new NavMenuItem()
            {
                Title = "Welcome",
                Icon = "info.png",
                TargetType = typeof(pages.WelcomePage)
            });

            this.Add(new NavMenuItem()
            {
                Title = "Config",
                Icon = "info.png",
                TargetType = typeof(pages.ConfigPage)
            });

            this.Add(new NavMenuItem()
            {
                Title = "About",
                Icon = "info.png",
                TargetType = typeof(pages.WelcomePage)
            });

        }
    }

}
