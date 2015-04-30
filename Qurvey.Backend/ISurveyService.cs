using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Qurvey.Backend
{
    [ServiceContract]
    public interface ISurveyService
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        string AddSurvey(Survey survey);

        [OperationContract]
        void UpdateSurvey(Survey survey);

        [OperationContract]
        void DeleteSurvey(Survey survey);

        [OperationContract]
        Survey[] GetSurveys();
    }
}
