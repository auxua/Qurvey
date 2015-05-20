#if BACKEND
using System.ComponentModel.DataAnnotations;
#endif
using System.Runtime.Serialization;

namespace Qurvey.Shared.Models
{
    [DataContract]
    public class Vote
    {
#if BACKEND
        [Key]
#endif
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public User User { get; set; }

        [DataMember]
        public Survey Survey { get; set; }

        [DataMember]
        public Answer Answer { get; set; }

        protected Vote()
        {

        }

        public Vote(User user, Survey survey, Answer answer)
            : this()
        {
            this.User = user;
            this.Survey = survey;
            this.Answer = answer;
        }
    }
}
