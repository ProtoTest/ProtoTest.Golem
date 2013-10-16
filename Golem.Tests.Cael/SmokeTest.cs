using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using Golem.PageObjects.Cael;
using MbUnit.Framework;
using OpenQA.Selenium;

namespace Golem.Tests.Cael
{
   public class SmokeTests : TestBaseClass
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
