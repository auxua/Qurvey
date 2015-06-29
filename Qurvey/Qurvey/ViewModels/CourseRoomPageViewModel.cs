using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Qurvey.api;
using Qurvey.pages;
using Qurvey.Shared.Models;
using Xamarin.Forms;
using System.Linq;

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

		private bool isCreated;

		public bool IsCreated {
			get {
				return this.isCreated;
			}
			set {
				this.isCreated = value;
				// Now, do some work
				if (value) {
					Thread t = new Thread (new ThreadStart (InitVM));
					t.Start ();
				}
				RaisePropertyChanged ("IsCreated");
			}
		}

		private List<SurveyCellViewModel> surveys;

		public List<SurveyCellViewModel> Surveys {
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

		private bool isRefreshing = false;

		public bool IsRefreshing {
			get {
				return this.isRefreshing;
			}
			set {
				this.isRefreshing = value;
				RaisePropertyChanged ("IsRefreshing");
			}
		}

		public Color CellBackgroundColor { get; set; }

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
			List<Survey> sur = await api.Backend.GetSurveysAsync (cid);
			Surveys = SurveyCellViewModel.CreateViewModels(sur, CellBackgroundColor);
			IsRefreshing = false;
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

		public async Task DeleteSurveyExecute (Survey s)
		{
			await Backend.DeleteSurveyAsync (s);
			await LoadSurveys ();
		}

		protected async Task CreateSurveyExecute ()
		{
			NewSurveyPage surveyPage = new NewSurveyPage (CID);
			await this.navigation.PushAsync (surveyPage);
		}

		#endregion

		public CourseRoomPageViewModel (string course, string title, INavigation navigation, Color cellBackgroundColor)
		{
			if ((course != null) && (course != "")) {
				CID = course;
			} else {
				CID = "Unknown CID"; // TODO throw exception?
			}
			this.Title = title;
			this.navigation = navigation;
			CellBackgroundColor = cellBackgroundColor;
			this.IsAdmin = App.isAdmin ();

			// Create Commands
			this.refreshCommand = new Command (async () => await LoadSurveys ());
			this.panicCommand = new Command (async () => await PanicExecute ());
			this.createSurveyCommand = new Command (async () => await CreateSurveyExecute ());

			this.IsCreated = true;
		}

		private async void InitVM ()
		{
			IsBusy = true;
			await LoadSurveys ();
			// Start the panicThread if being admin
			if (this.IsAdmin) {
				try {
					this.panicThread = new Thread (new ThreadStart (panicWork));
					panicThread.Start ();
				} catch (Exception ex) {
					// should be mainly ThreadState fails that may occur - just ignore it
				}
			} else { // else start the refreshThread
				try {
					this.refreshThread = new Thread (new ThreadStart (refreshWork));
					refreshThread.Start ();
				} catch (Exception ex) {
					// should be mainly ThreadState fails that may occur - just ignore it
				}
			}
			IsBusy = false;
		}

		~CourseRoomPageViewModel ()
		{
			this.CancelWork ();
		}

		public void CancelWork ()
		{
			// Check the threads and end it if needed
			try {
				if (this.panicThread != null)
					this.panicThread.Abort ();

				if (this.refreshThread != null)
					this.refreshThread.Abort ();
			} catch {
				// nothing // TODO do something about it
			}
		}

		private Thread panicThread;
		private Thread refreshThread;

		protected void panicWork ()
		{
			try {
				while (true) {
					// Wait 30 sec.
					Thread.Sleep (30 * 1000);
					getLastPanicsAsync ();
				}
			} catch (Exception ex) {
				// just die... TODO do something about it
			}
		}

		protected void refreshWork () // TODO test this
		{
			try {
				while (true) {
					// Wait 30 sec.
					Thread.Sleep (60 * 1000);
					List<Survey> sur = api.Backend.GetSurveysAsync (cid).Result;
					if(sur.Count != Surveys.Count) {
						Surveys = SurveyCellViewModel.CreateViewModels(sur, CellBackgroundColor);
					}
				}
			} catch (Exception ex) {
				// just die... TODO do something about it
			}
		}

		protected async void getLastPanicsAsync ()
		{
			int result = await Backend.CountPanicsAsync (CID, DateTime.Now.AddMinutes (-1));
			//int result = await Backend.CountLastPanicsAsync(CID, 60);
			if (result > 1) {
				Device.BeginInvokeOnMainThread (() => App.Current.MainPage.DisplayAlert ("PANIC!", "Multiple people pressed the Panic button in the last 60 seconds!", "OK"));
			}
		}
	}
}