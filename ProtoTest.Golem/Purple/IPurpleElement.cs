using System;
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
