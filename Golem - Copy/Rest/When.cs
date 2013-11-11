using System;
using ProtoTest.Golem.Core;
using RestSharp;

namespace ProtoTest.Golem.Rest
{
    public class When
    {
        private readonly IRestClient client;
        private readonly IRestRequest request;
        private Object data;
        private IRestResponse response;

        public When(IRestClient client, IRestRequest request, IRestResponse response)
        {
            this.client = client;
            this.request = request;
            this.response = response;
            data = new Object();
        }

        public Then Then
        {
            get { return new Then(client, request, response); }
        }

        public When And
        {
            get { return this; }
        }

        private void Execute(string resource, Method method)
        {
            request.Method = method;
            if (resource != "") request.Resource = resource;
            TestBase.LogEvent(string.Format("Executing {0} : {1}{2}", request.Method, client.BaseUrl, request.Resource));
            response = client.Execute(request);
            TestBase.LogEvent(string.Format("Received Response : {0}", response.StatusCode));
        }

        public When Get(string resource = "")
        {
            Execute(resource, Method.GET);
            return this;
        }

        public When Post(string resource = "")
        {
            Execute(resource, Method.POST);
            return this;
        }

        public When Put(string resource = "")
        {
            Execute(resource, Method.PUT);
            return this;
        }

        public When Delete(string resource = "")
        {
            Execute(resource, Method.DELETE);
            return this;
        }

        public When Options(string resource = "")
        {
            Execute(resource, Method.OPTIONS);
            return this;
        }
    }
}