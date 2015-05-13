using System.Data.Entity;

namespace Qurvey.Backend
{
    public class SurveyContext : DbContext
    {
        public SurveyContext() : base("QurveyContext") { }
        
        public DbSet<Survey> Surveys { get; set; }

        public DbSet<Vote> Votes { get; set; }
    }
}
