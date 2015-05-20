using Qurvey.Shared.Models;
using System.Runtime.Serialization;

namespace Qurvey.Shared.Response
{
    [DataContract]
    public class UserResponse
    {
        [DataMember]
        public User User { get; set; }

        [DataMember]
        public string ExceptionMessage { get; set; }

        public UserResponse()
        {

        }

        public UserResponse(User user, string e)
        {
            this.User = user;
            this.ExceptionMessage = e;
        }
    }
}
