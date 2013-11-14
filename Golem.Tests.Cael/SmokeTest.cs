using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;
using Golem.PageObjects.Cael;
using MbUnit.Framework;
using OpenQA.Selenium;

namespace Golem.Tests.Cael
{
    [TestFixture]
    public class SmokeTests : WebDriverTestBase
    {
       [Test, Category("Smoke Test")]
       public void SmokeTest()
       {
           string global_admin = Config.GetConfigValue("GlobalAdmin", "msiwiec@prototest.com");

           HomePage.OpenHomePage().
               GoToLoginPage().
               Login(global_admin, UserTests.password);
       }
    }
}
