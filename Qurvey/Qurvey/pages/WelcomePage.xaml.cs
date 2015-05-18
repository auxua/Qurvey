using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Qurvey.api.DataModel;
using Qurvey.api;

using Xamarin.Forms;

namespace Qurvey.pages
{
	public partial class WelcomePage : ContentPage
	{

        private Label statusLabel;

        public string AuthStatus
        {
            get { return (string)GetValue(AuthStatusProperty); }
            set { SetValue(AuthStatusProperty, value); }
        }

        public static BindableProperty AuthStatusProperty = BindableProperty.Create<WelcomePage, string>(w => w.AuthStatus, "not Authorized");

		public WelcomePage ()
		{
			//InitializeComponent ();

            Label titleLabel = new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                Text = "Welcome To Qurvey"
            };

            Label descriptionLabel = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                FontAttributes = FontAttributes.None,
                Text = "\n\n At the moment, the App is trying to get all your course rooms from L2P. If you haven't authorized the app yet, or the authorization has expired, you will be redirected to the authorization page of RWTH\n\n",
            };

            Button authButton = new Button
            {
                Text = "Authorize this App",
            };
            authButton.Clicked += authButton_Clicked;

            Button getButton = new Button
            {
                Text = "Get Courses",
            };
            getButton.Clicked += getButton_Clicked;

            Label AuthLabel = new Label
            {
                XAlign = TextAlignment.Center
            };
            AuthLabel.SetBinding(Label.TextProperty, "AuthStatus");
            AuthLabel.BindingContext = this;

            statusLabel = new Label
            {
                Text = "Checking Tokens...",
                XAlign = TextAlignment.Center,
            };

            StackLayout stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(20, 20, 10, 5),
                Children = { titleLabel, descriptionLabel, AuthLabel, authButton, getButton, statusLabel },
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            Content = stack;

            Title = "Welcome";

            api.AuthenticationManager.CheckStateAsync();
            if (api.AuthenticationManager.getState() == api.AuthenticationManager.AuthenticationState.ACTIVE)
            {
                this.AuthStatus = "authorized";
                //Then, get all Courses...
                this.getButton_Clicked(this, null);
            }

            
		}

        async void getButton_Clicked(object sender, EventArgs e)
        {
            IsBusy = true;
            GetAllData();
            IsBusy = false;
        }

        async void authButton_Clicked(object sender, EventArgs e)
        {
            api.AuthenticationManager.CheckStateAsync();
            if (api.AuthenticationManager.getState() != api.AuthenticationManager.AuthenticationState.ACTIVE)
            {
                await DisplayAlert("Authorization", "You will be redirected to the RWTH-Authorization Page.\n Please authorize the App and come back again.", "OK");
                statusLabel.Text += "\nNo valid Token found";
                //Device.BeginInvokeOnMainThread(() => DisplayAlert("Authorization needed", "The App is not authorized at the moment. You will be redirected to the authorization page of RWTH. Please come back right after", "OK"));
                string url = await api.AuthenticationManager.StartAuthenticationProcessAsync();
                if (String.Empty == url)
                {
                    // Error
                    statusLabel.Text += "\nError!";
                }
                this.AuthStatus = "Authorization pending...";
                Device.OpenUri(new Uri(url));
            }
            else
            {
                await DisplayAlert("Authorization", "The app is already authorized", "OK");
                //statusLabel.Text += "Bla";
            }
            int i = 0;
            this.statusLabel.Text += "\n going to check";
            while (i<50) // just try to check authorize sometimes in case of slow device
            {
                i++;
                Thread.Sleep(1000);
                bool auth = await api.AuthenticationManager.CheckAuthenticationProgressAsync();
                if (auth)
                {
                    this.AuthStatus = "authorized";
                    return;
                }
                this.statusLabel.Text += "\n Checked";
            }
            // If we get here, the user did not authorize the app
            this.AuthStatus = "not authorized";
            //statusLabel.Text += "\n in manager: "+api.AuthenticationManager.getState();
        }

        private async void GetAllData()
        {
            bool useAll = App.isUsingAllCourses();
            IsBusy = true;
            
            if (api.AuthenticationManager.getState() != api.AuthenticationManager.AuthenticationState.ACTIVE)
            {
                Device.BeginInvokeOnMainThread(() => DisplayAlert("Authorization needed", "Please authorize the App first", "OK"));
                IsBusy = false;
                return;
            }

            //var c = new System.Net.Http.HttpClient();

            // Get the courses!
            L2PCourseInfoSetData courses;
            if (useAll)
            {
                courses = await api.RESTCalls.L2PViewAllCourseInfoAsync();
            }
            else
            {
                string semester;
                if (DateTime.Now.Month < 4)
                {
                    // winter semester but already in new year -> ws+last year
                    semester = "ws" + DateTime.Now.AddYears(-1).Year.ToString().Substring(2);
                }
                else if (DateTime.Now.Month > 9)
                {
                    // winter semester, still in this year
                    semester = "ws" + DateTime.Now.Year.ToString().Substring(2);
                }
                else
                {
                    // summer semester
                    semester = "ss" + DateTime.Now.Year.ToString().Substring(2);
                }
                courses = await api.RESTCalls.L2PViewAllCourseIfoBySemesterAsync(semester);
            }
            
            // Workaround (should be moved to Manager)
            if (courses == null)
            {
                await api.AuthenticationManager.GenerateAccessTokenFromRefreshTokenAsync();
                courses = await api.RESTCalls.L2PViewAllCourseInfoAsync();
            }

            if (courses.dataset == null)
            {
                Device.BeginInvokeOnMainThread(() => DisplayAlert("Error", "An error occured: " + courses.errorDescription, "OK"));
                IsBusy = false;
                return;
            }

            MenuPage.ClearMenu();
            foreach (L2PCourseInfoData l2pcourse in courses.dataset)
            {
                MenuPage.AddCourseToMenu(l2pcourse.uniqueid, l2pcourse.courseTitle);
            }
            IsBusy = false;
            
        }
	}
}
