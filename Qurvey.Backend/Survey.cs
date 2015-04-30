using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Qurvey.Backend
{
    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class Survey
    {
        //[DataMember]
        //[Key]
        //public int Id;
        
        //[DataMember]
        //public string Question;
        
        //[DataMember]
        //public DateTime Created;

        [DataMember]
        [Key]
        public int Id
        {
            get;
            set;
        }

        [DataMember]
        public string Question
        {
            get;
            set;
        }

        [DataMember]
        public DateTime Created
        {
            get;
            set;
        }

        [DataMember]
        List<string> answers = new List<string>(); 
    }
}