using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Qurvey.Shared.Models
{
    [DataContract]
    public class GetVoteResultResponse
    {
        [DataMember]
        public Result[] Results { get; set; }

        [DataMember]
        public string ExceptionMessage { get; set; }

        public GetVoteResultResponse()
        {

        }

        public GetVoteResultResponse(Result[] results, string e)
        {
            this.Results = results;
            this.ExceptionMessage = e;
        }
    }
}
