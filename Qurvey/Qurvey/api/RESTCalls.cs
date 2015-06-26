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
using System.Threading;

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
					//http.Accept = "application/json";
					http.ContentType = "application/json";
					http.Method = "POST";

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
                        //http.Abort();
                        http = null;
						return answer;
					}
				}
				else
				{
					// For GET, use a simple httpClient
					/*HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                    var response = await client.GetAsync(new Uri(endpoint));
                    var result = await response.Content.ReadAsStringAsync();
                    T1 answer = JsonConvert.DeserializeObject<T1>(result);
                    return answer;*/

					T1 res = default(T1);

					try
					{


						var http = (HttpWebRequest)WebRequest.Create(new Uri(endpoint));
						//http.Accept = "application/json";
						//http.ContentType = "text/xml; encoding='utf-8'";
						http.Method = "GET";

						if (http.Headers == null)
							http.Headers = new WebHeaderCollection();

                        // Depends on OS!
#if (__ANDROID__ || __IOS__)

						// Should work for every Android-Version now
						http.IfModifiedSince = DateTime.UtcNow;
#else
                        // Windows Phone does not know these properties
                        http.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString("r");
#endif


						using (var response = await Task.Factory.FromAsync<WebResponse>(http.BeginGetResponse,
							http.EndGetResponse, null))
						{
							var stream = response.GetResponseStream();
							var sr = new StreamReader(stream);
							var content = sr.ReadToEnd();

							res = JsonConvert.DeserializeObject<T1>(content);
                            //http.Abort();
                            http = null;
							return res;
						}

					}
					catch (Exception)
					{
						return res;
					}
				}
			}
			catch (Exception ex)
			{
				var t = ex.Message;
				return default(T1);
			}
		}
		public static T1 RestCallSync<T1>(string input, string endpoint, bool post)
		{
			try
			{
				if (post)
				{
#if WINDOWS_PHONE
                    
                    /*var client = new HttpClient();
                    //client.BaseAddress = endpoint;

                    StringContent content = new StringContent(input, Encoding.UTF8, "application/json");
                    //content.Headers.Expires = DateTime.UtcNow;

                    //var tt = client.GetStreamAsync(endpoint).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

                    //Byte[] bytes = Encoding.UTF8.GetBytes(input);

                    var m = new HttpRequestMessage(HttpMethod.Get, endpoint);
                    //m.Content = content;

                    HttpResponseMessage resp = client.SendAsync(m).Result;

                    var t = client.PostAsync(endpoint, content).ConfigureAwait(continueOnCapturedContext:false).GetAwaiter();
                    while (!t.IsCompleted)
                    {
                        Thread.Sleep(1000);
                        
                        //t.Wait();
                    }

                    //HttpResponseMessage answer = t.Result;
                    HttpResponseMessage answer = t.GetResult();
                    //HttpResponseMessage answer = client.PostAsync(endpoint, content).GetAwaiter().GetResult();
                    string re = answer.Content.ReadAsStringAsync().Result;
                    T1 response = JsonConvert.DeserializeObject<T1>(re);
                    return response;*/

                    WebClient wc = new WebClient();
                    Uri uri = new Uri(endpoint);
                    wc.Headers["Content-Type"] = "application/json";
                    wc.Headers["Cache-Control"] = "no-cache";
                    wc.Headers["Content-Length"] = input.Length.ToString();
                    wc.UploadStringCompleted += wc_UploadStringCompleted;
                    wc.UploadStringAsync(uri, "POST", input);

                    
                    var t = getPOSTResponse();

                    T1 response = JsonConvert.DeserializeObject<T1>(t);
                    //wc.CancelAsync();
                    wc = null;
                    return response;
#else

                    /*client.PostAsync(endpoint,)
                    
                    client.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString("r");
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";

                    Byte[] bytes = Encoding.UTF8.GetBytes(input);

                    client.OpenWriteAsync(new Uri(endpoint), "POST");
                    

                    //var answer = client.UploadStringAsync(new Uri(endpoint), "POST", input);*/
                    
                    
                    var http = (HttpWebRequest)WebRequest.Create(new Uri(endpoint));
					//http.Accept = "application/json";
					http.ContentType = "application/json";
                    if (http.Headers == null)
                        http.Headers = new WebHeaderCollection();

                    // Depends on OS!
#if (__ANDROID__ || __IOS__)

						// Should work for every Android-Version now
						http.IfModifiedSince = DateTime.UtcNow;
#else
                    // Windows Phone does not know these properties
                    http.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString("r");
#endif
					http.Method = "POST";

					Byte[] bytes = Encoding.UTF8.GetBytes(input);

					using (var stream = Task.Factory.FromAsync<Stream>(http.BeginGetRequestStream,
						http.EndGetRequestStream, null).Result)
					{
                        // Write the bytes to the stream
                        stream.WriteAsync(bytes, 0, bytes.Length); 
					}
                    
					using (var response = Task.Factory.FromAsync<WebResponse>(http.BeginGetResponse,
						http.EndGetResponse, null).Result)
					{
						var stream = response.GetResponseStream();
						var sr = new StreamReader(stream);
						var content = sr.ReadToEnd();

						T1 answer = JsonConvert.DeserializeObject<T1>(content);
						return answer;
					}
#endif

				}
				else
				{
                    
                    // For GET, use a simple httpClient
					HttpClient client = new HttpClient();
                    
					var response = client.GetAsync(new Uri(endpoint)).Result;
					var result = response.Content.ReadAsStringAsync().Result;
					T1 answer = JsonConvert.DeserializeObject<T1>(result);
                    //client.CancelPendingRequests();
                    client = null;
					return answer;
				}
			}
			catch (Exception ex)
			{
				return default(T1);
			}
		}

        static void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                Uploadresult = e.Result;
            }
            catch (Exception ex)
            {
                // fail!
            }
        }

        private static string Uploadresult = null;

        private static string getPOSTResponse()
        {
            while (Uploadresult == null)
            {
                Thread.Sleep(500);
            }
            return Uploadresult;
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
		/// Workaround:
		/// Check the Token for being valid by calling the L2P Api (Only in case of errors of the tokeninfo-endpoint)
		/// </summary>
		/// <returns>true, if the token is valid</returns>
		public async static Task<bool> CheckValidTokenAsync()
		{
			string callURL = Config.L2PEndPoint + "/Ping?accessToken=" + Config.getAccessToken() + "&p=ping";
			var answer = await RestCallAsync<L2PPingData>("", callURL, false);
			if ((answer == null) || (answer.status == false))
				return false;
			return true;
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
		/// Gets all courses of the specified semester
		/// </summary>
		/// <param name="semester">the semester specifier (e.g. 14ss)</param>
		/// <returns>A representation of all courses of the semester</returns>
		public async static Task<L2PCourseInfoSetData> L2PViewAllCourseIfoBySemesterAsync(string semester)
		{
			await api.AuthenticationManager.CheckAccessTokenAsync();
			string callURL = Config.L2PEndPoint + "/viewAllCourseInfoBySemester?accessToken=" + Config.getAccessToken()+"&semester="+semester;

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