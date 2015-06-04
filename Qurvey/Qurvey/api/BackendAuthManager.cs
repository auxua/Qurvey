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
			if (!Application.Current.Properties.ContainsKey("User")) {
				IsAuthenticated = false;
			} else {
				User user = (User) Application.Current.Properties["User"];
				IsAuthenticated = true;
				User = user;
			}
		}

		public bool IsAuthenticated { get; protected set; }

		public User User { get; protected set; }

		public async void AuthenticateWithBackend() {
			if (IsAuthenticated) {
				return;
				//throw new Exception ("User is already authenticated with backend");
			}

			User user = await Backend.CreateNewUserAsync();
			Application.Current.Properties ["User"] = user;
			Application.Current.SavePropertiesAsync();
			User = user;
			IsAuthenticated = true;
		}
	}
}

