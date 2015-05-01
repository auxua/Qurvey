﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Qurvey.Backend
{
    [DataContract]
    public class Survey
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
        public Answer[] Answers { get; set; } 
    }
}