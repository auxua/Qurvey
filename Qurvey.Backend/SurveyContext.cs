using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Qurvey.Backend
{
    public class SurveyContext : DbContext
    {
        public SurveyContext() : base("QurveyContext") { }
        
        public DbSet<Survey> Surveys { get; set; }

        public DbSet<Vote> Votes { get; set; }

        public Result[] getResultsFor(Survey survey)
        {
            var query = this.Votes.Where(v => v.Survey.Equals(survey))
                    .GroupBy(v => v.Answer, v => v.UserId, (answer, users) => new Result(answer,users.Count()));
            return query.ToArray<Result>();
        }
    }
}
