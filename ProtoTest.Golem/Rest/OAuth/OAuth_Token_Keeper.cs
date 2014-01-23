using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ProtoTest.Golem.Rest.OAuth
{
    public static class REST_VERB
    {
        public const string GET = "GET";
        public const string POST = "POST";
    }
    public static class Oauth_Signature_Methods
    {
        //enum Signautre_Methods {HMAC_SHA1, RSA_SHA1}
        public const string HMAC_SHA1 = "HMAC-SHA1";
        public const string RSA_SHA1 = "RSA-SHA1";
        public const string VERSION = "1.0";
    }
    public static class OAuth_Token_Keeper
    {
        //This class is used to store the things needed for oauthentication
        public static string oauth_token { get; set; }
        public static string oauth_token_secret { get; set; }
        public static string oauth_consumer_key { get; set; }
        public static string oauth_consumer_secret { get; set; }
        public static string oauth_signature_method { get; set; }
        public static string resource_url { get; set; }
        public static string oauth_version { get; set; }
    }
    public class OAuth_Request_Builder
    {
        private string oauth_signature; //This is the hashed string
        private string oauth_nonce;
        private string oauth_timestamp;
        private string resource_URL;
        private string baseFormat;
        private string baseString;
        private string http_verb;
       

        public OAuth_Request_Builder(string URL, string verb)
        {
            resource_URL = URL;
            http_verb = verb;
        }

        private void setNonce()
        {
            //building this at the time th request is made
            oauth_nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
        }

        private void setTimestamp()
        {
            //building this at the time the request is made
            var timeSpan = DateTime.UtcNow 
                - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();
        }

        private void BuildBaseRequest(string[] parameters)
        {
            //we need to get a basestring out of this eventually
            //There maybe some special considerations in here (i.e. Twitter API expects parameters in alphabetical order)
            baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}";

        }

        private string compositeKey()
        {
            string key;
            if (OAuth_Token_Keeper.oauth_consumer_secret != null && OAuth_Token_Keeper.oauth_token_secret != null)
            {
                key = string.Concat(Uri.EscapeDataString(OAuth_Token_Keeper.oauth_consumer_secret), "&", Uri.EscapeDataString(OAuth_Token_Keeper.oauth_token_secret));
            }
            else
            {
                //Should probably add this to logging instead of returning an invalid key
                key = "Problem with values in OAuth_Token_Keeper consumer_secret OR token_secret";
            }
            return key;
        }

        
        private void buildHash()
        {
            if (OAuth_Token_Keeper.oauth_signature_method != null)
            {
                //TODO: Need to check to make sure all the peices are set up correctly first

                if (OAuth_Token_Keeper.oauth_signature_method == Oauth_Signature_Methods.HMAC_SHA1)
                {
                    using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey())))
                    {
                        oauth_signature = Convert.ToBase64String(hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
                    }
                }
                if (OAuth_Token_Keeper.oauth_signature_method == Oauth_Signature_Methods.RSA_SHA1)
                {
                    //TODO: Figure out how to do proper RSA hashing
                }
            }
        }

        
        



    }

}
