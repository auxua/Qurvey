using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Qurvey.pages
{
	public partial class CoursePage : ContentPage
	{
		public CoursePage ()
		{
			InitializeComponent ();
			surveysListView.ItemsSource = new string[]{ "What does Bayes theorem state?", "How is the speed?" };
		}
	}
}

