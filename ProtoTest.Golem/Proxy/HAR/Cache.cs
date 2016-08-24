namespace Golem.Proxy.HAR
{
    public class Cache
    {
        public CacheEntry BeforeRequest { get; set; }
        public CacheEntry AfterRequest { get; set; }
        public string Comment { get; set; }
    }
}