using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ProtoTest.Golem.Tests.PageObjects.Google;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    class TestComponent : WebDriverTestBase
    {
        [Test]
        public void TestComponents()
        {
            OpenPage<GoogleResultsPage>("https://www.google.com/#q=selenium").clicktest("Wikipedia");
        }
    }
}
