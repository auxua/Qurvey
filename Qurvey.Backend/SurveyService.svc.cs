using Qurvey.Shared.Models;
using System;
using System.Linq;

namespace Qurvey.Backend
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SurveyService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SurveyService.svc or SurveyService.svc.cs at the Solution Explorer and start debugging.
    public class SurveyService : ISurveyService
    {
        public string GetData(string value)
        {
            return string.Format("You entered: {0}", value);
        }

        public string AddSurvey(Survey survey)
        {
            if (survey == null)
            {
                throw new ArgumentNullException();
            }
            using (var db = new SurveyContext())
            {
                db.Surveys.Add(survey);
                db.SaveChanges();
                return "0";
            }
        }

        public void UpdateSurvey(Survey survey)
        {
            if (survey == null)
            {
                throw new ArgumentNullException();
            }
            using (var db = new SurveyContext())
            {
                db.Entry(survey).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteSurvey(Survey survey)
        {
            if (survey == null)
            {
                throw new ArgumentNullException();
            }
            using (var db = new SurveyContext())
            {
                db.Surveys.Remove(survey);
                db.SaveChanges();
            }
        }

        public Survey[] GetSurveys()
        {
            using (var db = new SurveyContext())
            {
                var query = from s in db.Surveys
                            orderby s.Created descending
                            select s;
                return query.ToArray<Survey>();
            }
        }

        public void AddVote(Vote vote)
        {
            if (vote == null)
            {
                throw new ArgumentNullException();
            }
            using (var db = new SurveyContext())
            {
                db.Votes.Add(vote);
                db.SaveChanges();
            }
        }

        public Result[] GetVoteResult(Survey survey)
        {
            if (survey == null)
            {
                throw new ArgumentNullException();
            }
            using (var db = new SurveyContext())
            {
                return db.getResultsFor(survey);
            }
        }
    }
}
