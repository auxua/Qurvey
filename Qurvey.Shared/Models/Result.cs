using System.Runtime.Serialization;

namespace Qurvey.Shared.Models
{
    [DataContract]
    public class Result
    {
        [DataMember]
        public Answer Answer { get; set; }

        [DataMember]
        public int Count { get; set; }

        public Result()
        {

        }

        public Result(Answer answer, int count) : this()
        {
            this.Answer = answer;
            this.Count = count;
        }
    }
}