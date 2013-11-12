using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gallio.Model.Tree;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;
using MbUnit.Framework;
using Golem.PageObjects.DTVE;

namespace Golem.Tests.DTVE
{
    class DTVE_CelebrityDetails_Demo : WebDriverTestBase
    {
        [Test]
        public void DTVE_0012()
        {
            Config.Settings.imageCompareSettings.fuzziness = 10;
            CelebrityDetails.OpenPage(Config.Settings.runTimeSettings.EnvironmentUrl + "/celebrity/29109").
                ClickAwardsTab().
                ClickShowsTab().VerifyCelebrityImage("James Gandolfini");
        }
    }

}
