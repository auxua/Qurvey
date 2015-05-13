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
    public class Answer
    {
        [DataMember]
        [Key]
        public int Id { get; set; }

        [DataMember]
        public string AnswerText { get; set; }

        protected Answer()
        {

        }

        public Answer(string answerText) : this()
        {
            this.AnswerText = answerText;
        }
    }
}