namespace Golem.Proxy.HAR
{
    public class Content
    {
        public int Size { get; set; }
        public int? Compression { get; set; }
        public string MimeType { get; set; }
        public string Text { get; set; }
        public string Encoding { get; set; }
        public string Comment { get; set; }
    }
}