using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Golem.Framework
{
    [XmlRpcUrl("http://127.0.0.1:5400")]
    public interface IEggPlantDriver : IXmlRpcProxy
    {
        [XmlRpcMethod("StartSession")]
        dynamic StartSession(string suitePath);

        [XmlRpcMethod("Execute")]
        XmlRpcStruct Execute(string command);

        [XmlRpcMethod("EndSession")]
        dynamic EndSession();
    }
}
