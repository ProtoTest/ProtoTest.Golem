namespace ProtoTest.Golem.Proxy.HAR
{
    public class Log
    {
        public Entry[] Entries { get; set; }

        public string Version { get; set; }

        public Browser Browser { get; set; }

        public Creator Creator { get; set; }

        public Page[] Pages { get; set; }

        public string Comment { get; set; }
    }
}