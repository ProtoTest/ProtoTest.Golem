namespace Golem.Proxy.HAR
{
    public class Response
    {
        public Content Content { get; set; }
        public Header[] Headers { get; set; }
        public int Status { get; set; }
        public Cookie[] Cookies { get; set; }
        public int BodySize { get; set; }
        public string HttpVersion { get; set; }
        public int HeadersSize { get; set; }
        public string StatusText { get; set; }
        public string RedirectUrl { get; set; }
        public string Comment { get; set; }
    }
}