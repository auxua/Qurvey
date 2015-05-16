// FAKE forces the backend to provide fake-survey data for debugging purpose
#define FAKE

using System;
using Qurvey.Shared.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;



namespace Qurvey.api
{
	public class Backend
	{
		private async static Task<T1> CallBackendAsync<T1> (string endpoint, object input) {
			string inp = JsonConvert.SerializeObject (input);

			endpoint = "http://qurvey.raederscheidt.de/SurveyService.svc/" + endpoint;
			Task<T1> res = RESTCalls.RestCallAsync<T1> (inp, endpoint, true); 
			return await res;
		}

		public async static Task<string> SaveSurveyAsync(Survey survey) {
			return await CallBackendAsync<string> ("savesurvey", survey);
		}

		public async static Task<string> DeleteSurveyAsync(Survey survey) {
			return await CallBackendAsync<string> ("deletesurvey", survey);
		}

		public async static Task<Survey[]> GetSurveysAsync(string course) {
#if FAKE
            Survey s1 = new Survey("Isn't it a great app?");
            s1.Created = DateTime.Now;
            s1.Modified = DateTime.Now.AddSeconds(1);
            s1.Status = Survey.SurveyStatus.Published;
            s1.Answers = new List<Answer>();
            s1.Answers.Add(new Answer("YES!"));
            s1.Answers.Add(new Answer("Maybe"));
            s1.Answers.Add(new Answer("Not really!"));
            s1.Answers.Add(new Answer("I like Cookies!"));

            Survey s2 = new Survey("Questions should not be too long because it just looks a bit creepy and nobody will read it...");
            s2.Created = DateTime.Now;
            s2.Modified = DateTime.Now.AddSeconds(2);
            s2.Status = Survey.SurveyStatus.Published;
            s2.Answers = new List<Answer>();
            s2.Answers.Add(new Answer("Right!"));
            s2.Answers.Add(new Answer("Maybe.."));
            s2.Answers.Add(new Answer("I think the same should hold for answers but I#m just a little student..."));

            Survey[] surveys = new Survey[2] { s1, s2 };
            return surveys;
#else
			return await CallBackendAsync<Survey[]> ("getsurveys", course);
#endif
		}

        // Idea: Send a panic-indicator to the backend for the course
        public async static Task SendPanicAsync(string course)
        {
            //TODO:
        }

        // get the number of panics in the last... 2mins?
        public async static Task<int> GetPanicsAync(string course)
        {
            // TODO:
            return 0;
        }

        // Idea: Save the Answer the user selected
        public async static Task SaveAnswer(Survey survey, Answer answer)
        {
            // TODO:
        }
	}
}