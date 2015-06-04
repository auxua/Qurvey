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
	public partial class CourseRoomPage : NavigationPage
	{
        private string cid;

        private ObservableCollection<Survey> Surveys;

        private ListView SurveyList;

		private Label noSurveys;

		public CourseRoomPage (string course, string title)
		{
			InitializeComponent ();

            IsBusy = true;

            // get selected course room
            if ((course != null) && (course != ""))
            {
                cid = course;
            }
            else
            {
                cid = "Unknown CID";
            }
            
            // Now get the basic data from backend
            try
            {
                var surs = api.Backend.GetSurveysAsync(course).Result;
                Surveys = new ObservableCollection<Survey>(surs);
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(() => DisplayAlert("Failed", "Failed getting the course room surveys. "+e.Message, "damn..."));
                IsBusy = false;
                return;
            }

            // Worked - now create a page

            Label titleLabel = new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                Text = title
            };

            noSurveys = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                FontAttributes = FontAttributes.None,
                Text = "There are no surveys for this course",
				VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Button RefreshButton = new Button
            {
                Text = "Refresh"
            };

            RefreshButton.Clicked += RefreshButton_Clicked;

            DataTemplate cell = new DataTemplate(typeof(TextCell));
            cell.SetBinding(TextCell.TextProperty,"Question");
            //cell.SetBinding(TextCell.DetailProperty,"")

            SurveyList = new ListView
            {
                ItemTemplate = cell,
            };

            // Debugging-Reasons:
            //string[] data = { "test1", "testb", "wait", "uhh", "stuff" };
            SurveyList.ItemsSource = Surveys;
            //SurveyList.ItemSelected += list_ItemSelected;
            SurveyList.ItemTapped += SurveyList_ItemTapped;

            Button panicButton = new Button
            {
                Text = "PANIC",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                BackgroundColor = Color.Red,
				VerticalOptions = LayoutOptions.EndAndExpand
            };
            
            // For students, show panic button
            // For admins, show Create button
            if (App.isAdmin())
            {
                panicButton.Text = "Create new Survey";
                panicButton.Clicked += newSurveyButton_Clicked;
            }
            else
            {
                panicButton.Clicked += panicButton_Clicked;
            }

            /*activityIndicator = new ActivityIndicator();
            //activityIndicator.HorizontalOptions = LayoutOptions.CenterAndExpand;
            activityIndicator.IsRunning = true;
            activityIndicator.IsEnabled = true;
            activityIndicator.IsVisible = false;
            activityIndicator.BindingContext = this;
            //activityIndicator.SetBinding(ActivityIndicator.IsEnabledProperty, "IsBusy");
            //activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");
            activityIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");*/

            //this.isBusy = true;

            StackLayout stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Fill,
                Children = { titleLabel, RefreshButton, noSurveys, /*activityIndicator,*/ SurveyList, panicButton }
            };

			noSurveys.IsVisible = Surveys.Count == 0;
			SurveyList.IsVisible = Surveys.Count > 0;
            
            ContentPage content = new ContentPage();
            content.Content = stack;
            content.Title = title;
            content.Padding = new Thickness(10, 20, 10, 4);

            PushAsync(content);
            IsBusy = false;
		}

        /// <summary>
        /// On Button press, go to a page for creating surveys
        /// </summary>
        void newSurveyButton_Clicked(object sender, EventArgs e)
        {
            NewSurveyPage surveyPage = new NewSurveyPage(this.cid);
            this.PushAsync(surveyPage);
        }

        /// <summary>
        /// Panic Button pressed - send the panic to the server!
        /// </summary>
        async void panicButton_Clicked(object sender, EventArgs e)
        {
            try
            {
				Panic panic = new Panic(cid, BackendAuthManager.Instance.User);
				await api.Backend.SavePanicAsync(panic);
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() => DisplayAlert("Error", "Could not send the Panic! (Error: "+ex.Message+" )", "OK"));
            }
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
            
            PushAsync(sp);
        }

        /*void list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Selected one of the Surveys -> Show a new page for that
            if (SurveyList.SelectedItem == null)
            {
                // The event may get fired twice - invalidate the second!
                return;
            }

            Survey selectedSurvey = (Survey)SurveyList.SelectedItem;
            //SurveyList.ItemSelected -= list_ItemSelected;
            // setting the selectedItem to null allows user to select the same item again next time, but will crash on Android
            //SurveyList.SelectedItem = null;
            //SurveyList.ItemSelected += list_ItemSelected;
            SurveyPage sp = new SurveyPage(selectedSurvey);
            PushAsync(sp);
        }*/

        /// <summary>
        /// Get the currently active surveys from the server
        /// </summary>
        private async void RefreshButton_Clicked(object sender, EventArgs e)
        {
            this.IsBusy = true;
            //this.PushAsync(new pages.WelcomePage());
            // refresh list of Surveys
            //Surveys.RemoveAt(1);

            Survey[] sur = await api.Backend.GetSurveysAsync(cid);
            Surveys.Clear();
            // No Surveys available?
            if (sur == null)
            {
                IsBusy = false;
                return;
            }
            // If there are Surveys, add them!
            foreach (Survey survey in sur)
            {
                Surveys.Add(survey);
            }
			noSurveys.IsVisible = Surveys.Count == 0;
			SurveyList.IsVisible = Surveys.Count > 0;

            this.IsBusy = false;
        }
	}
}
