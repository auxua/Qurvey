using Qurvey.Shared.Models;
using System;
using System.Linq;

namespace Qurvey.Backend
{
    public class SurveyService : ISurveyService
    {
        public string GetData(string value)
        {
            return string.Format("You entered: {0}", value);
        }

        public string SaveSurvey(Survey survey)
        {
            if (survey == null)
            {
                throw new ArgumentNullException();
            }
            using (var db = new SurveyContext())
            {
                if (survey.Id == 0)
                {
                    db.Surveys.Add(survey);
                }
                else
                {
                    db.Surveys.Attach(survey);
                }
                db.SaveChanges();
            }
            return "0";
        }

        public string DeleteSurvey(Survey survey)
        {
            if (survey == null)
            {
                throw new ArgumentNullException();
            }
            using (var db = new SurveyContext())
            {
                db.Surveys.Attach(survey);
                db.Surveys.Remove(survey);
                db.SaveChanges();
            }
            return "0";
        }

        public Survey[] GetSurveys(string course)
        {
            using (var db = new SurveyContext())
            {
                var query = db.Surveys.Where(s => s.Course == course).OrderByDescending(s => s.Modified);
                return query.ToArray<Survey>();
            }
        }

        public string SaveVote(Vote vote)
        {
            if (vote == null)
            {
                throw new ArgumentNullException();
            }
            using (var db = new SurveyContext())
            {
                if (vote.Id == 0)
                {
                    db.Votes.Add(vote);
                }
                else
                {
                    db.Votes.Attach(vote);
                }
                db.SaveChanges();
            }
            return "0";
        }

        public string DeleteVote(Vote vote)
        {
            if (vote == null)
            {
                throw new ArgumentNullException();
            }
            using (var db = new SurveyContext())
            {
                db.Votes.Remove(vote);
                db.SaveChanges();
            }
            return "0";
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
