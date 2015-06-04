#if BACKEND
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endif
using System.Runtime.Serialization;
using System;

namespace Qurvey.Shared.Models
{
    [DataContract]
    public class Panic
    {
#if BACKEND
        [Key]
#endif
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Course { get; set; }

        [DataMember]
        public User User { get; set; }

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

		public Panic(string course, User user) {
			this.Course = course;
			this.User = user;
			this.Created = DateTime.Now;
		}
    }
}
