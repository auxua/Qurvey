// FAKE forces the backend to provide fake-survey data for debugging purpose
//#define FAKE

using System;
using Qurvey.Shared.Models;
using Qurvey.Shared.Response;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Qurvey.Shared.Request;
using System.Net.Http;
using System.Net;
using System.IO;

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
		/// <param name="post">True = use POST; False = use GET</param>
		/// <param name="endpoint">The endpoint</param>
		/// <param name="input">The request</param>
		/// <typeparam name="T1">The type of the response</typeparam>
		private async static Task<T1> CallBackendAsync<T1> (bool post, string endpoint, object input)
		{
			string inp = JsonConvert.SerializeObject (input);

			endpoint = "http://qurvey12.azurewebsites.net/SurveyService.svc/" + endpoint;
			Task<T1> res = RESTCalls.RestCallAsync<T1> (inp, endpoint, post); 
			return await res;
		}

		private static T1 CallBackendSync<T1> (bool post, string endpoint, object input) {
			string inp = JsonConvert.SerializeObject (input);

			endpoint = "http://qurvey12.azurewebsites.net/SurveyService.svc/" + endpoint;
			T1 res = RESTCalls.RestCallSync<T1> (inp, endpoint, post); 
			return res;
		}

		/// <summary>
		/// Calls the backend async using POST.
		/// </summary>
		/// <returns>The response</returns>
		/// <param name="endpoint">The endpoint</param>
		/// <param name="input">The request</param>
		/// <typeparam name="T1">The type of the response</typeparam>
		private async static Task<T1> CallBackendAsync<T1> (string endpoint, object input) {
			return await CallBackendAsync<T1> (true, endpoint, input);
		}

		/// <summary>
		/// Calls the backend async using GET.
		/// </summary>
		/// <returns>The response</returns>
		/// <param name="endpoint">The endpoint</param>
		/// <typeparam name="T1">The type of the response</typeparam>
		private async static Task<T1> CallBackendAsync<T1> (string endpoint) {
			return await CallBackendAsync<T1> (false, endpoint, null);
		}


		/// <summary>
		/// Calls the backend async without expecting a response
		/// </summary>
		/// <returns>The backend void async.</returns>
		/// <param name="endpoint">The endpoint</param>
		/// <param name="input">The request</param>
		private async static Task CallBackendVoidAsync (string endpoint, object input)
		{
			string res = await CallBackendAsync<string> (endpoint, input);
			if (res != "0") {
				throw new Exception (res);
			}
		}

		/// <summary>
		/// Saves a survey on the backend
		/// </summary>
		/// <param name="survey">The survey</param>
		public async static Task SaveSurveyAsync (Survey survey)
		{
			await CallBackendVoidAsync ("savesurvey", survey);
		}

		/// <summary>
		/// Delete a survey on the backend
		/// </summary>
		/// <param name="survey">The survey</param>
		public async static Task DeleteSurveyAsync (Survey survey)
		{
			await CallBackendVoidAsync ("deletesurvey", survey);
		}

		/// <summary>
		/// Gets the surveys for a certain course from the backend
		/// </summary>
		/// <returns>The surveys</returns>
		/// <param name="course">Course.</param>
		public async static Task<Survey[]> GetSurveysAsync (string course)
		{
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
			GetSurveysResponse res = await CallBackendAsync<GetSurveysResponse> ("getsurveys/"+ course);

			/*string endpoint = "http://qurvey12.azurewebsites.net/SurveyService.svc/" + "getsurveys/"+ course;
			HttpClient client = new HttpClient();
			var response = await client.GetAsync(new Uri(endpoint));
			var result = await response.Content.ReadAsStringAsync();
			GetSurveysResponse res = JsonConvert.DeserializeObject<GetSurveysResponse>(result);*/

            // Do it this manual way to avoid Caching mechanisms!
            /*GetSurveysResponse res = null;

            try
            {
                

                var http = (HttpWebRequest)WebRequest.Create(new Uri("http://qurvey12.azurewebsites.net/SurveyService.svc/" + "getsurveys/" + course));
                //http.Accept = "application/json";
                //http.ContentType = "text/xml; encoding='utf-8'";
                http.Method = "GET";

                if (http.Headers == null)
                    http.Headers = new WebHeaderCollection();

                http.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString("r");

                using (var response = await Task.Factory.FromAsync<WebResponse>(http.BeginGetResponse,
                                 http.EndGetResponse, null))
                {
                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();

                    res = JsonConvert.DeserializeObject<GetSurveysResponse>(content);
                }
            }
            catch (Exception)
            {

            }*/
            if (!string.IsNullOrEmpty (res.ExceptionMessage)) {
				throw new Exception (res.ExceptionMessage);
			}
			return res.Surveys;
#endif
		}

		/// <summary>
		/// Saves a vote on the backend in Azure
		/// </summary>
		/// <param name="vote">The vote</param>
		public async static Task SaveVoteAsync (Vote vote)
		{
			await CallBackendVoidAsync ("savevote", vote);
		}

		/// <summary>
		/// Deletes a vote from the backend in Azure
		/// </summary>
		/// <param name="vote">The vote</param>
		public async static Task DeleteVoteAsync (Vote vote)
		{
			await CallBackendVoidAsync ("deletevote", vote);
		}

		/// <summary>
		/// Gets the vote result.
		/// </summary>
		/// <returns>The vote result.</returns>
		/// <param name="survey">Survey.</param>
		public async static Task<Result[]> GetVoteResultAsync (Survey survey)
		{
			//GetVoteResultResponse res = await CallBackendAsync<GetVoteResultResponse> ("getvoteresult", survey);
			GetVoteResultResponse res = CallBackendSync<GetVoteResultResponse>(true, "getvoteresult", survey);
            //GetVoteResultResponse res = CallBackendSync<GetVoteResultResponse>(true, "getvoteresult", "");
			if (res == null)
				return new Result[0];
			
			if (!string.IsNullOrEmpty (res.ExceptionMessage)) {
				throw new Exception (res.ExceptionMessage);
			}
			return res.Results;
		}

		/// <summary>
		/// Gets the vote for a user-survey pair. Returns null if the user hasnt voted on this survey
		/// </summary>
		/// <returns>The vote. Null if not-existing</returns>
		/// <param name="survey">The survey.</param>
		/// <param name="user">The User</param>
		public async static Task<Vote> GetVoteForUserAsync (Survey survey, User user)
		{
			VoteResponse res = await CallBackendAsync<VoteResponse> ("getvoteforuser", new GetVoteForUserRequest(survey, user));
			if (!string.IsNullOrEmpty (res.ExceptionMessage)) {
				throw new Exception (res.ExceptionMessage);
			}
			return res.Vote;
		}

		/// <summary>
		/// Saves a panic.
		/// </summary>
		/// <param name="panic">The Panic.</param>
		public async static Task SavePanicAsync (Panic panic)
		{
			await CallBackendVoidAsync ("savepanic", panic);
		}

		/// <summary>
		/// Counts the panics for a certain course since a certain time
		/// </summary>
		/// <returns>The number of panics</returns>
		/// <param name="course">The course</param>
		/// <param name="since">The DateTime since</param>
		public async static Task<int> CountPanicsAsync (string course, DateTime since)
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
			IntResponse res = await CallBackendAsync<IntResponse> ("countlastpanics", new CountPanicsRequest(course, since));
            // In case of an error keep the App working
            if (res == default(IntResponse))
            {
                return 0;
            }
			if (!string.IsNullOrEmpty (res.ExceptionMessage)) {
				throw new Exception (res.ExceptionMessage);
			}
			return res.Int;
#endif
		}

        public async static Task<int> CountLastPanics(string course, int seconds)
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
            IntResponse res = await CallBackendAsync<IntResponse>("countpanics", new CountLastPanicsRequest(course, seconds));
            // In case of an error keep the App working
            if (res == default(IntResponse))
            {
                return 0;
            }
            if (!string.IsNullOrEmpty(res.ExceptionMessage))
            {
                throw new Exception(res.ExceptionMessage);
            }
            return res.Int;
#endif
        }

		public async static Task<User> CreateNewUserAsync ()
		{
			UserResponse res = await CallBackendAsync<UserResponse> ("createnewuser");
			if (!string.IsNullOrEmpty (res.ExceptionMessage)) {
				throw new Exception (res.ExceptionMessage);
			}
			return res.User;
		}
	}
}