using Qurvey.Shared.Models;
using System.Data.Entity;

namespace Qurvey.Backend.Test
{
    public class TestSurveyContext : SurveyContext
    {
        public new DbSet<Survey> Surveys
        {
            get { return base.Surveys; }
            set { base.Surveys = value; }
        }

        public new DbSet<Vote> Votes {
            get { return base.Votes; }
            set { base.Votes = value; }
        }
    }
}
