using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Qurvey.Navigation;

namespace Qurvey
{
	public class App : Application
	{
		public App ()
		{
			// The root page of your application
            MainPage = new RootPage();

            Task.Run(() => api.AuthenticationManager.CheckStateAsync()).Wait();
            

            //var re = new api.RESTCalls();
            //var ans = re.RestCall<api.L2PPingData>("", api.Config.L2PEndPoint + "Ping?accessToken=foo&p=pp",false);
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
            // App resumes
		}

        #region App-based Configuration

        /// <summary>
        /// gets the indicator, whether user decided to be admin (== l2p-amanger)
        /// </summary>
        public static bool isAdmin()
        {
            if (!Application.Current.Properties.ContainsKey("isAdmin"))
            {
                return false;
            }
            else
            {
                bool admin = (bool)Application.Current.Properties["isAdmin"];
                return admin;
            }
        }

        /// <summary>
        /// Sets the indicator, whether the app user is admin
        /// </summary>
        /// <param name="admin"></param>
        public static void setAdmin(bool admin)
        {
            Application.Current.Properties["isAdmin"] = admin;
            Application.Current.SavePropertiesAsync();
        }

        /// <summary>
        /// get the Indicator, whether the user wants to see every course room (or only current ones)
        /// </summary>
        public static bool isUsingAllCourses()
        {
            if (!Application.Current.Properties.ContainsKey("isUsingAllCourses"))
            {
                return false;
            }
            else
            {
                bool admin = (bool)Application.Current.Properties["isUsingAllCourses"];
                return admin;
            }
        }

        /// <summary>
        /// Sets the indicator, whether the user wants to see every course room
        /// </summary>
        public static void setUsingAllCourses(bool use)
        {
            Application.Current.Properties["isUsingAllCourses"] = use;
            Application.Current.SavePropertiesAsync();
        }

        #endregion
    }
}
