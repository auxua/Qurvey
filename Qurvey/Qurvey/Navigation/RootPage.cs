using System;
using Xamarin.Forms;

namespace Qurvey.Navigation
{
	public class RootPage : MasterDetailPage
	{
		/// <summary>
		/// Create the Root Page
		/// Define master and Detail as wenn as the default Detail page
		/// </summary>
		public RootPage ()
		{
			var menuPage = new MenuPage ();
			// The effective navigation gets assigned
			menuPage.Menu.ItemSelected += (sender, e) => {
				// Before Navigating, recolor the cell
				if (menuPage.Menu.selected != null) {
					menuPage.Menu.selected.SetColors (true);
				}

				// Select new
				menuPage.Menu.selected = (menuPage.Menu.SelectedItem as NavMenuItem);
				menuPage.Menu.selected.SetColors (false);

				NavigateTo (e.SelectedItem as NavMenuItem);
			};

			Master = menuPage;
			// Set default Detail page
			Detail = new NavigationPage (new pages.WelcomePage ());
		}


		/// <summary>
		/// Depending on the selected item, go to the corresponding page
		/// </summary>
		/// <param name="menu">the menuitem, that was selected</param>
		void NavigateTo (NavMenuItem menu)
		{
			try {
				// If there is a Navigation inside the Page, Pop back to allow a working Navigation Stack
				if (Detail.Navigation != null)
					Detail.Navigation.PopToRootAsync ();

				//if (Detail is pages.CourseRoomPage)
				if (Detail is NavigationPage) {
					var page = ((NavigationPage)Detail).CurrentPage;
					if (page is pages.CourseRoomPage) {
						var bc = page.BindingContext as ViewModels.CourseRoomPageViewModel;
						// Stop worker Threads if existing (Destructors are not called in guarantee...)
						bc.CancelWork ();
					}
				}

				if (menu.TargetType == typeof(pages.CourseRoomPage)) {
					Detail = new NavigationPage (new pages.CourseRoomPage (menu.CID, menu.Title));
					IsPresented = false;
					return;
				}
				Page displayPage = (Page)Activator.CreateInstance (menu.TargetType);
				Detail = new NavigationPage (displayPage);

				IsPresented = false;
			} catch (Exception e) {
				Device.BeginInvokeOnMainThread (() => {
					DisplayAlert ("Error", "Could not load Page: " + e.Message, "Damn!");
				});
			}

		}
	}
}