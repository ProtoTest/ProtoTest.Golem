using System;

namespace ProtoTest.Golem.Proxy.HAR
{
    public class Entry
    {
        public int Time { get; set; }

        public Request Request { get; set; }

        public Response Response { get; set; }

        public DateTime StartedDateTime { get; set; }

        public Timings Timings { get; set; }

        public string PageRef { get; set; }

        public Cache Cache { get; set; }

        public string ServerIpAddress { get; set; }

        public string Connection { get; set; }

        public string Comment { get; set; }
    }
}