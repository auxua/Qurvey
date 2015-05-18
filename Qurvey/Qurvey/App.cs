using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Qurvey
{
	public class App : Application
	{
		public App ()
		{
			// The root page of your application
            MainPage = new RootPage();

            api.AuthenticationManager.CheckStateAsync();

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
	}
}
