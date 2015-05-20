using Qurvey.Shared.Models;
using System.Runtime.Serialization;

namespace Qurvey.Shared.Request
{
    [DataContract]
    public class GetVoteForUserRequest
    {
        [DataMember]
        public Survey Survey { get; set; }

        [DataMember]
        public User User { get; set; }

        public GetVoteForUserRequest()
        {

        }

        public GetVoteForUserRequest(Survey survey, User user)
        {
            this.Survey = survey;
            this.User = user;
        }
    }
}
