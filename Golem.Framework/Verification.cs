using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Golem.Framework
{
    public class Verification
    {
        public Verification(){}

        public Verification That(bool condition){}
        public Verification ContainsText(string text){}
        public Verification ContainsAttribute(string attribute, string value){}
        public Verification IsPresent(){}
        public Verification IsVisible(){}

    }
}
