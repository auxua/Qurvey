using System;
using System.Runtime.Serialization;

namespace Qurvey.Shared.Request
{
    [DataContract]
    public class CountPanicsRequest
    {
        [DataMember]
        public string Course { get; set; }

        [DataMember]
        public DateTime Since { get; set; }

        public CountPanicsRequest()
        {

        }

        public CountPanicsRequest(string course, DateTime since)
        {
            this.Course = course;
            this.Since = since;
        }
    }
}
