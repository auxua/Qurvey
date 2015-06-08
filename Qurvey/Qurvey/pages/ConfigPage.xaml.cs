using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qurvey.api.DataModel;

using Xamarin.Forms;

namespace Qurvey.pages
{
	public partial class ConfigPage : ContentPage
	{

        public ConfigPage()
        {
            //InitializeComponent();

            SwitchCell adminSwitch = new SwitchCell
            {
                Text = "I am Manager",
                On = App.isAdmin(),
            };
            adminSwitch.OnChanged += (o,s) => { App.setAdmin(adminSwitch.On); };


            SwitchCell courseSwitch = new SwitchCell
            {
                Text = "Get all courses",
                On = App.isUsingAllCourses(),
            };
            courseSwitch.OnChanged += async (o, s) => { 
                App.setUsingAllCourses(courseSwitch.On);
                RefreshNavbar(courseSwitch.On);
            };


            Content = new TableView
            {
                Intent = TableIntent.Settings,
                Root = new TableRoot {
                    new TableSection("Manager or Student?") {
                        adminSwitch
                    },
                    new TableSection("All Courses/Semester Courses") {
                        courseSwitch
                    }
                }
            };

			// In case of iOS the padding is done automatically by the table representation
			if (Device.OS != TargetPlatform.iOS)
            	Padding = new Thickness(15, 10, 15, 10);
            Title = "Settings";
        }

        private async Task RefreshNavbar(bool useAll)
        {
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
