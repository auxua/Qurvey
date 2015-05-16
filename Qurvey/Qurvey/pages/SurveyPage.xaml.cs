using System;
using System.Collections.Generic;
using Qurvey.Shared.Models;

using Xamarin.Forms;

namespace Qurvey.pages
{
	public partial class SurveyPage : ContentPage
	{
		//public string[] SurveyAnswers { get; set; }

        private Survey survey;

        /// <summary>
        /// flag representing whether the user already answered
        /// TODO: typical use-case against the backend?
        /// </summary>
        private bool done = false;

		public SurveyPage (Survey survey)
		{
            this.survey = survey;
            
            //SurveyQuestions = new string[]{ "Option 1", "Option 2", "This is option 3", "Another option"};
			//InitializeComponent ();

            Label titleLabel = new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                XAlign = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Text = survey.Question
            };

            Label timeLabel = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                FontAttributes = FontAttributes.None,
                XAlign = TextAlignment.Center,
                //VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.Fill,
                Text = "Created at " + survey.Created.ToString() + " Last modified at " + survey.Modified.ToString()
            };

            ListView listview = new ListView
            {
                VerticalOptions = LayoutOptions.Start
            };
            listview.ItemTapped += listview_ItemTapped;

            //DataTemplate template = new DataTemplate(typeof(TextCell));
            //template.SetBinding(TextCell.TextProperty, "AnswerText");
			//listView.ItemsSource = survey.Answers;

            var Answers = new List<string>();
            foreach (Answer ans in survey.Answers)
            {
                // apply sorting, if needed...
                Answers.Add(ans.AnswerText);
            }

            listview.ItemsSource = Answers;

            // TODO: create own ViewCell to allow word wrapping in listview

            Title = "Survey";

            StackLayout stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                //Spacing = 5,
                VerticalOptions = LayoutOptions.Fill,
                Children = { titleLabel, listview, timeLabel }
            };

            /*ScrollView scroll = new ScrollView
            {
                Orientation = ScrollOrientation.Vertical,
                VerticalOptions = LayoutOptions.Fill,
                Content = stack
            };*/

            Padding = new Thickness(10, 20, 10, 4);

            Content = stack;

		}

        async void listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (done)
            {
                // Some platforms ignore the disabled-state of the view
                DisplayAlert("Not Allowed", "Sorry, you have already selected your answer", "OK");
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
                await api.Backend.SaveAnswer(survey, selected);
                ListView listView = (ListView)sender;
                done = true;
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() => DisplayAlert("Error", "Could not save answer. (Error: " + ex.Message + ")", "OK, I'll try again later"));
            }
        }
	}
}

