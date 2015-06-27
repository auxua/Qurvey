using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qurvey.api;
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
			BindingContext = new CourseRoomPageViewModel(course, title, this.Navigation);
		}

        /// <summary>
        /// User wants to see a survey
        /// </summary>
        void SurveyList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // Use Tapped instead of selected to allow the re-selection of the item without crashing Android
            var item = (Survey)e.Item;
            Page sp;
            if (App.isAdmin())
            {
                sp = new SurveyResultPage(item);
            }
            else
            {
                if (Device.OS == TargetPlatform.WinPhone)
                {
                    Device.BeginInvokeOnMainThread(() => DisplayAlert("Not supported", "Due to technical reasons, this Action is not supported on WindowsPhone at the moment.", "OK"));
                    return;
                }
                sp = new SurveyPage(item);
            }
			Navigation.PushAsync (sp);
			// TODO unselect item
        }

		public void OnDelete (object sender, EventArgs e) {
			if (!App.isAdmin ()) {
				Device.BeginInvokeOnMainThread(() => DisplayAlert("Forbidden", "Only course managers can delete surveys", "OK"));
				return;
			}
				
			//Xamarin doesnt support the necessary Bindings
			var mi = (MenuItem)sender;
			Survey survey = (Survey)mi.CommandParameter;
			var vm = (CourseRoomPageViewModel)this.BindingContext;
			vm.DeleteSurveyExecute (survey);
		}
	}
}
