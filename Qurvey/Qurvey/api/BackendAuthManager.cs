using System;
using Qurvey.Shared.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
			if (!Application.Current.Properties.ContainsKey("UserAsString")) {
				IsAuthenticated = false;
			} else {
				User user = JsonConvert.DeserializeObject<User>(Application.Current.Properties["UserAsString"] as string);
				IsAuthenticated = true;
				User = user;
			}
		}

		public bool IsAuthenticated { get; protected set; }

		public User User { get; protected set; }

		public async Task AuthenticateWithBackend() {
			if (IsAuthenticated) {
				return;
				//throw new Exception ("User is already authenticated with backend");
			}

			User user = await Backend.CreateNewUserAsync();
			Application.Current.Properties ["UserAsString"] = JsonConvert.SerializeObject(user);
			Application.Current.SavePropertiesAsync();
			User = user;
			IsAuthenticated = true;
		}
	}
}

