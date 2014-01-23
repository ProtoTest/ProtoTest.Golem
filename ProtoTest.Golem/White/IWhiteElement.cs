using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace ProtoTest.Golem.White.Elements
{
    public interface IWhiteElement
    {
        string description { get; set; }
        SearchCriteria criteria { get; set; }
        UIItem parent{ get; set; }
        UIItem getItem();
        ElementVerification Verify(int timeout);
        ElementVerification WaitUntil(int timeout);
    }
}
