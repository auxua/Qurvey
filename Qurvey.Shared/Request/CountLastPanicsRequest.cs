using System;
using System.Runtime.Serialization;

namespace Qurvey.Shared.Request
{
    [DataContract]
    public class CountLastPanicsRequest
    {
        [DataMember]
        public string Course { get; set; }

        [DataMember]
        public int Seconds { get; set; }

        public CountLastPanicsRequest()
        {

        }

        public CountLastPanicsRequest(string course, int seconds)
        {
            this.Course = course;
            this.Seconds = seconds;
        }
    }
}
