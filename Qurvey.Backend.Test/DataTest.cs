using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qurvey.Shared.Models;
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
            survey.Answers.Add(new Answer("Answer 1", 0));
            survey.Answers.Add(new Answer("Answer 2", 1));
            survey.Answers.Add(new Answer("Answer 3", 2));
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

            Assert.IsNull(GetTestSurveyFromDb());
        }

        [TestMethod]
        public void TestResultComputation()
        {
            Assert.IsNull(GetTestSurveyFromDb());

            Survey s = CreateTestSurvey();
            Survey s2 = CreateTestSurvey();
            s2.Question += "2";
            using (var db = new SurveyContext())
            {
                db.Surveys.Add(s);
                db.Surveys.Add(s2);
                db.SaveChanges();
                Vote v1 = new Vote("User1", s, s.Answers.ElementAt(0));
                db.Votes.Add(v1);
                Vote v2 = new Vote("User2", s, s.Answers.ElementAt(2));
                db.Votes.Add(v2);
                Vote v3 = new Vote("User3", s, s.Answers.ElementAt(0));
                db.Votes.Add(v3);
                Vote v4 = new Vote("User3", s2, s2.Answers.ElementAt(0));
                db.Votes.Add(v4);
                db.SaveChanges();
            }

            using (var db = new SurveyContext())
            {
                Result[] res = db.getResultsFor(s);
                Assert.IsNotNull(res);
                Assert.IsTrue(res.Length == 2);
                Assert.IsTrue(res[0].Answer.AnswerText == "Answer 1");
                Assert.IsTrue(res[0].Count == 2);
                Assert.IsTrue(res[1].Answer.AnswerText == "Answer 3");
                Assert.IsTrue(res[1].Count == 1);

                Vote[] votes = db.Votes.Where(v => v.Survey.Id == s.Id || v.Survey.Id == s2.Id).ToArray<Vote>();
                foreach (Vote v in votes)
                {
                    db.Votes.Remove(v);
                }
                db.SaveChanges();
            }

            using (var db = new SurveyContext())
            {
                db.Surveys.Attach(s);
                db.Surveys.Remove(s);
                db.Surveys.Attach(s2);
                db.Surveys.Remove(s2);
                db.SaveChanges();
            }
        }
    }
}
