using System;
using Qurvey.Shared.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
			return await CallBackendAsync<Survey[]> ("getsurveys", course);
		}
	}
}