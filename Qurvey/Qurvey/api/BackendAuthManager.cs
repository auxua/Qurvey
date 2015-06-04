using System;
using Qurvey.Shared.Models;
using Xamarin.Forms;

namespace Qurvey.api
{
	public class BackendAuthManager
	{
		protected static BackendAuthManager instance;

		public static BackendAuthManager Instance {
			get {
				if (instance == null) {
					instance = new BackendAuthManager ();
				}
				return instance;
			}
		}

		protected BackendAuthManager() {
			User user = (User) Application.Current.Properties["User"];
			if (user == null) {
				IsAuthenticated = false;
			} else {
				IsAuthenticated = true;
				User = user;
			}
		}

		public bool IsAuthenticated { get; protected set; }

		public User User { get; protected set; }

		public void AuthenticateWithBackend() {
			if (IsAuthenticated) {
				throw new Exception ("User is already authenticated with backend");
			}

			User user = Backend.CreateNewUserAsync().Result;
			Application.Current.Properties ["User"] = user;
			Application.Current.SavePropertiesAsync();
			User = user;
			IsAuthenticated = true;
		}
	}
}

