using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Input;
using Qurvey.api;
using Qurvey.api.DataModel;
using Xamarin.Forms;
using Qurvey.Shared.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Qurvey.pages;
using Qurvey.Commands;

namespace Qurvey.ViewModels
{
	class CourseRoomPageViewModel : INotifyPropertyChanged
	{
		#region properties

		private INavigation navigation;

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

		private bool isAdmin;

		public bool IsAdmin {
			get {
				return this.isAdmin;
			}
			set {
				this.isAdmin = value;
				RaisePropertyChanged ("IsAdmin");
			}
		}

		private string cid;

		public string CID {
			get {
				return this.cid;
			}
			set {
				this.cid = value;
				RaisePropertyChanged ("CID");
			}
		}

		private string status;

		public string Status {
			get {
				return this.status;
			}
			set {
				this.status = value;
				RaisePropertyChanged ("Status");
			}
		}

		private string title;

		public string Title {
			get {
				return this.title;
			}
			set {
				this.title = value;
				RaisePropertyChanged ("Title");
			}
		}

		private List<Survey> surveys;

		public List<Survey> Surveys {
			get {
				return this.surveys;
			}
			set {
				this.surveys = value;
				RaisePropertyChanged ("Surveys");
				RaisePropertyChanged ("NoSurveys");
			}
		}

		public bool NoSurveys {
			get {
				if (this.Surveys == null)
					return true;

				return this.Surveys.Count == 0;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged (string propName)
		{
			if (PropertyChanged != null) {
				PropertyChanged (this, new PropertyChangedEventArgs (propName));
			}
		}

		#endregion

		#region Commands

		private ICommand refreshCommand;

		public ICommand RefreshCommand {
			get {
				return refreshCommand;
			}
		}

		private ICommand panicCommand;

		public ICommand PanicCommand {
			get {
				return panicCommand;
			}
		}

		private ICommand createSurveyCommand;

		public ICommand CreateSurveyCommand {
			get {
				return createSurveyCommand;
			}
		}

		private async Task LoadSurveys ()
		{
			IsBusy = true;
			Survey[] sur = await api.Backend.GetSurveysAsync (cid);
			var lSurveys = new List<Survey> ();
            
			// No Surveys available?
			if (sur == null) {
				return; // TODO throw error?
			}
			// If there are Surveys, add them!
			foreach (Survey survey in sur) {
				lSurveys.Add (survey);
			}
			Surveys = lSurveys;
			Status = "Refreshed Surveys";
			IsBusy = false;
		}

		protected async Task PanicExecute ()
		{
			try {
				IsBusy = true;
				Panic panic = new Panic (cid, BackendAuthManager.Instance.User);
				await api.Backend.SavePanicAsync (panic);
				Status = "Sent Panic";
				IsBusy = false;
			} catch (Exception ex) {
				Status = "Failed: " + ex.Message;                
			}
		}

		public async Task DeleteSurveyExecute (Survey s) {
			await Backend.DeleteSurveyAsync (s);
			await LoadSurveys ();
		}

		protected async Task CreateSurveyExecute ()
		{
			NewSurveyPage surveyPage = new NewSurveyPage (CID);
			await this.navigation.PushAsync (surveyPage);
		}

		#endregion

		public CourseRoomPageViewModel (string course, string title, INavigation navigation)
		{
			if ((course != null) && (course != "")) {
				CID = course;
			} else {
				CID = "Unknown CID"; // TODO throw exception?
			}
			this.Title = title;
			this.navigation = navigation;
			this.IsAdmin = App.isAdmin ();

			LoadSurveys ();

			// Create Commands
			this.refreshCommand = new Command (async () => await LoadSurveys ());
			this.panicCommand = new Command (async () => await PanicExecute ());
			this.createSurveyCommand = new Command (async () => await CreateSurveyExecute ());
		}
	}
}
