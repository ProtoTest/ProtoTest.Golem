using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{

    [TestFixtureSource("GetName")]
    class TestSource
    {
        private string name;

        public TestSource(string name)
        {
            this.name = name;
        }

        public TestSource()
        {
        }

        public static IEnumerable<string> GetName()
        {
            yield return "one";
            yield return "two";
        }

        [Test]
        public void testone()
        {
            TestContext.WriteLine("Pass");
        }
    }
}
