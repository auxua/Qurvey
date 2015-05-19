using Qurvey.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace Qurvey.Backend
{
    public class SurveyService : ISurveyService
    {
        public string GetData(string value)
        {
            return string.Format("You entered: {0}", value);
        }

        public string PostData(string value)
        {
            return string.Format("You entered: {0}", value);
        }

        public string SaveSurvey(Survey survey)
        {
            try
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
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string DeleteSurvey(Survey survey)
        {
            try
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
            catch (Exception e)
            {
                return e.Message;
            }
        }

        //public GetSurveysResponse GetSurveys(string course)
        //{
        //    try
        //    {
        //        Survey[] res = null;
        //        using (var db = new SurveyContext())
        //        {
        //            db.Configuration.LazyLoadingEnabled = false;
        //            res = db.getSurveysFor(course);
        //           //return new GetSurveysResponse(new Survey[] { new Survey("Test") }, null);
        //        }
        //        string a = new JavaScriptSerializer().Serialize(res[0]);
        //        res[0].Answers = new List<Answer>();
        //        return new GetSurveysResponse(res, null);
        //    }
        //    catch (Exception e)
        //    {
        //        return new GetSurveysResponse(null, e.Message);
        //    }
        //}

        public string GetSurveys(string course)
        {
            Survey[] res = null;
            string error = null;
            try
            {
                using (var db = new SurveyContext())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    res = db.getSurveysFor(course);
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return new JavaScriptSerializer().Serialize(new GetSurveysResponse(res, error));
        }

        public string SaveVote(Vote vote)
        {
            try
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
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string DeleteVote(Vote vote)
        {
            try
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
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public GetVoteResultResponse GetVoteResult(Survey survey)
        {
            Result[] res = null;
            string error = null;
            try
            {
                if (survey == null)
                {
                    throw new ArgumentNullException();
                }
                using (var db = new SurveyContext())
                {
                    res = db.getResultsFor(survey);
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return new GetVoteResultResponse(res, error);
        }
    }
}
