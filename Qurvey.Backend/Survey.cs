﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Qurvey.Backend
{
    [DataContract]
    public class Survey : IEquatable<Survey>
    {
        [DataMember]
        [Key]
        public int Id { get; set; }

        [DataMember]
        public string Question { get; set; }

        [DataMember]
        public DateTime Created { get; set; }

        [DataMember]
        public DateTime Modified { get; set; }

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

        public Survey(string question) : this()
        {
            Answers = new List<Answer>();
            this.Created = this.Modified = DateTime.Now;
            this.Question = question;
        }

        public void addAnswer(string answerText)
        {
            if (Answers == null)
            {
                Answers = new List<Answer>();
            }
            this.Answers.Add(new Answer(answerText));
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