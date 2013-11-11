using System;

namespace ProtoTest.Golem.Proxy.HAR
{
    public class Page
    {
        public string Id { get; set; }

        public PageTimings PageTimings { get; set; }
        
        public DateTime StartedDateTime { get; set; }

        public string Title { get; set; }

        public string Comment { get; set; }
    }
}
