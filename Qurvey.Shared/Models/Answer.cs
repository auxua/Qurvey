#if BACKEND
using System.ComponentModel.DataAnnotations;
#endif
using System.Runtime.Serialization;

namespace Qurvey.Shared.Models
{
    [DataContract]
    public class Answer
    {
        [DataMember]
#if BACKEND
        [Key]
#endif
        public int Id { get; set; }

        [DataMember]
        public string AnswerText { get; set; }

        [DataMember]
        public int Position { get; set; }

        protected Answer()
        {

        }

        public Answer(string answerText)
            : this()
        {
            this.AnswerText = answerText;
        }

        public Answer(string answerText, int position)
            : this(answerText)
        {
            this.Position = position;
        }

        public override bool Equals(object obj)
        {
            Answer other = obj as Answer;
            if (other == null)
                return false;

            return this.Id == other.Id && this.AnswerText.Equals(other.AnswerText) && this.Position == other.Position;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}