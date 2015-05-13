using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

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

        [DataMember]
        public int Position { get; set; }

        protected Answer()
        {

        }

        public Answer(string answerText) : this()
        {
            this.AnswerText = answerText;
        }

        public Answer(string answerText, int position)
            : this(answerText)
        {
            this.Position = position;
        }
    }
}