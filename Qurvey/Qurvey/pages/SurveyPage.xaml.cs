using System;
using System.Collections.Generic;
using Qurvey.Shared.Models;
using Xamarin.Forms;
using Qurvey.api;

namespace Qurvey.pages
{
	public partial class SurveyPage : ContentPage
	{
		public SurveyPage (Survey survey)
		{
			//BackgroundColor = Color.FromHex ("#2A2A2A");
			InitializeComponent ();
            Title = "Survey";
            if (Device.OS == TargetPlatform.Android)
                NavigationPage.SetTitleIcon(this, "opac.png");
			BindingContext = new ViewModels.SurveyViewModel (survey, BackgroundColor);
            AnswerList.ItemSelected += AnswerList_ItemSelected;
		}

        void AnswerList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (Device.OS == TargetPlatform.Android)
            {
                
            }
        }
	}
}

