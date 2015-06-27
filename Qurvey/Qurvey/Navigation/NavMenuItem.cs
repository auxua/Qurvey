using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Qurvey.Navigation
{
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
}

