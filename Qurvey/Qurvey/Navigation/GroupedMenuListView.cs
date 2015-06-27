using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Qurvey.Navigation
{
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
			GroupHeaderTemplate = new DataTemplate (typeof(HeaderCell));

			//ItemsSource = data;
			VerticalOptions = LayoutOptions.FillAndExpand;
			BackgroundColor = Color.Transparent;

			// Set Bindings
			var cell = new DataTemplate (typeof(MenuImageCell));
			cell.SetBinding (TextCell.TextProperty, "Title");
			// Image Binding possible in here..
			// cell.SetBinding(ImageCell.ImageSourceProperty, "IconSource");

			ItemTemplate = cell;
			// The first item is selected - the lines 2 and 3 are only for the color-fix...
			//SelectedItem = data[0];
			//data[0][0].SetColors(false);
			//selected = data[0][0];
		}

		public void SetItemSource (ObservableCollection<NavigationGroup> source)
		{
			this.ItemsSource = source;
			source [0] [0].SetColors (false);
			selected = source [0] [0];
		}
	}
}

