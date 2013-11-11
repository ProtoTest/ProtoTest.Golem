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
    public class SmokeTests : WebDriverTestBase
    {
       [Test]
       public void SmokeTest()
       {
           HomePage.OpenHomePage().
               GoToLoginPage().
               Login(UserTests.email1,UserTests.password);
       }
    }
}
