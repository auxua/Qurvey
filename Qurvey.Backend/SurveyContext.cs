using Qurvey.Shared.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
            var query = this.Votes.Where(v => v.Survey.Id == survey.Id)
                   .GroupBy(v => v.Answer, v => v.UserId, (answer, users) => new Result() { Answer = answer, Count = users.Count() })
                   .OrderBy(r => r.Answer.Position);
            return query.ToArray<Result>();
        }

        public Survey[] getSurveysFor(string course)
        {
            var query = this.Surveys.Where(s => s.Course == course).OrderByDescending(s => s.Modified);
            //Survey s2 = this.Surveys.First();
            //((IObjectContextAdapter)this).ObjectContext.Detach(s2);
            //return new Survey[] {s2  };
            return query.ToArray<Survey>();
        }
    }
}
