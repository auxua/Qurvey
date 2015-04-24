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
        
        /// <summary>
        /// A generic REST-Call to an endpoint using GET or POST method
        /// 
        /// TODO: Better to throw exception/forward exception on error?
        /// </summary>
        /// <typeparam name="T1">The Datatype to await for response</typeparam>
        /// <param name="input">the data as string (ignored, if using GET)</param>
        /// <param name="endpoint">The REST-Endpoint to call</param>
        /// <param name="post">A flag indicating whether to use POST or GET</param>
        /// <returns>The datatype that has been awaited for the call or default(T1) on error</returns>
        public static T1 RestCall<T1>(string input, string endpoint, bool post)
        {
            try
            {
                var http = (HttpWebRequest)WebRequest.Create(new Uri(endpoint));
                http.Accept = "application/json";
                http.ContentType = "application/json";
                //http.ContentType = "application/x-www-form-urlencoded";
                if (post)
                {
                    http.Method = "POST";
                    //string parsedContent = "{ \"client_id\": \"" + Config.ClientID + "\", \"scope\": \"l2p.rwth userinfo.rwth\" }";
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    Byte[] bytes = encoding.GetBytes(input);
                    Stream newStream = http.GetRequestStream();
                    newStream.Write(bytes, 0, bytes.Length);
                    newStream.Close();
                }
                else
                {
                    http.Method = "GET";
                }

                var response = http.GetResponse();

                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();

                T1 answer = JsonConvert.DeserializeObject<T1>(content);

                //string content = response.Content.ReadAsStringAsync().Result;
                //Console.WriteLine(content);
                return answer;
            }
            catch (Exception ex)
            {
                var t = ex.Message;
                return default(T1);
            }
        }

        public static string L2PPingCall(string ping)
        {
            // Check Auth.
            string callURL = Config.L2PEndPoint + "/Ping?accessToken=" + Config.getAccessToken() + "&p=" + ping;
            var answer = RestCall<L2PPingData>("", callURL, false);
            return answer.comment;
        }

        /// <summary>
        /// Gets the Course Info for the provided Course
        /// </summary>
        /// <param name="cid">The course room id (14ss-xxxxx)</param>
        /// <returns>A representation of the course room or null, if no data was available</returns>
        public static L2PCourseInfoData L2PViewCourseInfo(string cid)
        {
            string callURL = Config.L2PEndPoint + "/viewCourseInfo?accessToken=" + Config.getAccessToken() + "&cid=" + cid;

            var answer = RestCall<L2PCourseInfoSetData>("", callURL, false);
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
        public static L2PCourseInfoSetData L2PViewAllCourseInfo()
        {
            string callURL = Config.L2PEndPoint + "/viewAllCourseInfo?accessToken=" + Config.getAccessToken();

            var answer = RestCall<L2PCourseInfoSetData>("", callURL, false);
            return answer;
        }


        /// <summary>
        /// Gets the Role of a user inside the coure room
        /// </summary>
        /// <param name="cid">the course room</param>
        /// <returns>A Role representation</returns>
        public static L2PRole L2PViewUserRole(string cid)
        {
            string callURL = Config.L2PEndPoint + "/viewUserRole?accessToken=" + Config.getAccessToken() + "&cid=" + cid;

            var answer = RestCall<L2PRole>("", callURL, false);
            return answer;
        }
    }
}
