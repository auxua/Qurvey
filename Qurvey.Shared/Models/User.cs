using System;
#if BACKEND
using System.ComponentModel.DataAnnotations;
#endif
using System.Linq;
using System.Runtime.Serialization;

namespace Qurvey.Shared.Models
{
    [DataContract]
    public class User
    {

#if BACKEND
        [Key]
#endif
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public DateTime Created { get; set; }

        public User()
        {

        }

        private static Random random;

        public static User GenerateNewUser()
        {
            User u = new User();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            random = new Random();
            u.Code = new string(
                Enumerable.Repeat(chars, 30)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            u.Created = DateTime.Now;
            return u;
        }
    }
}
