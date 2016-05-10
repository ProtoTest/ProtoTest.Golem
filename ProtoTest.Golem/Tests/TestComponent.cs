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
        public void TestBaseComponentActsLikeElement()
        {
            var item =
                OpenPage<GoogleResultsPage>("https://www.google.com/#q=selenium")
                    .SearchItem.First(x => x.Text.Contains("wikipedia"));
            item.Highlight(-1, "grey");
            item.Verify().Visible();
        }

        [Test]
        public void TestComponents()
        { 
            var item =
                OpenPage<GoogleResultsPage>("https://www.google.com/#q=selenium")
                    .SearchItem.First(x => x.Text.Contains("wikipedia"));
            item.Description.Verify().Text("Selenium is a portable software testing framework for web applications");
        }

        [Test]
        public void TestComponentt()
        {
            var item =
                OpenPage<GoogleResultsPage>("https://www.google.com/#q=selenium").Header.Search.Verify().Visible();
        }

    }
}
