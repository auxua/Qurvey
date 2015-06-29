using System;
using Qurvey.ViewModels;
using Xamarin.Forms;

namespace Qurvey.pages
{
	public partial class WelcomePage : ContentPage
	{
		public WelcomePage ()
		{
			BackgroundColor = Color.FromHex ("#2A2A2A");
			InitializeComponent ();
            // Not tested completely because Android Auth. is not working anymore!
            //NavigationPage.SetTitleIcon(this, "l2plogo.png");
            if (Device.OS == TargetPlatform.Android)
                NavigationPage.SetTitleIcon(this, "opac.png");
            
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
