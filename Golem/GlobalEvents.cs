using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Golem
{
    class GlobalEvents
    {
        public GlobalEvents() { }

        public static event ActionEvent beforeTestEvent;
        public static event ActionEvent afterTestEvent;
        private EventArgs e = null;

        public delegate void ActionEvent(string name, DateTime time, EventArgs e);


    }
}
