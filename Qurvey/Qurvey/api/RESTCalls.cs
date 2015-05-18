using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using Newtonsoft.Json;
using Qurvey.api.DataModel;

namespace Qurvey.api
{
    class RESTCalls
	{
        #region generic calls

        /// <summary>
        /// A generic REST-Call to an endpoint using GET or POST method
        /// 
        /// Uses a WebRequest for POST, a httpClient for GET calls
        /// 
        /// TODO: Better to throw exception/forward exception on error?
        /// </summary>
        /// <typeparam name="T1">The Datatype to await for response</typeparam>
        /// <param name="input">the data as string (ignored, if using GET)</param>
        /// <param name="endpoint">The REST-Endpoint to call</param>
        /// <param name="post">A flag indicating whether to use POST or GET</param>
        /// <returns>The datatype that has been awaited for the call or default(T1) on error</returns>
        public async static Task<T1> RestCallAsync<T1>(string input, string endpoint, bool post)
        {
            try
            {
                if (post)
                {
                    var http = (HttpWebRequest)WebRequest.Create(new Uri(endpoint));
                    http.Accept = "application/json";
                    http.ContentType = "application/json";
                    //http.ContentType = "application/x-www-form-urlencoded";
                    http.Method = "POST";
                    //string parsedContent = "{ \"client_id\": \"" + Config.ClientID + "\", \"scope\": \"l2p.rwth userinfo.rwth\" }";
                    
                    //ASCIIEncoding encoding = new ASCIIEncoding();
                    Byte[] bytes = Encoding.UTF8.GetBytes(input);

                    using (var stream = await Task.Factory.FromAsync<Stream>(http.BeginGetRequestStream,
                                                         http.EndGetRequestStream, null))
                    {
                        // Write the bytes to the stream
                        await stream.WriteAsync(bytes,0,bytes.Length);
                    }

                    using (var response = await Task.Factory.FromAsync<WebResponse>(http.BeginGetResponse,
                                     http.EndGetResponse, null))
                    {
                        var stream = response.GetResponseStream();
                        var sr = new StreamReader(stream);
                        var content = sr.ReadToEnd();

                        T1 answer = JsonConvert.DeserializeObject<T1>(content);

                        //string content = response.Content.ReadAsStringAsync().Result;
                        //Console.WriteLine(content);
                        return answer;
                    }
                }
                else
                {
                    // For GET, use a simple httpClient
                    HttpClient client = new HttpClient();
                    var response = await client.GetAsync(new Uri(endpoint));
                    var result = await response.Content.ReadAsStringAsync();
                    T1 answer = JsonConvert.DeserializeObject<T1>(result);
                    return answer;
                }
            }
            catch (Exception ex)
            {
                var t = ex.Message;
                return default(T1);
            }
        }

        #endregion

        #region L2P API

        /// <summary>
        /// Calls the Ping-API of the L2P
        /// </summary>
        /// <param name="ping">a sample text that should be returned</param>
        /// <returns>The result of the call</returns>
        public async static Task<string> L2PPingCallAsync(string ping)
        {
            // Check Auth.
            await api.AuthenticationManager.CheckAccessTokenAsync();
            string callURL = Config.L2PEndPoint + "/Ping?accessToken=" + Config.getAccessToken() + "&p=" + ping;
            var answer = await RestCallAsync<L2PPingData>("", callURL, false);
            return answer.comment;
        }

        /// <summary>
        /// Gets the Course Info for the provided Course
        /// </summary>
        /// <param name="cid">The course room id (14ss-xxxxx)</param>
        /// <returns>A representation of the course room or null, if no data was available</returns>
        public async static Task<L2PCourseInfoData> L2PViewCourseInfoAsync(string cid)
        {
            await api.AuthenticationManager.CheckAccessTokenAsync();
            string callURL = Config.L2PEndPoint + "/viewCourseInfo?accessToken=" + Config.getAccessToken() + "&cid=" + cid;

            var answer = await RestCallAsync<L2PCourseInfoSetData>("", callURL, false);
            if (answer.dataset.Count == 0)
            {
                // no elements!
                return null;
            }
            return answer.dataset[0];
        }


        /// <summary>
        /// Get all Courses of the user
        /// </summary>
        /// <returns>A representation of all courses</returns>
        public async static Task<L2PCourseInfoSetData> L2PViewAllCourseInfoAsync()
        {
            await api.AuthenticationManager.CheckAccessTokenAsync();
            string callURL = Config.L2PEndPoint + "/viewAllCourseInfo?accessToken=" + Config.getAccessToken();

            var answer = await RestCallAsync<L2PCourseInfoSetData>("", callURL, false);
            return answer;
        }


        /// <summary>
        /// Gets the Role of a user inside the coure room
        /// </summary>
        /// <param name="cid">the course room</param>
        /// <returns>A Role representation</returns>
        public async static Task<L2PRole> L2PViewUserRoleAsync(string cid)
        {
            await api.AuthenticationManager.CheckAccessTokenAsync();
            string callURL = Config.L2PEndPoint + "/viewUserRole?accessToken=" + Config.getAccessToken() + "&cid=" + cid;

            var answer = await RestCallAsync<L2PRole>("", callURL, false);
            return answer;
        }

        #endregion
    }
}
