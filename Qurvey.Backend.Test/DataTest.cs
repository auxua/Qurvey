using System;
using Qurvey.Backend;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Qurvey.Backend.Test
{
    [TestClass]
    public class DataTest
    {
        private const string Title = "Test Question 124";

        [TestMethod]
        public void TestSurveyData()
        {
            Assert.IsNull(GetTestSurveyFromDb());

            Survey survey = CreateTestSurvey();
            using (var db = new SurveyContext())
            {
                db.Surveys.Add(survey);
                db.SaveChanges();
            }

            Survey fromDb;
            using (var db = new SurveyContext())
            {
                fromDb = db.Surveys.Where(s => s.Question == Title).FirstOrDefault<Survey>();
                Assert.IsNotNull(fromDb);
                Assert.IsTrue(fromDb.Question == Title);
                Assert.IsTrue(fromDb.Answers.Count == 3);
                Assert.IsTrue(fromDb.Answers.Where<Answer>(a => a.AnswerText == "Answer 2").Count() == 1);

                db.Surveys.Remove(fromDb);
                db.SaveChanges();
            }

            Assert.IsNull(GetTestSurveyFromDb());
        }

        private Survey GetTestSurveyFromDb()
        {
            // Check that test question is not 
            using (var db = new SurveyContext())
            {
                return db.Surveys.Where(s => s.Question == Title).FirstOrDefault<Survey>();
            }
        }

        private Survey CreateTestSurvey()
        {
            Survey survey = new Survey(Title);
            survey.Answers =
                new List<Answer>();
            survey.Answers.Add(new Answer("Answer 1"));
            survey.Answers.Add(new Answer("Answer 2"));
            survey.Answers.Add(new Answer("Answer 3"));
            return survey;
        }

        [TestMethod]
        public void TestVoteData()
        {
            Assert.IsNull(GetTestSurveyFromDb());
            Survey survey = CreateTestSurvey();
            using (var db = new SurveyContext())
            {
                db.Surveys.Add(survey);
                Vote v1 = new Vote("User1", survey, survey.Answers.ElementAt(0));
                db.Votes.Add(v1);
                Vote v2 = new Vote("User2", survey, survey.Answers.ElementAt(2));
                db.Votes.Add(v2);
                Vote v3 = new Vote("User3", survey, survey.Answers.ElementAt(0));
                db.Votes.Add(v3);
                db.SaveChanges();
            }

            using (var db = new SurveyContext())
            {
                db.Surveys.Attach(survey);
                var query = db.Votes.Where(v => v.Survey.Id == survey.Id);
                Assert.IsNotNull(query);
                Assert.IsTrue(query.Count() == 3);
                Assert.IsTrue(query.Where(v => v.UserId == "User3").Count() == 1);

                foreach(Vote v in query) {
                    db.Votes.Remove(v);
                }
                db.Surveys.Remove(survey);
                db.SaveChanges();
            }
        }
    }
}
