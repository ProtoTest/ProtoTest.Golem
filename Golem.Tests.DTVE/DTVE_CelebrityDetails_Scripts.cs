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
    class DTVE_CelebrityDetails_Scripts : WebDriverTestBase
    {
        #region FixtureSetups
        List<string> ListOfShows = new List<string>() { "8MM", "Be Cool", "Burt Wonderstone, The Incredible"};
        List<string> ListOfAwards = new List<string>() { "Screen Actors Guild Awards", "Emmy (Primetime)", "Golden Globe, Best Performance"};

        [FixtureSetUp]
        public void setupConfig()
        {

            Config.Settings.imageCompareSettings.fuzziness = 10;
        }
        #endregion

        [Test]
        [Category("GoldenList")]
        [Category("DesktopOnly")]
        //DTVE 0005 - Desktop Only - Description
        public void DTVE_0005()
        {
            CelebrityDetails.OpenPage(Config.Settings.runTimeSettings.EnvironmentUrl + "/celebrity/29109")
                .ExpandShowsListFirstItem();
        }

        [Test]
        [Category("GoldenList")]
        //DTVE 0012 - Test description
        public void DTVE_0012()
        {
            CelebrityDetails.OpenPage(Config.Settings.runTimeSettings.EnvironmentUrl + "/celebrity/29109").
                VerifyCelebrityDetails().
                ClickAwardsTab().
                VerifyCelebrityAwardsList(ListOfAwards).
                ClickShowsTab().
                VerifyCelebrityShowsList(ListOfShows).
                VerifyCelebrityImage("James Gandolfini");
        }
    }

}
