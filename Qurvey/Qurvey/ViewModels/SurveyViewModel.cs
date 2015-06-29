using System;
using Qurvey.Shared.Models;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Qurvey.api;

namespace Qurvey.ViewModels
{
	public class SurveyViewModel : INotifyPropertyChanged
	{

		private bool isBusy;

		public bool IsBusy {
			get {
				return this.isBusy;
			}
			set {
				this.isBusy = value;
				RaisePropertyChanged ("IsBusy");
			}
		}

		private Survey survey;

		public Survey Survey {
			get { return survey; }
			set {
				survey = value;
				surveyAnswers = AnswerViewModel.CreateViewModels (survey.Answers, CellBackgroundColor);
				RaisePropertyChanged ("SurveyQuestion");
				RaisePropertyChanged ("SurveyCreatedModified");
				RaisePropertyChanged ("SurveyAnswers");
			}
		}

		public string SurveyQuestion {
			get { 
				if (Survey == null)
					return "";

				return Survey.Question;
			}
		}

		public string SurveyCreated {
			get {
				if (Survey == null)
					return "";

				return string.Format ("Survey was created at {0}", Survey.Created);
			}
		}

		public string SurveyModified {
			get {
				if (Survey == null)
					return "";

				return string.Format ("Survey was last modified at {0}", Survey.Modified);
			}
		}

		private List<AnswerViewModel> surveyAnswers;

		public ICollection<AnswerViewModel> SurveyAnswers {
			get {
				if (Survey == null)
					return new List<AnswerViewModel> ();

				return surveyAnswers;
			}
		}

		private AnswerViewModel usersAnswer;

		public AnswerViewModel UsersAnswer {
			get { return usersAnswer; }
			set {
				if ((usersAnswer == null && value != null) || (usersAnswer != null && !usersAnswer.Equals (value))) { // if value really is different 
					if (usersAnswer != null) {
						usersAnswer.BackgroundColor = CellBackgroundColor;
						usersAnswer.Selected = false;
					}

					usersAnswer = value;
					if (usersVote == null || !usersVote.Answer.Equals (value.Answer)) { // we have a new vote. Save it!
						SaveNewVote ();
					}
					RaisePropertyChanged ("UsersAnswer");
					usersAnswer.BackgroundColor = HighlightBackgroundColor;
					usersAnswer.Selected = true;
				}
			}
		}

		private Vote usersVote;

		public Color CellBackgroundColor { get; set; }

		public Color HighlightBackgroundColor { get { return Color.FromHex ("#FF4F45"); }}

		protected async void LoadUsersVote ()
		{
			usersVote = await Backend.GetVoteForUserAsync (Survey, BackendAuthManager.Instance.User);
			if (usersVote != null)
				UsersAnswer = new AnswerViewModel (usersVote.Answer, HighlightBackgroundColor);

			IsBusy = false;
		}

		protected async void SaveNewVote ()
		{
			if (usersVote != null) {
				// Delete old vote if it exists
				await api.Backend.DeleteVoteAsync (usersVote);
			}
			Vote newVote = new Vote (BackendAuthManager.Instance.User, Survey, UsersAnswer.Answer);
			await api.Backend.SaveVoteAsync (newVote);
			usersVote = await Backend.GetVoteForUserAsync (Survey, BackendAuthManager.Instance.User);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged (string propName)
		{ 
			if (PropertyChanged != null) {
				PropertyChanged (this, new PropertyChangedEventArgs (propName));
			}
		}

		public SurveyViewModel (Survey survey, Color cellBackgroundColor)
		{
			CellBackgroundColor = cellBackgroundColor;
			Survey = survey;
			IsBusy = true;
			LoadUsersVote ();
		}
	}
}