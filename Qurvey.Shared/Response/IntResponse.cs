using System.Runtime.Serialization;

namespace Qurvey.Shared.Response
{
    [DataContract]
    public class IntResponse
    {
        [DataMember]
        public int Int { get; set; }

        [DataMember]
        public string ExceptionMessage { get; set; }

        public IntResponse()
        {

        }

        public IntResponse(int theInt, string e)
        {
            this.Int = theInt;
            this.ExceptionMessage = e;
        }
    }
}
