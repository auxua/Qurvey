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
        public int SurveyId { get; set; }

        [DataMember]
        public int AnswerId { get; set; }
    }
}
