using System;
using Qurvey.Shared.Models;
using System.ComponentModel;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Qurvey.ViewModels
{
	public class AnswerViewModel : INotifyPropertyChanged
	{
		public Answer Answer { get; set; }

		public string AnswerText {
			get {
				return Answer.AnswerText;
			}
		}

		private Color backgroundColor;

		public Color BackgroundColor {
			get { 
				return backgroundColor;
			}
			set {
				backgroundColor = value;
				RaisePropertyChanged ("BackgroundColor");
			}
		}

		public AnswerViewModel (Answer answer, Color backgroundColor)
		{
			Answer = answer;
			RaisePropertyChanged ("AnswerText");
			BackgroundColor = backgroundColor;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged (string propName)
		{ 
			if (PropertyChanged != null) {
				PropertyChanged (this, new PropertyChangedEventArgs (propName));
			}
		}

		public static List<AnswerViewModel> CreateViewModels(List<AnswerViewModel> answers, Color backgroundColor) {
			List<AnswerViewModel> l = new List<AnswerViewModel> ();
			foreach (Answer a in answers) {
				l.Add (new AnswerViewModel (a, backgroundColor));
			}
			return l;
		}
	}
}