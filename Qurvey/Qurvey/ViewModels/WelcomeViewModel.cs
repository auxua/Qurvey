using System;
using System.ComponentModel;

namespace Qurvey.ViewModels
{
	public class WelcomeViewModel : INotifyPropertyChanged
	{
		private string title;
		public string Title {
			get {
				return this.title;
			}
			set {
				this.title = value;
				RaisePropertyChanged ("Title");
			}
		}

		public WelcomeViewModel ()
		{
			Title = "Bla bla";
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged(string propName) {
			if (PropertyChanged != null) {
				PropertyChanged (this, new PropertyChangedEventArgs (propName));
			}
		}
	}
}

