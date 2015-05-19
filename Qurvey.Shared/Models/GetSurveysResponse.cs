using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Qurvey.Shared.Models
{
    [DataContract]
    public class GetSurveysResponse
    {
        [DataMember]
        public Survey[] Surveys { get; set; }

        [DataMember]
        public string ExceptionMessage { get; set; }

        public GetSurveysResponse()
        {

        }

        public GetSurveysResponse(Survey[] surveys, string e)
        {
            this.Surveys = surveys;
            this.ExceptionMessage = e;
        }
    }
}
