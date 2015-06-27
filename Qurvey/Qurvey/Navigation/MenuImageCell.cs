using System;
using Xamarin.Forms;

namespace Qurvey.Navigation
{
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

			image.SetBinding (Image.SourceProperty, "Icon");

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
}

