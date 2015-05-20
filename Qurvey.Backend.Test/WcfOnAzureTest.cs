using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qurvey.Shared.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Qurvey.Shared.Response;

namespace Qurvey.Backend.Test
{
    [TestClass]
    public class WcfOnAzureTest
    {
        private const string AzureAddress = "http://qurvey12.azurewebsites.net/SurveyService.svc/";

        [TestMethod]
        public void TestSurveys()
        {
            Survey s = new Survey("Test question");
            s.Course = "TEST";
            s.Status = Survey.SurveyStatus.Published;
            string a = new JavaScriptSerializer().Serialize(s);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AzureAddress);
               // client.DefaultRequestHeaders.Accept.Clear();
               // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage n = client.GetAsync("Data/10").Result;
                Assert.IsTrue(n.IsSuccessStatusCode);

                HttpResponseMessage n2 = client.PostAsJsonAsync("postdata", "10").Result;
                Assert.IsTrue(n2.IsSuccessStatusCode);

                //HttpResponseMessage m = client.PostAsJsonAsync("savesurvey", a).Result;
                ////Assert.IsTrue(m.IsSuccessStatusCode);
                //string result = m.Content.ReadAsStringAsync().Result;
                //Assert.IsTrue(result == "0");

                HttpResponseMessage m = client.PostAsJsonAsync("getsurveys", "TEST").Result;
                Assert.IsTrue(m.IsSuccessStatusCode);
                string res = m.Content.ReadAsAsync<string>().Result;
                GetSurveysResponse r = new JavaScriptSerializer().Deserialize<GetSurveysResponse>(res);
                Assert.IsNull(r.ExceptionMessage);

                foreach (Survey s2 in r.Surveys)
                {
                    HttpResponseMessage m2 = client.PostAsJsonAsync("deletesurvey", s2).Result;
                    Assert.IsTrue(m2.IsSuccessStatusCode);
                    Assert.IsTrue(m2.Content.ReadAsStringAsync().Result == "0");
                }
            }
        }
    }
}
