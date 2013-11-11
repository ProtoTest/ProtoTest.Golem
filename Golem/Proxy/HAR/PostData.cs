namespace ProtoTest.Golem.Proxy.HAR
{
    public class PostData
    {
        public string MimeType { get; set; }

        public Param[] Params { get; set; }

        public string Text { get; set; }

        public string Comment { get; set; }
    }
}