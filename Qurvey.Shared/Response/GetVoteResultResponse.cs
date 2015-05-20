using Qurvey.Shared.Models;
using System.Runtime.Serialization;

namespace Qurvey.Shared.Response
{
    [DataContract]
    public class GetVoteResultResponse
    {
        [DataMember]
        public Result[] Results { get; set; }

        [DataMember]
        public string ExceptionMessage { get; set; }

        public GetVoteResultResponse()
        {

        }

        public GetVoteResultResponse(Result[] results, string e)
        {
            this.Results = results;
            this.ExceptionMessage = e;
        }
    }
}
