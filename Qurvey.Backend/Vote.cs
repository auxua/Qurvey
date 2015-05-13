using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Qurvey.Backend
{
    [DataContract]
    public class Vote
    {
        [DataMember]
        [Key]
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
