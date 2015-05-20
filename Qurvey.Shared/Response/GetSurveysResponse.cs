using Qurvey.Shared.Models;
using System.Runtime.Serialization;

namespace Qurvey.Shared.Response
{
    [DataContract]
    public class GetSurveysResponse
    {
        [DataMember]
        public Survey[] Surveys { get; set; }

        [DataMember]
        public string ExceptionMessage { get; set; }

        public GetSurveysResponse()
        {

        }

        public GetSurveysResponse(Survey[] surveys, string e)
        {
            this.Surveys = surveys;
            this.ExceptionMessage = e;
        }
    }
}
