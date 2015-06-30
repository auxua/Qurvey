using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Qurvey.Shared.Models;

using Xamarin.Forms;

namespace Qurvey.pages
{
	public partial class NewSurveyPage : ContentPage
	{
        private Editor editor;

        private Entry e1, e2, e3, e4;

        private string cid;
        
        public NewSurveyPage (string CID)
		{
			//InitializeComponent ();

			if (Device.OS == TargetPlatform.Android) 
			{
				NavigationPage.SetTitleIcon (this, "opac.png");
				BackgroundColor = Color.FromHex ("#2A2A2A");
			}
            cid = CID;

            Label titleLabel = new Label
            {
                Text = "Create new Survey",
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            editor = new Editor
            {
                Text = "Please insert the question here\n\n\n",
                HorizontalOptions = LayoutOptions.Fill
            };
            if (Device.OS == TargetPlatform.WinPhone)
            {
                editor.BackgroundColor = Color.White;
            }

            Label optionsLabel = new Label
            {
                Text = "Options/Answers",
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            //TODO: allow dynmic adding of options for the survey!
            e1 = new Entry { Text = "Option/Answer 1" };
            e1.Completed += (o, s) => { e2.Focus(); };
            e2 = new Entry { Text = "Option/Answer 2" };
            e2.Completed += (o, s) => { e3.Focus(); };
            e3 = new Entry { Text = "" };
            e3.Completed += (o, s) => { e4.Focus(); };
            e4 = new Entry { Text = "" };
            

            Button createButton = new Button
            {
                Text = "Create Survey",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            createButton.Clicked += createButton_Clicked;

            e4.Completed += (o, s) => { createButton.Focus(); };

            StackLayout stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 30,
                Children = { titleLabel, editor, optionsLabel, e1, e2, e3, e4, createButton },
                VerticalOptions = LayoutOptions.Fill
            };

            Padding = new Thickness(15, 10, 15, 5);
            ScrollView scroll = new ScrollView { Content = stack };
            Content = scroll;
		}

        async void createButton_Clicked(object sender, EventArgs e)
        {
            IsBusy = true;
            
            // verify data of field
            if ((editor.Text == "") || (e1.Text == "") || (e2.Text == ""))
            {
                Device.BeginInvokeOnMainThread(() => DisplayAlert("Error","Please provide at least a question and two options","OK") );
                IsBusy = false;
                return;
            }

            // Create data representation
            Answer a1 = new Answer(e1.Text, 0);
            Answer a2 = new Answer(e2.Text, 1);
            List<Answer> answers = new List<Answer>();
            answers.Add(a1);
            answers.Add(a2);
            if (e3.Text != "")
            {
                Answer a3 = new Answer(e3.Text, 2);
                answers.Add(a3);
            }
            if (e4.Text != "")
            {
                Answer a4 = new Answer(e4.Text, 3);
                answers.Add(a4);
            }
            Survey sur = new Survey(editor.Text.Trim());
            sur.Answers = answers;
            sur.Status = Survey.SurveyStatus.Published; //TODO: workflow for non-published surveys
			sur.Course = cid;

            // Send Survey to Server
            try
            {
                await api.Backend.SaveSurveyAsync(sur);
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() => DisplayAlert("Error", "An Error occured while saving the survey: " + ex.Message, "OK"));
                IsBusy = false;
                return;
            }

            // Everythin worked fine!
            Device.BeginInvokeOnMainThread(() => DisplayAlert("Success!", "The Survey was added", "OK"));
            IsBusy = false;

        }
	}
}
