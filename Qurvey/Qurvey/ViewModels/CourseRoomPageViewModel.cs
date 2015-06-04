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

namespace Qurvey.ViewModels
{
    class CourseRoomPageViewModel : INotifyPropertyChanged
    {
        #region properties

        private bool isAdmin;

        public bool IsAdmin
        {
            get
            {
                return this.isAdmin;
            }
            set
            {
                this.isAdmin = value;
                if (value)
                {
                    this.buttonText = "Create New Survey";
                }
                else
                {
                    this.ButtonText = "Panic!";
                }
                RaisePropertyChanged("IsAdmin");
            }
        }

        private string cid;

        public string CID
        {
            get
            {
                return this.cid;
            }
            set
            {
                this.cid = value;
                RaisePropertyChanged("CID");
            }
        }

        private string status;

        public string Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
                RaisePropertyChanged("Status");
            }
        }

        private string title;

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
                RaisePropertyChanged("Title");
            }
        }

        private List<Survey> surveys;

        public List<Survey> Surveys
        {
            get
            {
                return this.surveys;
            }
            set
            {
                this.surveys = value;
                RaisePropertyChanged("Surveys");
            }
        }

        private bool noSurveys;

        public bool NoSurveys
        {
            get
            {
                return this.noSurveys;
            }
            set
            {
                this.noSurveys = value;
                RaisePropertyChanged("NoSurveys");
            }
        }

        private string buttonText;

        public string ButtonText
        {
            get
            {
                return this.buttonText;
            }
            set
            {
                this.buttonText = value;
                RaisePropertyChanged("ButtonText");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        #endregion

        #region Commands

        private ICommand refreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                return refreshCommand;
            }
        }

        private async Task RefreshSurveys()
        {
            Survey[] sur = await api.Backend.GetSurveysAsync(cid);
            var lSurveys = new List<Survey>();
            
            // No Surveys available?
            if (sur == null)
            {
                return;
            }
            // If there are Surveys, add them!
            foreach (Survey survey in sur)
            {
                lSurveys.Add(survey);
            }
            NoSurveys = lSurveys.Count == 0;
            Surveys = lSurveys;
            Status = "Refreshed Surveys";
            //SurveyList.IsVisible = Surveys.Count > 0;
        }

        private ICommand panicCommand;

        public ICommand PanicCommand
        {
            get
            {
                return panicCommand;
            }
        }

        protected async Task PanicExecute()
        {
            try
            {
                Panic panic = new Panic(cid, BackendAuthManager.Instance.User);
                await api.Backend.SavePanicAsync(panic);
                Status = "Sent Panic";
            }
            catch (Exception ex)
            {
                Status = "Failed: " + ex.Message;                
            }
        }

        #endregion

        public CourseRoomPageViewModel(string course, string title)
        {
            if ((course != null) && (course != ""))
            {
                CID = course;
            }
            else
            {
                CID = "Unknown CID";
            }
            //this.CID = course;
            this.Title = title;

            this.IsAdmin = App.isAdmin();

            this.NoSurveys = true;

            // Create Commands
            this.refreshCommand = new Command(async () => await RefreshSurveys());
            this.panicCommand = new Command(async () => await PanicExecute());

        }

        

        
    }
}
