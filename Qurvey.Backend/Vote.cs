using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Qurvey.Backend
{
    [DataContract]
    public class Vote
    {

        [Key]
        public int Id { get; set; }

        [DataMember]
        //[Key]
        //[Column(Order=1)]
        public string UserId { get; set; }

        [DataMember]
        //[Key]
        //[Column(Order = 2)]
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
