﻿using System;
using System.Collections.Generic;
#if BACKEND
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endif
using System.Runtime.Serialization;

namespace Qurvey.Shared.Models
{
    [DataContract]
    public class Survey : IEquatable<Survey>
    {
        public enum SurveyStatus { NotPublished, Published, Terminated };

        [DataMember]
#if BACKEND
        [Key]
#endif
        public int Id { get; set; }

        [DataMember]
        public string Question { get; set; }

        [IgnoreDataMember]

        public DateTime Created { get; set; }

        [DataMember]
#if BACKEND
        [NotMapped]
#endif
        public double CreatedTimestamp
        {
            get { return DateTimeTimestampConverter.DateTimeToUnixTimestamp(Created); }
            set { Created = DateTimeTimestampConverter.UnixTimeStampToDateTime(value); }
        }

        [IgnoreDataMember]
        public DateTime Modified { get; set; }

        [DataMember]
#if BACKEND
        [NotMapped]
#endif
        public double ModifiedTimestamp
        {
            get { return DateTimeTimestampConverter.DateTimeToUnixTimestamp(Modified); }
            set { Modified = DateTimeTimestampConverter.UnixTimeStampToDateTime(value); }
        }

        [DataMember]
        public string Course { get; set; }

        [DataMember]
        public virtual ICollection<Answer> Answers { get; set; }

        [DataMember]
        public SurveyStatus Status { get; set; }

        public bool Equals(Survey other)
        {
            return this.Id == other.Id;
        }

        public Survey()
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