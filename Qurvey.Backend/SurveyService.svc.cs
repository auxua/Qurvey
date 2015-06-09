using Qurvey.Shared.Models;
using Qurvey.Shared.Request;
using Qurvey.Shared.Response;
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
                    db.SaveSurvey(survey);
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
                    db.DeleteSurvey(survey);
                }
                return "0";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public GetSurveysResponse GetSurveys(string course)
        {
            Survey[] res = null;
            string error = null;
            try
            {
                using (var db = new SurveyContext())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    res = db.getSurveysFor(course);
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return new GetSurveysResponse(res, error);
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
                    db.SaveVote(vote);
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
                    db.DeleteVote(vote);
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
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    res = db.getResultsFor(survey);
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return new GetVoteResultResponse(res, error);
        }

        public GetVoteResultResponse GetVoteResultByID(string surveyID)
        {
            Result[] res = null;
            string error = null;
            try
            {
                if (surveyID == null)
                {
                    throw new ArgumentNullException();
                }
                using (var db = new SurveyContext())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    res = db.getResultsFor(Convert.ToInt32(surveyID));
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return new GetVoteResultResponse(res, error);
        }

        public VoteResponse GetVoteForUser(GetVoteForUserRequest req)
        {
            Vote vote = null;
            string error = null;
            try
            {
                if (req.Survey == null || req.User == null)
                {
                    throw new ArgumentNullException();
                }
                using (var db = new SurveyContext())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    vote = db.GetVoteForUser(req.Survey, req.User);
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return new VoteResponse(vote, error);
        }

        public string SavePanic(Panic panic)
        {
            try
            {
                if (panic == null)
                {
                    throw new ArgumentNullException();
                }
                using (var db = new SurveyContext())
                {
                    db.SavePanic(panic);
                }
                return "0";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public IntResponse CountPanics(CountPanicsRequest req)
        {
            int res = -1;
            string error = null;
            try
            {
                if (req.Course == null || req.Since == null)
                {
                    throw new ArgumentNullException();
                }
                using (var db = new SurveyContext())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    res = db.CountPanics(req.Course, req.Since);
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return new IntResponse(res, error);
        }

        public IntResponse CountLastPanics(CountLastPanicsRequest req)
        {
            int res = -1;
            string error = null;
            try
            {
                if (req.Course == null || req.Seconds == null)
                {
                    throw new ArgumentNullException();
                }
                using (var db = new SurveyContext())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    res = db.CountLastPanics(req.Course, req.Seconds);
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return new IntResponse(res, error);
        }

        public UserResponse CreateNewUser()
        {
            User res = null;
            string error = null;
            try
            {
                using (var db = new SurveyContext())
                {
                    res = User.GenerateNewUser();
                    db.SaveUser(res);
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return new UserResponse(res, error);
        }
    }
}