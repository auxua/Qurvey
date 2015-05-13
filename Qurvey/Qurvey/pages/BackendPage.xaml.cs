using System;
using System.Collections.Generic;
using Qurvey.api;
using Xamarin.Forms;
using System.Threading.Tasks;
using Qurvey.Shared.Models;

namespace Qurvey.pages
{
	public partial class BackendPage : ContentPage
	{
		public BackendPage ()
		{
			InitializeComponent ();
			var s = new Survey ("Frage");
			//getData ();
			dataLabel.Text = s.Question;
		}

		private async void getData() {
			Task<string> data = RESTCalls.RestCallAsync<string>(null, "http://qurvey.raederscheidt.de/SurveyService.svc/Data/10", false);
			dataLabel.Text = await data;	
		}
	}
}

