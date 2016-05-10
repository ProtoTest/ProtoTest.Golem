using System.Dynamic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using ProtoTest.Golem.Core;
using RestSharp;

namespace ProtoTest.Golem.Rest
{
    /// <summary>
    ///     Then contains post-operation commands such as validations and return statements;
    /// </summary>
    public class Then
    {
        public IRestClient client;
        public IRestRequest request;
        public IRestResponse response;

        public Then(IRestClient client, IRestRequest request, IRestResponse response)
        {
            this.client = client;
            this.request = request;
            this.response = response;
        }

        public Then And
        {
            get { return this; }
        }

        public string Body
        {
            get { return response.Content; }
        }

        public RestResponseVerify Verify(int timeoutSec = -1)
        {
            if (timeoutSec == -1) timeoutSec = Config.settings.runTimeSettings.ElementTimeoutSec;
            return new RestResponseVerify(response);
        }

        public T GetBodyAs<T>()
        {
            var generic = (RestResponse<T>) response;
            return generic.Data;
        }

        public dynamic GetBodyAsDynamic()
        {
            dynamic content;
            if (response.ContentType.Contains("json"))
            {
                content = SimpleJson.DeserializeObject(response.Content);
            }
            else
            {
                var xDoc = XDocument.Load(new StringReader(response.Content));

                dynamic root = new ExpandoObject();

                DynamicXml.Parse(root, xDoc.Elements().First());
                content = root;
            }
            return content;
        }

        public string GetBodyAsString()
        {
            return response.Content;
        }

        public string GetStringFromBody(string xpath)
        {
            var doc = new XmlDocument();

            if (response.ContentType.Contains("json"))
                doc = JsonConvert.DeserializeXmlNode(response.Content);
            else
                doc.LoadXml(response.Content);

            var node = doc.SelectSingleNode(xpath);
            return node.InnerText;
        }
    }
}