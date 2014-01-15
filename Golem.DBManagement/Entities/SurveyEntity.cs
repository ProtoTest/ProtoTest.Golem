using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Golem.DBManagement.Entities
{
    [Serializable]
    public class SurveyEntity : BaseEntity
    {
        //public SurveyEntity();

        public long SurveySubmissionID { get; set; }
        public int ProviderID { get; set; }
        public string UserEmail { get; set; }
        public DateTime SubmitDate { get; set; }

    }
}
