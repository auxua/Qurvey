using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Input;
using Qurvey.api;
using Qurvey.api.DataModel;
using Xamarin.Forms;

namespace Qurvey.ViewModels
{
	public class WelcomeViewModel : INotifyPropertyChanged
	{
		private bool pleaseAuth;

		public bool PleaseAuth {
			get {
				return this.pleaseAuth;
			}
			set {
				this.pleaseAuth = value;
				RaisePropertyChanged ("PleaseAuth");
			}
		}

		private bool authorizing;

		public bool Authorizing {
			get {
				return this.authorizing;
			}
			set {
				this.authorizing = value;
				RaisePropertyChanged ("Authorizing");
			}
		}

		private ICommand authorizeCommand;

		public ICommand AuthorizeCommand {
			get {
				return authorizeCommand;
			}
		}

		private bool loadingCourses;

		public bool LoadingCourses {
			get {
				return this.loadingCourses;
			}
			set {
				this.loadingCourses = value;
				RaisePropertyChanged ("LoadingCourses");
			}
		}

		private bool ready;

		public bool Ready {
			get {
				return this.ready;
			}
			set {
				this.ready = value;
				RaisePropertyChanged ("Ready");
			}
		}

		private string errorMessage;

		public string ErrorMessage {
			get {
				return this.errorMessage;
			}
			set {
				if (string.IsNullOrEmpty (value)) {
					this.errorMessage = "";
				} else {
					this.errorMessage = string.Format ("An error occured: {0}", value);
				}
				RaisePropertyChanged ("ErrorMessage");
				RaisePropertyChanged ("HasErrorMessage");
			}
		}

		public bool HasErrorMessage {
			get {
				return !string.IsNullOrEmpty (ErrorMessage);
			}
		}

		public WelcomeViewModel ()
		{
			LoadingCourses = false;
			authorizeCommand = new Command ((nothing) => StartAuthenticationProcess ());
			CheckAuthorizationStatus ();
		}

		protected void CheckAuthorizationStatus ()
		{
			api.AuthenticationManager.CheckStateAsync ();
			if (api.AuthenticationManager.getState () != api.AuthenticationManager.AuthenticationState.ACTIVE) {
				PleaseAuth = true;
				Ready = false;
			} else {
				PleaseAuth = false;
				if (!MenuPage.HasCourses) {
					LoadCourses ();
				} else {
					Ready = true;
				}
			}
		}

		protected async void StartAuthenticationProcess ()
		{
			Authorizing = true;
			ErrorMessage = "";

			string url = await api.AuthenticationManager.StartAuthenticationProcessAsync();
			if (string.IsNullOrEmpty(url))
			{
				ErrorMessage = "URL was empty!";
				return;
			}
			Device.OpenUri(new Uri(url));

			// just try to check authorize sometimes in case of slow device
			for(int i = 0; i < 50; i++) {
				Thread.Sleep(1000);
				bool auth = await api.AuthenticationManager.CheckAuthenticationProgressAsync();
				if (auth)
				{
					// Authentication with L2P was successful
					// Now authenticate with Backend
					this.AuthenticateWithBackend();
					return;
				}
			}
			Authorizing = false;
			ErrorMessage = "Authentification was not completed. Please try again.";
		}

		protected async void AuthenticateWithBackend() {
			await BackendAuthManager.Instance.AuthenticateWithBackend ();
			Console.WriteLine ("User token: {0}", BackendAuthManager.Instance.User.Code);
			Authorizing = false;
			PleaseAuth = false;
			// Now load courses
			this.LoadCourses();
		}

		protected async void LoadCourses() {
			LoadingCourses = true;
			bool useAll = App.isUsingAllCourses();
			//IsBusy = true;

			/*if (api.AuthenticationManager.getState() != api.AuthenticationManager.AuthenticationState.ACTIVE)
			{
				Device.BeginInvokeOnMainThread(() => DisplayAlert("Authorization needed", "Please authorize the App first", "OK"));
				IsBusy = false;
				return;
			}*/

			// Get the courses!
			L2PCourseInfoSetData courses;
			if (useAll)
			{
				courses = await api.RESTCalls.L2PViewAllCourseInfoAsync();
			}
			else
			{
				string semester;
				if (DateTime.Now.Month < 4)
				{
					// winter semester but already in new year -> ws+last year
					semester = "ws" + DateTime.Now.AddYears(-1).Year.ToString().Substring(2);
				}
				else if (DateTime.Now.Month > 9)
				{
					// winter semester, still in this year
					semester = "ws" + DateTime.Now.Year.ToString().Substring(2);
				}
				else
				{
					// summer semester
					semester = "ss" + DateTime.Now.Year.ToString().Substring(2);
				}
				courses = await api.RESTCalls.L2PViewAllCourseIfoBySemesterAsync(semester);
			}

			// Workaround (should be moved to Manager)
			if (courses == null)
			{
				await api.AuthenticationManager.GenerateAccessTokenFromRefreshTokenAsync();
				courses = await api.RESTCalls.L2PViewAllCourseInfoAsync();
			}

			if (courses.dataset == null)
			{
				ErrorMessage = courses.errorDescription;
				return;
			}

			MenuPage.ClearMenu();
			foreach (L2PCourseInfoData l2pcourse in courses.dataset)
			{
				MenuPage.AddCourseToMenu(l2pcourse.uniqueid, l2pcourse.courseTitle);
			}
			LoadingCourses = false;
			Ready = true;
			//IsBusy = false;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged (string propName)
		{
			if (PropertyChanged != null) {
				PropertyChanged (this, new PropertyChangedEventArgs (propName));
			}
		}
	}
}

