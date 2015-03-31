using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Qurvey.pages
{
	public partial class ConfigPage : ContentPage
	{
        /// <summary>
        /// A static refernce for the Label representing the progress of the OAuth Connection
        /// Is visible from outside, but only the UI-Thread is allowed to change text!
        /// </summary>
        public static Label UpdateLabel;


        public ConfigPage()
        {
            //InitializeComponent();

            // Not sure - Show a dialog box for credentials, or put it right on the page?


            // The label for the User to tell, what to do
            Label DescLabel = new Label
            {
                XAlign = TextAlignment.Center,
                FontAttributes = FontAttributes.Italic,
                Text = "Press the Button to Authorize the App \n "
            };

            // The Label for the Updates
            UpdateLabel = new Label
            {
                XAlign = TextAlignment.Center,
                FontAttributes = FontAttributes.Italic,
                Text = " " //No Text on start
            };

            // The Button, triggering an Authorization Process
            Button UpdateButton = new Button
            {
                Text = "Authorize (Will open a web page of RWTH)"
            };

            // Connect the method for the Authorization with the Button
            UpdateButton.Clicked += OnButtonClicked;

            // Create embedding Scrollview to allow scrolling on this page (in advance - maybe later on the page will grow...)
            ScrollView v = new ScrollView();
            v.Orientation = ScrollOrientation.Vertical;


            //Content = new StackLayout
            StackLayout stack = new StackLayout
            {
                Padding = new Thickness(0.3, 0.1, 0.2, 0),
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Center,
                Children = { DescLabel, UpdateButton, UpdateLabel }
            };

            v.Content = stack;
            Content = v;


            Title = "Authorization";
        }

        /// <summary>
        /// Triggers an authorizationa.
        /// The log of this will be written into the UpdateLabel
        /// </summary>
        void OnButtonClicked(object sender, EventArgs e)
        {
            // Tell user what will happen
            UpdateLabel.Text = "\n Starting Authorization...";



            UpdateLabel.Text += "\n finished Authorization";


        }
	}
}
