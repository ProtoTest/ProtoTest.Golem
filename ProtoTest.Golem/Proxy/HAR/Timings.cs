namespace ProtoTest.Golem.Proxy.HAR
{
    public class Timings
    {
        public int? Blocked { get; set; }
        public int? Dns { get; set; }
        public int? Connect { get; set; }
        public int Send { get; set; }
        public int Wait { get; set; }
        public int Receive { get; set; }
        public int? Ssl { get; set; }
        public string Comment { get; set; }
    }
}