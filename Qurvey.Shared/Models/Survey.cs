using System;
using System.Collections.Generic;
#if BACKEND
using System.ComponentModel.DataAnnotations;
#endif
using System.Runtime.Serialization;

namespace Qurvey.Shared.Models
{
    [DataContract]
    public class Survey : IEquatable<Survey>
    {
        [DataMember]
#if BACKEND
        [Key]
#endif
        public int Id { get; set; }

        [DataMember]
        public string Question { get; set; }

        [DataMember]
        public DateTime Created { get; set; }

        [DataMember]
        public DateTime Modified { get; set; }

        [DataMember]
        public string Course { get; set; }

        [DataMember]
        //public Answer[] Answers { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }

        public bool Equals(Survey other)
        {
            return this.Id == other.Id;
        }

        protected Survey()
        {

        }

        public Survey(string question)
            : this()
        {
            Answers = new List<Answer>();
            this.Created = this.Modified = DateTime.Now;
            this.Question = question;
        }

        public void addAnswer(string answerText, int position)
        {
            if (Answers == null)
            {
                Answers = new List<Answer>();
            }
            this.Answers.Add(new Answer(answerText, position));
        }

        public override bool Equals(object obj)
        {
            Survey other = obj as Survey;
            if (other == null)
            {
                return false;
            }
            else
            {
                return this.Equals(other);
            }
        }

        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}