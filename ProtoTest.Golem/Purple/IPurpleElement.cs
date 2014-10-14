using System;
using System.Windows.Automation;


namespace ProtoTest.Golem.Purple.Elements
{
    public interface IPurpleElement
    {
        String ElementName { get; }
        String PurplePath { get; }
        AutomationElement UIAElement { get; }
    }
}
