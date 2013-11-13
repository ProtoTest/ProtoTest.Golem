using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MbUnit.Framework;
using ProtoTest.Golem.WebDriver;

namespace Golem.Tests.DTVE
{
    class DTVE_GoldenList_Scripts : WebDriverTestBase
    {
        //[Test]
        //public void GL1_CelebrityDetails_01()
        //{
        //    DTVE_CelebrityDetails_Scripts GoldenList1 = new DTVE_CelebrityDetails_Scripts();
        //    GoldenList1.DTVE_0005();
        //}

        [Test]
        public void GL1_CelebrityDetails_02()
        {
            DTVE_CelebrityDetails_Scripts GoldenList1 = new DTVE_CelebrityDetails_Scripts();
            GoldenList1.DTVE_0012();
        }
    }
}
