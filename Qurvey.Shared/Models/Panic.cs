#if BACKEND
using System.ComponentModel.DataAnnotations;
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

        [DataMember]
        public DateTime Created { get; set; }

        public Panic()
        {

        }

		public Panic(string course, User user) {
			this.Course = course;
			this.User = user;
			this.Created = DateTime.Now;
		}
    }
}
