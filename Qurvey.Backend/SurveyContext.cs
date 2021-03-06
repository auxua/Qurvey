﻿using Qurvey.Shared.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace Qurvey.Backend
{
    public class SurveyContext : DbContext
    {
        public SurveyContext() : base("QurveyContext") { }

        public DbSet<Survey> Surveys { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Vote> Votes { get; set; }

        public DbSet<Panic> Panics { get; set; }

        public DbSet<LogEntry> Logs { get; set; }

        public DbSet<User> Users { get; set; }

        #region Survey
        public Survey[] getSurveysFor(string course)
        {
            var query = this.Surveys.Include(s => s.Answers).Where(s => s.Course == course).OrderByDescending(s => s.Modified);
            return query.ToArray<Survey>();
        }

        public void SaveSurvey(Survey survey)
        {
            if (survey.Id == 0)
            {
                this.Surveys.Add(survey);
            }
            else
            {
                this.Surveys.Attach(survey);
            }
            this.SaveChanges();
        }

        protected void SaveAnswer(Answer answer)
        {
            if (answer.Id == 0)
            {
                this.Answers.Add(answer);
            }
            else
            {
                this.Answers.Attach(answer);
            }
            this.SaveChanges();
        }

        public void DeleteSurvey(Survey survey)
        {
            this.Votes.RemoveRange(this.Votes.Where(v => v.Survey.Id == survey.Id));
            foreach (Answer a in survey.Answers)
                this.Answers.Attach(a);

            this.Answers.RemoveRange(survey.Answers);
            this.SaveChanges();
            Survey surveyDb = this.Surveys.Where(s => s.Id == survey.Id).First<Survey>();
            this.Surveys.Attach(surveyDb);
            this.Surveys.Remove(surveyDb);
            this.SaveChanges();
        }
        #endregion Survey

        #region Vote
        public void SaveVote(Vote vote)
        {
            this.Surveys.Attach(vote.Survey);
            this.Users.Attach(vote.User);
            vote.Answer = vote.Survey.Answers.First<Answer>(a => a.Id == vote.Answer.Id);

            if (vote.Id == 0)
            {
                this.Votes.Add(vote);
            }
            else
            {
                this.Votes.Attach(vote);
            }
            this.SaveChanges();
        }

        public void DeleteVote(Vote vote)
        {
            vote.Survey = this.Surveys.Where(s => s.Id == vote.Survey.Id).First<Survey>();
            vote.User = this.Users.Where(u => u.Id == vote.User.Id).First<User>();
            vote.Answer = this.Answers.Where(a => a.Id == vote.Answer.Id).First<Answer>();
            this.Votes.Attach(vote);
            this.Votes.Remove(vote);
            this.SaveChanges();
        }

        public Vote GetVoteForUser(Survey survey, User user)
        {
            var query = this.Votes
                .Include(v => v.Answer).Include(v => v.Survey).Include(v => v.User)
                .Where(v => v.Survey.Id == survey.Id && v.User.Id == user.Id);
            if (query.Count() > 1)
            {
                this.Log(string.Format("User {0} has voted multiple times for survey {1}", user.Id, survey.Id));
            }
            return query.FirstOrDefault<Vote>();
        }
        #endregion Vote

        #region Panic
        public void SavePanic(Panic panic)
        {
            this.SaveUser(panic.User);
            if (panic.Id == 0)
            {
                this.Panics.Add(panic);
            }
            else
            {
                this.Panics.Attach(panic);
            }
            this.SaveChanges();
        }

        public void DeletePanic(Panic panic)
        {
            this.Panics.Attach(panic);
            this.Panics.Remove(panic);
            this.SaveChanges();
        }

        public int CountPanics(string course, DateTime since)
        {
            var query = this.Panics.Where(p => p.Course == course && p.Created > since);
            return query.Count();
        }

        public int CountLastPanics(string course, int seconds)
        {
            DateTime since = DateTime.Now.AddSeconds(-1 * seconds);
            return CountPanics(course, since);
        }
        #endregion Panic

        public Result[] getResultsFor(Survey survey)
        {
            var query = this.Votes.Where(v => v.Survey.Id == survey.Id)
                   .GroupBy(v => v.Answer, v => v.User, (answer, users) => new Result() { Answer = answer, Count = users.Count() })
                   .OrderBy(r => r.Answer.Position);
            return query.ToArray<Result>();
        }

        public Result[] getResultsFor(int surveyID)
        {
            var query = this.Votes.Where(v => v.Survey.Id == surveyID)
                   .GroupBy(v => v.Answer, v => v.User, (answer, users) => new Result() { Answer = answer, Count = users.Count() })
                   .OrderBy(r => r.Answer.Position);
            return query.ToArray<Result>();
        }

        public void Log(string text)
        {
            this.Logs.Add(new LogEntry(text));
            this.SaveChanges();
        }

        public void SaveUser(User user)
        {
            if (user.Id == 0)
            {
                this.Users.Add(user);
            }
            else
            {
                this.Users.Attach(user);
            }
            this.SaveChanges();
        }
    }
}