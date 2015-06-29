using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qurvey.Shared.Models;
using Xamarin.Forms;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Net.Http;


namespace Qurvey.pages
{
	public partial class SurveyResultPage : ContentPage
	{
        private Survey survey;

        private TableRoot root;
        
        public SurveyResultPage (Survey survey)
		{
			BackgroundColor = Color.FromHex ("#2A2A2A");
			Title = "Survey results for";

            if (Device.OS == TargetPlatform.Android)
                NavigationPage.SetTitleIcon(this, "opac.png");
            IsBusy = true;

            this.survey = survey;

            Label questLabel = new Label
            {
                Text = survey.Question,
				FontAttributes = FontAttributes.Bold,
				FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label)),
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Button refreshButton = new Button
            {
                Text = "Refresh"
            };
            refreshButton.Clicked += refreshButton_Clicked;

            // Get results
            Result[] results;

            if ((Device.OS == TargetPlatform.Android) || (Device.OS == TargetPlatform.iOS))
            {
                // Android/iOS is working that way
                results = api.Backend.GetVoteResultAsync(survey).Result;
            }
            else
            {
                // WP will work hopefully that way
                results = api.Backend.GetVoteResultByIDAsync(survey.Id).Result;

            }

            root = new TableRoot();

            TableView table = new TableView
            {
                Intent = TableIntent.Form,
                Root = root
            };

            // Get the sum of all votes
            int sum = 0;
            foreach (var r in results)
            {
                sum += r.Count;
            }

            // create internal representation
            foreach (var r in results)
            {
                double quot = ((double)r.Count / (double)sum);
                TableSection ts = new TableSection(" ") {
                    new TextCell { Text=r.Answer.AnswerText + " (" + ((int)(quot * 100)) + " %)" },
                    new ViewCell { View = new ProgressBar { Progress=quot, HeightRequest=50, MinimumHeightRequest=50 }},
                };
                table.Root.Add(ts);
            }
			TableSection tss = new TableSection(" Total Amount of Votes: ") {
				new TextCell { Text=sum.ToString() }
			};
			table.Root.Add(tss);

            StackLayout stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Fill,
                Children = { questLabel, table, refreshButton },
            };

            Content = stack;

            IsBusy = false;

		}

        async void refreshButton_Clicked(object sender, EventArgs e)
        {
            await this.RefreshVotesAsync();
        }

        async Task RefreshVotesAsync()
        {
            // Get results
            var results = await api.Backend.GetVoteResultAsync(this.survey);

			if (results.Length == 0)
			{
				DisplayAlert ("No Votes", "At the moment, there are no Votes/Results available", "OK");
			}

            // Get the sum of all votes
            int sum = 0;
            foreach (var r in results)
            {
                sum += r.Count;
            }

            root.Clear();
            // create internal representation
            foreach (var r in results)
            {
                double quot = ((double)r.Count / (double)sum);
                TableSection ts = new TableSection(" ") {
                    new TextCell { Text=r.Answer.AnswerText + " (" + ((int)(quot * 100)) + " %)" },
                    new ViewCell { View = new ProgressBar { Progress=quot, HeightRequest=50, MinimumHeightRequest=50 }},
                };
                root.Add(ts);
            }
			TableSection tss = new TableSection(" Total Amount of Votes: ") {
				new TextCell { Text=sum.ToString() }
			};
			root.Add(tss);

            IsBusy = false;
        }
	}

    public class ProgressCell : ViewCell
    {

        public static readonly BindableProperty TextProperty =
         BindableProperty.Create("Text", typeof(string),
                                  typeof(ProgressCell),
                                  default(string));

        public static readonly BindableProperty ProgressProperty =
         BindableProperty.Create("Progress", typeof(double),
                                  typeof(ProgressCell),
                                  default(double));

        private Label label;

        private ProgressBar bar;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set 
            { 
                SetValue(TextProperty, value);
                label.Text = value;
            }
        }

        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set
            {
                SetValue(ProgressProperty, value);
                bar.ProgressTo(value, 250, Easing.Linear);
            }
        }

        public ProgressCell(string t, double d)
        {
            label = new Label
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Text = t
            };

            bar = new ProgressBar
            {
                //Progress = d,
                HorizontalOptions = LayoutOptions.Fill
            };

            StackLayout stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { label, bar },
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Fill
            };

            View = stack;

            bar.ProgressTo(d, 250, Easing.Linear);
        }

        public ProgressCell() : this("test", 0.5) { }

    }

    public class ProgressResult : INotifyPropertyChanged
    {
        public string Text { get; set; }
        public double Progress { get; set; }

        public ProgressResult(string text, double progress)
        {
            Text = text;
            Progress = progress;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
