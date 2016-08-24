using System;
using System.Net;
using RestSharp;

namespace Golem.Rest
{
    //given.Domain("http://www.google.com").Header("key","value").Authentication(username,password).when.post("/resource/id").then.responseBody.Verify().Text("text in body").header.Verify().Text("Header text");
    /// <summary>
    ///     Given class holds methods for any setup, such as url setting, and request configuring.
    /// </summary>
    public class Given
    {
        private readonly IRestClient client;
        private readonly IRestRequest request;
        private readonly IRestResponse response;

        public Given(WebProxy proxy = null)
        {
            client = new RestClient();
            response = new RestResponse();
            request = new RestRequest();
            if (proxy != null)
            {
                client.Proxy = proxy;
            }
        }

        public When When
        {
            get { return new When(client, request, response); }
        }

        public Given And
        {
            get { return this; }
        }

        public Given Token(string token, string value)
        {
            request.AddUrlSegment(token, value);
            return this;
        }

        public Given Resource(string resource)
        {
            request.Resource = resource;
            return this;
        }

        public Given Header(string name, string value)
        {
            request.AddHeader(name, value);
            return this;
        }

        public Given Domain(string domain)
        {
            client.BaseUrl = new Uri(domain);
            return this;
        }

        public Given Paramater(string name, string value)
        {
            request.AddParameter(name, value);
            return this;
        }

        public Given File(string filePath)
        {
            request.AddFile("", filePath);
            return this;
        }

        public Given Body(string bodyString)
        {
            request.AddParameter("text/xml", bodyString, ParameterType.RequestBody);
            return this;
        }

        public Given Cookie(string name, string value)
        {
            request.AddCookie(name, value);
            return this;
        }
    }
}