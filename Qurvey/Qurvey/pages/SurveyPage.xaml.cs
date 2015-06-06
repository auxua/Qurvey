using System;
using System.Collections.Generic;
using Qurvey.Shared.Models;

using Xamarin.Forms;
using Qurvey.api;

namespace Qurvey.pages
{
	public partial class SurveyPage : ContentPage
	{
        private Survey survey;

        /// <summary>
        /// flag representing whether the user already answered
        /// TODO: typical use-case against the backend?
        /// </summary>
        private bool done = false;

		public SurveyPage (Survey survey)
		{
			InitializeComponent ();
			BindingContext = new ViewModels.SurveyViewModel (survey);
		}

        /*void listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
			
           if (done)
            {
                // Some platforms ignore the disabled-state of the view
                await DisplayAlert("Not Allowed", "Sorry, you have already selected your answer", "OK");
                return;
            }
            string selectedText = (string)e.Item;
            
            List<Answer> list = new List<Answer>(survey.Answers);
            Answer selected = list.Find(x => x.AnswerText == selectedText);

            bool sure = await DisplayAlert("Confirm Selection", "Are you sure, you want to select this Option?", "Yes", "No");
            // Not sure? handle control back again to the user
            if (!sure)
                return;
            // sure! Let's tell the backend
            try
            {
				Vote vote = new Vote(BackendAuthManager.Instance.User, survey, selected);
                await api.Backend.SaveVoteAsync(vote);
                ListView listView = (ListView)sender;
                done = true;
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() => DisplayAlert("Error", "Could not save answer. (Error: " + ex.Message + ")", "OK, I'll try again later"));
            }
        }*/
	}
}

