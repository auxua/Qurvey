using System;
using Qurvey.Shared.Models;
using Xamarin.Forms;
using Qurvey.ViewModels;

namespace Qurvey.pages
{
	public partial class CourseRoomPage : ContentPage
	{
		public CourseRoomPage (string course, string title)
		{
			InitializeComponent ();
			//BackgroundColor = Color.FromHex ("#2A2A2A");
            if (Device.OS == TargetPlatform.Android)
                NavigationPage.SetTitleIcon(this, "opac.png");
			BindingContext = new CourseRoomPageViewModel(course, title, this.Navigation, BackgroundColor);

		}

        /// <summary>
        /// User wants to see a survey
        /// </summary>
        void SurveyList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // Use Tapped instead of selected to allow the re-selection of the item without crashing Android
			SurveyCellViewModel cell = (SurveyCellViewModel)e.Item;
			cell.BackgroundColor = Color.FromHex ("#FF4F45");
			Survey item = cell.Survey;
            Page sp;
            if (App.isAdmin())
            {
                if (Device.OS == TargetPlatform.WinPhone)
                {
                    Device.BeginInvokeOnMainThread(() => DisplayAlert("Not supported", "Due to technical reasons, this Action is not supported on WindowsPhone at the moment.", "OK"));
                    return;
                }
                sp = new SurveyResultPage(item);
            }
            else
            {
                sp = new SurveyPage(item);
            }
			cell.BackgroundColor = this.BackgroundColor;
			Navigation.PushAsync (sp);
        }

		public void OnDelete (object sender, EventArgs e) {
			if (!App.isAdmin ()) {
				Device.BeginInvokeOnMainThread(() => DisplayAlert("Forbidden", "Only course managers can delete surveys", "OK"));
				return;
			}
				
			//Xamarin doesnt support the necessary Bindings
			var mi = (MenuItem)sender;
			SurveyCellViewModel surveyCellViewModel = (SurveyCellViewModel)mi.CommandParameter;
			var vm = (CourseRoomPageViewModel)this.BindingContext;
			vm.DeleteSurveyExecute (surveyCellViewModel.Survey);
		}
	}
}
