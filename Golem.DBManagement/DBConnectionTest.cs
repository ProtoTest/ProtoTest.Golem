using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using MbUnit.Framework;
using Gallio.Model.Tree;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;
using Golem.DBManagement.Repositories;
using Golem.DBManagement.Entities;
using Golem.DBManagement;
using Golem.WebDriver;
using Gallio.Framework;
using Gallio.Model;


namespace Golem.DBManagement 
{

    public class DBConnectionTest : TestBase
    {
        private SurveyEntity _surveyEntity;
               
             
 
        public DBConnectionTest()
        { 
        
        }

        [Test]
        public void VerifyUserEmailBySurveyId()
        { 
            //Setup data
            var surveyId = 1;
            var setupUserEmail = "user@test.com";

            //Get row(s) from datbases
            List<SurveyEntity> surveyEntity = SurveyEntityRepository.GetSurveyInfoBySurveySubmissionId(surveyId);
            
            //Check to see of query returned any rows
            if (surveyEntity.Count > 0)
            {
                foreach (var row in surveyEntity)
                {
                    TestLog.BeginSection("Verifications");

                    if (row.UserEmail != setupUserEmail)
                    {
                        WebDriverTestBase.AddVerificationError(string.Format("Test failed. {0} and {1} do not match", row.UserEmail, setupUserEmail));
                    }
                    else
                    {
                        Common.Log("Validation passed.");
                    }

                    TestLog.End();
                }
            }
            else 
            {
                WebDriverTestBase.AddVerificationError(string.Format("Test Failed. No rows for email {0} were returned from database.",setupUserEmail));
            }
                        
        }


        [Test]
        public void VerifyProviderIdBySurveyId()
        {
            var surveyId = 3;
            int setupProviderId = 9876;

            int providerId = (SurveyEntityRepository.GetProviderIdBySurveySubmissionId(surveyId));
           

            TestLog.BeginSection("Verifications");
            //Check to see of query returned any rows


                if (providerId != setupProviderId)
                {
                    WebDriverTestBase.AddVerificationError(string.Format("Test failed. {0} and {1} do not match", providerId, setupProviderId));
                }
                else
                {
                    Common.Log("Validation passed.");
                }

                TestLog.End();

        }

        [Test]
        public void VerifyBlobData()
        {
            var id = 1;
            var blobData = (SurveyEntityRepository.GetBlobDataById(id));
            
        }

    }
}
