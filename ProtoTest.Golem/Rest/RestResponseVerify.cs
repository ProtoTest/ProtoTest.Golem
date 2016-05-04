using System.Net;
using HtmlAgilityPack;
using ProtoTest.Golem.Core;
using RestSharp;

namespace ProtoTest.Golem.Rest
{
    /// <summary>
    ///     Contains methods to validate rest responses
    /// </summary>
    public class RestResponseVerify
    {
        private readonly IRestResponse response;

        public RestResponseVerify(IRestResponse response)
        {
            this.response = response;
        }

        public RestResponseVerify BodyContainsText(string text)
        {
            if (response.Content.Contains(text))
            {
//                TestContext.CurrentContext.IncrementAssertCount();
            }
            else
            {
                TestBase.AddVerificationError(string.Format("Response body does not contain {0}. Actual : \r\n{1}", text,
                    response.Content));
            }
            return this;
        }

        public RestResponseVerify XpathOnBody(string xpath)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(response.Content);
            if (doc.DocumentNode.SelectNodes(xpath) == null)
            {
                TestBase.AddVerificationError(string.Format("Xpath Validation Failed for '{0}'. Body : {1}", xpath,
                    response.Content));
            }
            else
            {
            }
//                TestContext.CurrentContext.IncrementAssertCount();
            return this;
        }

        public RestResponseVerify HeadersContainsText(string text)
        {
            if (response.Headers.ToString().Contains(text))
            {
//                TestContext.CurrentContext.IncrementAssertCount();
            }
            else
            {
                TestBase.AddVerificationError("Response headers did not contain '" + text + "'. Actual value : " +
                                              response.Headers);
            }
            return this;
        }

        public RestResponseVerify ResponseCode(HttpStatusCode code)
        {
            if (response.StatusCode == code)
            {
//                TestContext.CurrentContext.IncrementAssertCount();
            }
            else
            {
                TestBase.AddVerificationError("Response Code was not correct.  Epected " + code + " actual : " +
                                              response.StatusCode);
            }
            return this;
        }
    }
}