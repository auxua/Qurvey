using System;
using System.Collections.Generic;
using Qurvey.api;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Qurvey.pages
{
	public partial class BackendPage : ContentPage
	{
		public BackendPage ()
		{
			InitializeComponent ();
			getData ();
		}

		private async void getData() {
			Task<string> data = RESTCalls.RestCallAsync<string>(null, "http://qurvey.raederscheidt.de/SurveyService.svc/Data/10", false);
			dataLabel.Text = await data;	
		}
	}
}

