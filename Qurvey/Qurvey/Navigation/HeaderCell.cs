using System;
using Xamarin.Forms;

namespace Qurvey.Navigation
{
	public class HeaderCell : ViewCell
	{
		public HeaderCell ()
		{
			Height = 25;

			var title = new Label {
				FontSize = Device.GetNamedSize (NamedSize.Small, this),
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.White,
				VerticalOptions = LayoutOptions.Center
			};

			title.SetBinding (Label.TextProperty, "GroupName");

			View = new StackLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 25,
				BackgroundColor = Color.FromRgb (52, 152, 218),
				Padding = 5,
				Orientation = StackOrientation.Horizontal,
				Children = { title }
			};
		}
	}
}