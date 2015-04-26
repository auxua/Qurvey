using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Qurvey.pages
{
	public partial class AuthorizePage : ContentPage
	{
        Label statusLabel;
        //WebView wv;

        public AuthorizePage ()
		{
			//InitializeComponent ();
            Title = "Authorize";

            StackLayout stack;

            /*wv = new WebView();
            wv.Source = "http://www.rwth-aachen.de";
            wv.MinimumHeightRequest = 200;
            //wv.IsEnabled = false;
            //wv.IsVisible = false;

            */

            statusLabel = new Label();
            statusLabel.Text = "waiting...";
            Button but = new Button();
            but.Text = "Start Authorization";
            but.Clicked += OnStartButtonClicked;

            Button CheckBut = new Button();
            CheckBut.Text = "Check Token";
            CheckBut.Clicked += OnCheckButtonClicked;

            Button PingBut = new Button();
            PingBut.Text = "Ping L2P";
            PingBut.Clicked += OnPingButClicked;

            Button Courses = new Button();
            Courses.Text = "Get Courses (show count)";
            Courses.Clicked += OnCoursesClicked;

            Button OneCourse = new Button();
            OneCourse.Text = "Get Course (14ws-44150)";
            OneCourse.Clicked += OnOneCourseClicked;

            Button Role = new Button();
            Role.Text = "Get Role (14ws-44150)";
            Role.Clicked += OnRoleClicked;

            Button RefreshBut = new Button();
            RefreshBut.Text = "Refresh Token manually";
            RefreshBut.Clicked += OnRefreshClicked;

            stack = new StackLayout {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { but,/*wv,*/ CheckBut, PingBut, Courses, OneCourse, Role, RefreshBut, statusLabel }
            };

            ScrollView scrollView = new ScrollView();
            scrollView.VerticalOptions = LayoutOptions.FillAndExpand;
            //scrollView.MinimumHeightRequest = 200;
            scrollView.Content = stack;

            Content = scrollView;
		}

        private async void OnRefreshClicked(object sender, EventArgs e)
        {
            var ret = await api.AuthenticationManager.GenerateAccessTokenFromRefreshTokenAsync();
            Device.BeginInvokeOnMainThread(() => { statusLabel.Text += "\n ret:" + ret; });
        }

        private async void OnRoleClicked(object sender, EventArgs e)
        {
            var answer = await api.RESTCalls.L2PViewUserRoleAsync("14ws-44150");

            Device.BeginInvokeOnMainThread(() => { statusLabel.Text += "\n Role:" + answer.role; });
        }

        private async void OnOneCourseClicked(object sender, EventArgs e)
        {
            var answer = await api.RESTCalls.L2PViewCourseInfoAsync("14ws-44150");
            
            Device.BeginInvokeOnMainThread(() => { statusLabel.Text += "\n "+answer.courseTitle; });
        }

        private async void OnCoursesClicked(object sender, EventArgs e)
        {
            var answer = await api.RESTCalls.L2PViewAllCourseInfoAsync();
            
            Device.BeginInvokeOnMainThread(() => { statusLabel.Text += "\n "+answer.dataset.Count; });
        }

        private async void OnPingButClicked(object sender, EventArgs e)
        {
            var answer = await api.RESTCalls.L2PPingCallAsync("Test?");
            //api.RESTCalls.L2PViewCourseInfo("15ss-51408");
            Device.BeginInvokeOnMainThread(() => { statusLabel.Text += "\n " + answer; });
        }

        private async void OnCheckButtonClicked(object sender, EventArgs e)
        {
            var answer = await api.AuthenticationManager.CheckAuthenticationProgressAsync();
            
            Device.BeginInvokeOnMainThread(() => { statusLabel.Text += "\n "+answer; });
        }

        async void OnStartButtonClicked(Object sender, EventArgs e)
        {
            //var rest = new api.RESTCalls();
            //var answer = api.RESTCalls.OAuthInitCall();
            var answer = await api.AuthenticationManager.StartAuthenticationProcessAsync();

            //Console.WriteLine(answer);
            
            if (answer == "")
            {
                Device.BeginInvokeOnMainThread(() => { statusLabel.Text = "Failed!"; });
                return;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                statusLabel.Text = "open: "+answer;
            });
            var uri = new Uri(answer);
            Device.OpenUri(uri);

        }
	}
}
