using System;
using Qurvey.Shared.Models;
using System.ComponentModel;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Qurvey.ViewModels
{
	public class SurveyCellViewModel : INotifyPropertyChanged
	{
		private Survey survey;

		public Survey Survey { 
			get {
				return survey;
			} 
		}

		public string Question {
			get { return survey.Question; }
		}

		private Color backgroundColor;

		public Color BackgroundColor {
			get { 
				return backgroundColor;
				//return Color.Aqua;
			}
			set {
				backgroundColor = value;
				RaisePropertyChanged ("BackgroundColor");
			}
		}

		public SurveyCellViewModel (Survey survey, Color backgroundColor)
		{
			this.survey = survey;
			RaisePropertyChanged ("Survey");
			BackgroundColor = backgroundColor;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged (string propName)
		{ 
			if (PropertyChanged != null) {
				PropertyChanged (this, new PropertyChangedEventArgs (propName));
			}
		}

		public static List<SurveyCellViewModel> CreateViewModels(List<Survey> surveys, Color backgroundColor) {
			List<SurveyCellViewModel> l = new List<SurveyCellViewModel> ();
			foreach (Survey s in surveys) {
				l.Add (new SurveyCellViewModel (s, backgroundColor));
			}
			return l;
		}
	}
}

