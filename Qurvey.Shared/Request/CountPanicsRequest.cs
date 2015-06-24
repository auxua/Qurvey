using System;
using System.Runtime.Serialization;

namespace Qurvey.Shared.Request
{
    [DataContract]
    public class CountPanicsRequest
    {
        [DataMember]
        public string Course { get; set; }

        [IgnoreDataMember]
        public DateTime Since { get; set; }

        [DataMember]
        public double SinceTimestamp
        {
            get { return DateTimeTimestampConverter.DateTimeToUnixTimestamp(Since); }
            set { Since = DateTimeTimestampConverter.UnixTimeStampToDateTime(value); }
        }

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
