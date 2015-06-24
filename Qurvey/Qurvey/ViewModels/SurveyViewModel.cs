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

		public bool IsBusy
		{
			get
			{
				return this.isBusy;
			}
			set
			{
				this.isBusy = value;
				RaisePropertyChanged("IsBusy");
			}
		}

		private Survey survey;

		public Survey Survey {
			get { return survey; }
			set {
				survey = value;
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

		// Necessary because Xamarin doesnt support multi binding
		public string SurveyCreatedModified {
			get {
				if (Survey == null)
					return "";

				return string.Format ("Created at {0} Last modified at {1}", Survey.Created, Survey.Modified);
			}
		}

		public ICollection<Answer> SurveyAnswers {
			get {
				if (Survey == null)
					return new List<Answer> ();

				return Survey.Answers;
			}
		}

		private Answer usersAnswer;

		public Answer UsersAnswer {
			get {return usersAnswer;}
			set {
				if ((usersAnswer == null && value != null) || (usersAnswer != null && !usersAnswer.Equals (value))) { // if value really is different 
					usersAnswer = value;
					if (usersVote == null || !usersVote.Answer.Equals (value)) { // we have a new vote. Save it!
						SaveNewVote();
					}
					RaisePropertyChanged ("UsersAnswer");
				}
			}
		}

		private Vote usersVote;

		protected async void LoadUsersVote () {
			usersVote = await Backend.GetVoteForUserAsync (Survey, BackendAuthManager.Instance.User);
			if(usersVote != null)
				UsersAnswer = usersVote.Answer;

			IsBusy = false;
		}

		protected async void SaveNewVote() {
			if (usersVote != null) {
				// Delete old vote if it exists
				await api.Backend.DeleteVoteAsync (usersVote);
			}
			Vote newVote = new Vote(BackendAuthManager.Instance.User, Survey, UsersAnswer);
			await api.Backend.SaveVoteAsync(newVote);
			usersVote = await Backend.GetVoteForUserAsync (Survey, BackendAuthManager.Instance.User);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged (string propName)
		{ 
			if (PropertyChanged != null) {
				PropertyChanged (this, new PropertyChangedEventArgs (propName));
			}
		}

		public SurveyViewModel (Survey survey)
		{
			Survey = survey;
			IsBusy = true;
			LoadUsersVote ();
		}
	}
}