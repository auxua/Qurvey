﻿using System;
using Qurvey.Shared.Models;
using Qurvey.Shared.Response;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Qurvey.Shared.Request;

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
        private async static Task<T1> CallBackendAsync<T1>(bool post, string endpoint, object input)
        {
            string inp = JsonConvert.SerializeObject(input);

            endpoint = Config.BackendBaseUrl + endpoint;
            Task<T1> res = RESTCalls.RestCallAsync<T1>(inp, endpoint, post);
            return await res;
        }

        private static T1 CallBackendSync<T1>(bool post, string endpoint, object input)
        {
            string inp = JsonConvert.SerializeObject(input);

            endpoint = Config.BackendBaseUrl + endpoint;
            T1 res = RESTCalls.RestCallSync<T1>(inp, endpoint, post);
            return res;
        }

        /// <summary>
        /// Calls the backend async using POST.
        /// </summary>
        /// <returns>The response</returns>
        /// <param name="endpoint">The endpoint</param>
        /// <param name="input">The request</param>
        /// <typeparam name="T1">The type of the response</typeparam>
        private async static Task<T1> CallBackendAsync<T1>(string endpoint, object input)
        {
            return await CallBackendAsync<T1>(true, endpoint, input);
        }

        /// <summary>
        /// Calls the backend async using GET.
        /// </summary>
        /// <returns>The response</returns>
        /// <param name="endpoint">The endpoint</param>
        /// <typeparam name="T1">The type of the response</typeparam>
        private async static Task<T1> CallBackendAsync<T1>(string endpoint)
        {
            return await CallBackendAsync<T1>(false, endpoint, null);
        }


        /// <summary>
        /// Calls the backend async without expecting a response
        /// </summary>
        /// <returns>The backend void async.</returns>
        /// <param name="endpoint">The endpoint</param>
        /// <param name="input">The request</param>
        private async static Task CallBackendVoidAsync(string endpoint, object input)
        {
            string res = await CallBackendAsync<string>(endpoint, input);
            if (res != "0")
            {
                throw new Exception(res);
            }
        }

        /// <summary>
        /// Saves a survey on the backend
        /// </summary>
        /// <param name="survey">The survey</param>
        public async static Task SaveSurveyAsync(Survey survey)
        {
            await CallBackendVoidAsync("savesurvey", survey);
        }

        /// <summary>
        /// Delete a survey on the backend
        /// </summary>
        /// <param name="survey">The survey</param>
        public async static Task DeleteSurveyAsync(Survey survey)
        {
            await CallBackendVoidAsync("deletesurvey", survey);
        }

        /// <summary>
        /// Gets the surveys for a certain course from the backend
        /// </summary>
        /// <returns>The surveys</returns>
        /// <param name="course">Course.</param>
		public async static Task<List<Survey>> GetSurveysAsync(string course)
        {
            GetSurveysResponse res = await CallBackendAsync<GetSurveysResponse>("getsurveys/" + course);
            if (!string.IsNullOrEmpty(res.ExceptionMessage))
            {
                throw new Exception(res.ExceptionMessage);
            }
			return new List<Survey>(res.Surveys);
        }

        /// <summary>
        /// Saves a vote on the backend in Azure
        /// </summary>
        /// <param name="vote">The vote</param>
        public async static Task SaveVoteAsync(Vote vote)
        {
            await CallBackendVoidAsync("savevote", vote);
        }

        /// <summary>
        /// Deletes a vote from the backend in Azure
        /// </summary>
        /// <param name="vote">The vote</param>
        public async static Task DeleteVoteAsync(Vote vote)
        {
            await CallBackendVoidAsync("deletevote", vote);
        }

        /// <summary>
        /// Gets the vote result.
        /// </summary>
        /// <returns>The vote result.</returns>
        /// <param name="survey">Survey.</param>
        public async static Task<Result[]> GetVoteResultAsync(Survey survey)
        {
            GetVoteResultResponse res = CallBackendSync<GetVoteResultResponse>(true, "getvoteresult", survey);
            if (res == null)
                return new Result[0];

            if (!string.IsNullOrEmpty(res.ExceptionMessage))
            {
                throw new Exception(res.ExceptionMessage);
            }
            return res.Results;
        }

        public async static Task<Result[]> GetVoteResultByIDAsync(int surveyID)
        {
            GetVoteResultResponse res = CallBackendSync<GetVoteResultResponse>(false, "getvoteresultbyid/"+surveyID, null);
            if (res == null)
                return new Result[0];

            if (!string.IsNullOrEmpty(res.ExceptionMessage))
            {
                throw new Exception(res.ExceptionMessage);
            }
            return res.Results;
        }

        /// <summary>
        /// Gets the vote for a user-survey pair. Returns null if the user hasnt voted on this survey
        /// </summary>
        /// <returns>The vote. Null if not-existing</returns>
        /// <param name="survey">The survey.</param>
        /// <param name="user">The User</param>
        public async static Task<Vote> GetVoteForUserAsync(Survey survey, User user)
        {
            VoteResponse res = await CallBackendAsync<VoteResponse>("getvoteforuser", new GetVoteForUserRequest(survey, user));
            if (!string.IsNullOrEmpty(res.ExceptionMessage))
            {
                throw new Exception(res.ExceptionMessage);
            }
            return res.Vote;
        }

        /// <summary>
        /// Saves a panic.
        /// </summary>
        /// <param name="panic">The Panic.</param>
        public async static Task SavePanicAsync(Panic panic)
        {
            await CallBackendVoidAsync("savepanic", panic);
        }

        /// <summary>
        /// Counts the panics for a certain course since a certain time
        /// </summary>
        /// <returns>The number of panics</returns>
        /// <param name="course">The course</param>
        /// <param name="since">The DateTime since</param>
        public async static Task<int> CountPanicsAsync(string course, DateTime since)
        {
            IntResponse res = await CallBackendAsync<IntResponse>("countpanics", new CountPanicsRequest(course, since));
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
        }

        public async static Task<int> CountLastPanics(string course, int seconds)
        {
            IntResponse res = await CallBackendAsync<IntResponse>("countlastpanics", new CountLastPanicsRequest(course, seconds));
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
        }

        public async static Task<User> CreateNewUserAsync()
        {
            UserResponse res = await CallBackendAsync<UserResponse>("createnewuser");
            // What to do on Fails? (e.g. res == null)
            if (!string.IsNullOrEmpty(res.ExceptionMessage))
            {
                throw new Exception(res.ExceptionMessage);
            }
            return res.User;
        }
    }
}