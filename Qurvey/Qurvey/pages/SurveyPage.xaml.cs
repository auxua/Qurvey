using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Qurvey.pages
{
	public partial class SurveyPage : ContentPage
	{
		public string[] SurveyQuestions { get; set; }

		public SurveyPage ()
		{
			SurveyQuestions = new string[]{ "Option 1", "Option 2", "This is option 3", "Another option"};
			InitializeComponent ();
			listView.ItemsSource = SurveyQuestions;
		}
	}
}

