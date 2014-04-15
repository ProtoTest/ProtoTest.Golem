using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Automation;


namespace ProtoTest.Golem.Purple.Elements
{
    //TODO this needs to be refactored
    public interface IPurpleElement
    {
        String ElementName { get; }
        String PurplePath { get; }
        AutomationElement UIAElement { get; }
    }
}
