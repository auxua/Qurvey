using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qurvey.api;
using Qurvey.Shared.Models;
using Xamarin.Forms;

namespace Qurvey.pages
{
	public partial class CourseRoomPage : ContentPage
	{
		public CourseRoomPage (string course, string title)
		{
			InitializeComponent ();
			BindingContext = new ViewModels.CourseRoomPageViewModel(course, title, this.Navigation);
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
                sp = new SurveyPage(item);
            }
			Navigation.PushAsync (sp);
        }
	}
}
