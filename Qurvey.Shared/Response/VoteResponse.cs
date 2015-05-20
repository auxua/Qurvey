using Qurvey.Shared.Models;
using System.Runtime.Serialization;

namespace Qurvey.Shared.Response
{
    [DataContract]
    public class VoteResponse
    {
        [DataMember]
        public Vote Vote { get; set; }

        [DataMember]
        public string ExceptionMessage { get; set; }

        public VoteResponse()
        {

        }

        public VoteResponse(Vote vote, string e)
        {
            this.Vote = vote;
            this.ExceptionMessage = e;
        }
    }
}
