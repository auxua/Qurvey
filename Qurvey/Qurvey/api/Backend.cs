// FAKE forces the backend to provide fake-survey data for debugging purpose
#define FAKE

using System;
using Qurvey.Shared.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;



namespace Qurvey.api
{
	/// <summary>
	/// Provides functionality to call the Backend on Azure
	/// </summary>
	public class Backend
	{
		/// <summary>
		/// Calls the backend async.
		/// </summary>
		/// <returns>The response</returns>
		/// <param name="endpoint">The endpoint</param>
		/// <param name="input">The request</param>
		/// <typeparam name="T1">The type of the response</typeparam>
		private async static Task<T1> CallBackendAsync<T1> (string endpoint, object input) {
			string inp = JsonConvert.SerializeObject (input);

			endpoint = "http://qurvey12.azurewebsites.net/SurveyService.svc/" + endpoint;
			Task<T1> res = RESTCalls.RestCallAsync<T1> (inp, endpoint, true); 
			return await res;
		}

		/// <summary>
		/// Calls the backend async without expecting a response
		/// </summary>
		/// <returns>The backend void async.</returns>
		/// <param name="endpoint">The endpoint</param>
		/// <param name="input">The request</param>
		private async static Task CallBackendVoidAsync (string endpoint, object input) {
			string res = await CallBackendAsync<string> (endpoint, input);
			if (res != "0") {
				throw new Exception (res);
			}
		}

		/// <summary>
		/// Saves a survey on the backend
		/// </summary>
		/// <param name="survey">The survey</param>
		public async static Task SaveSurveyAsync(Survey survey) {
			await CallBackendVoidAsync("savesurvey", survey);
		}

		/// <summary>
		/// Delete a survey on the backend
		/// </summary>
		/// <param name="survey">The survey</param>
		public async static Task DeleteSurveyAsync(Survey survey) {
			await CallBackendVoidAsync("deletesurvey", survey);
		}

		/// <summary>
		/// Gets the surveys for a certain course from the backend
		/// </summary>
		/// <returns>The surveys</returns>
		/// <param name="course">Course.</param>
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
			GetSurveysResponse res = await CallBackendAsync<GetSurveysResponse> ("getsurveys", course);
			if(!string.IsNullOrEmpty(res.ExceptionMessage)) {
				throw new Exception(res.ExceptionMessage);
			}
			return res.Surveys;
#endif
		}

		/// <summary>
		/// Saves a vote on the backend in Azure
		/// </summary>
		/// <param name="vote">The vote</param>
		public async static Task SaveVoteAsync(Vote vote) {
			 await CallBackendVoidAsync("savevote", vote);
		}

		/// <summary>
		/// Deletes a vote from the backend in Azure
		/// </summary>
		/// <param name="vote">The vote</param>
		public async static Task DeleteVoteAsync(Vote vote) {
			await CallBackendVoidAsync ("deletevote", vote);
		}

		/// <summary>
		/// Gets the vote result.
		/// </summary>
		/// <returns>The vote result.</returns>
		/// <param name="survey">Survey.</param>
		public async static Task<Result[]> GetVoteResultAsync(Survey survey) {
			#if FAKE
			Result r1 = new Result();
			r1.Answer = new Answer("I like it");
			r1.Count = 42;

			Result r2 = new Result();
			r2.Answer = new Answer("I don't like that");
			r2.Count = 15;

			return new Result[]{r1,r2};
			#else
			GetVoteResultResponse res = await CallBackendAsync<GetVoteResultResponse> ("getvoteresult", survey);
			if(!string.IsNullOrEmpty(res.ExceptionMessage)) {
				throw new Exception(res.ExceptionMessage);
			}
			return res.Results;
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
#if FAKE
            var ran = new System.Random();
            if (ran.Next(100) > 60)
            {
                return 7;
            }
            else
            {
                return 0;
            }
#else
            return 0;
#endif
        }

        // Idea: Save the Answer the user selected
        public async static Task SaveAnswerAsync(Survey survey, Answer answer)
        {
            // TODO:
        }
	}
}