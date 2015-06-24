using System;
using Qurvey.ViewModels;
using Xamarin.Forms;

namespace Qurvey.pages
{
	public partial class WelcomePage : ContentPage
	{
		public WelcomePage ()
		{
			InitializeComponent ();
			BindingContext = new WelcomeViewModel ();
		}

		async void authButton_Clicked(object sender, EventArgs e) {
			await DisplayAlert("Authorization", "You will be redirected to the RWTH-Authorization Page.\nPlease authorize the App and come back again.", "OK");
			// ugly. Better way of doing this?
			var vm = (WelcomeViewModel)BindingContext;
			vm.AuthorizeCommand.Execute (null);
		}
	}
}
