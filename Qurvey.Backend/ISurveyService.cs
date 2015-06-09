using Qurvey.Shared.Models;
using Qurvey.Shared.Request;
using Qurvey.Shared.Response;
using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Qurvey.Backend
{
    [ServiceContract]
    public interface ISurveyService
    {
        [OperationContract]
        [WebGet(UriTemplate = "Data/{value}", ResponseFormat = WebMessageFormat.Json)]
        string GetData(string value);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "postdata")]
        string PostData(string value);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "savesurvey")]
        string SaveSurvey(Survey survey);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "deletesurvey")]
        string DeleteSurvey(Survey survey);

        [OperationContract]
        // [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "getsurveys")]
        [WebGet(UriTemplate = "getsurveys/{course}", ResponseFormat = WebMessageFormat.Json)]
        GetSurveysResponse GetSurveys(string course);

        [OperationContract]
        [WebGet(UriTemplate = "getvoteresultbyid/{surveyid}", ResponseFormat = WebMessageFormat.Json)]
        GetVoteResultResponse GetVoteResultByID(int surveyID);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "savevote")]
        string SaveVote(Vote vote);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "deletevote")]
        string DeleteVote(Vote vote);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "getvoteresult")]
        GetVoteResultResponse GetVoteResult(Survey survey);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "getvoteforuser")]
        VoteResponse GetVoteForUser(GetVoteForUserRequest req);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "savepanic")]
        string SavePanic(Panic panic);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "countpanics")]
        IntResponse CountPanics(CountPanicsRequest req);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "countlastpanics")]
        IntResponse CountLastPanics(CountLastPanicsRequest req);

        [OperationContract]
        [WebGet(UriTemplate = "createnewuser", ResponseFormat = WebMessageFormat.Json)]
        UserResponse CreateNewUser();
    }
}