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
        public int Id { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public Survey Survey { get; set; }

        [DataMember]
        public Answer Answer { get; set; }

        protected Vote()
        {

        }

        public Vote(string userId, Survey survey, Answer answer)
            : this()
        {
            this.UserId = userId;
            this.Survey = survey;
            this.Answer = answer;
        }
    }
}
