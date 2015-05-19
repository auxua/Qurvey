using Qurvey.Shared.Models;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Qurvey.Backend
{
    [ServiceContract]
    public interface ISurveyService
    {
        [OperationContract]
        [WebGet(UriTemplate="Data/{value}", ResponseFormat= WebMessageFormat.Json)]
        string GetData(string value);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "postdata")]
        string PostData(string value);

        [OperationContract]
        [WebInvoke(Method="POST", ResponseFormat=WebMessageFormat.Json, RequestFormat=WebMessageFormat.Json, BodyStyle=WebMessageBodyStyle.Bare, UriTemplate="savesurvey")]
        string SaveSurvey(Survey survey);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "deletesurvey")]
        string DeleteSurvey(Survey survey);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "getsurveys")]
        string GetSurveys(string course);

        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "getsurveys2")]
        //string GetSurveys2(string course);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "savevote")]
        string SaveVote(Vote vote);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "deletevote")]
        string DeleteVote(Vote vote);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "getvoteresult")]
        Result[] GetVoteResult(Survey survey);
    }
}
