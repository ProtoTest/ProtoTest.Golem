namespace Golem.Proxy.HAR
{
    public class Request
    {
        public string Method { get; set; }
        public QueryStringItem[] QueryString { get; set; }
        public PostData PostData { get; set; }
        public Cookie[] Cookies { get; set; }
        public Header[] Headers { get; set; }
        public int BodySize { get; set; }
        public string Url { get; set; }
        public string HttpVersion { get; set; }
        public int HeadersSize { get; set; }
        public string Comment { get; set; }
    }
}