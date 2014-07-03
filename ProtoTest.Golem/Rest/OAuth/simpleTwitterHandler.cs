using System;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using ProtoTest.Golem.Rest.OAuth;


namespace TwitterOAuth
{
    class simpleTwitterHandler
    {
        public static void postMessage(string message)
        {
            // oauth application keys
            var oauth_token = OAuth_Token_Keeper.oauth_consumer_key;
            var oauth_token_secret = OAuth_Token_Keeper.oauth_token_secret;
            var oauth_consumer_key = OAuth_Token_Keeper.oauth_consumer_key;
            var oauth_consumer_secret = OAuth_Token_Keeper.oauth_consumer_secret;

            // oauth implementation details
            var oauth_version = "1.0";
            var oauth_signature_method = "HMAC-SHA1";
           

            // unique request details
            var oauth_nonce = Convert.ToBase64String(
                new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var timeSpan = DateTime.UtcNow
                - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            // message api details
            var status = message;
            var resource_url = "https://api.twitter.com/1.1/statuses/update.json";

            // create oauth signature
            var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                            "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&status={6}";

            var baseString = string.Format(baseFormat,
                                        oauth_consumer_key,
                                        oauth_nonce,
                                        oauth_signature_method,
                                        oauth_timestamp,
                                        oauth_token,
                                        oauth_version,
                                        Uri.EscapeDataString(status)
                                        );

            baseString = string.Concat("POST&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                                    "&", Uri.EscapeDataString(oauth_token_secret));

            string oauth_signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            //create the request header
            var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                               "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                               "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                               "oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                                    Uri.EscapeDataString(oauth_nonce),
                                    Uri.EscapeDataString(oauth_signature_method),
                                    Uri.EscapeDataString(oauth_timestamp),
                                    Uri.EscapeDataString(oauth_consumer_key),
                                    Uri.EscapeDataString(oauth_token),
                                    Uri.EscapeDataString(oauth_signature),
                                    Uri.EscapeDataString(oauth_version)
                            );


            // make the request
            var postBody = "status=" + Uri.EscapeDataString(status);

            ServicePointManager.Expect100Continue = false;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers.Add("Authorization", authHeader);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            using (Stream stream = request.GetRequestStream())
            {
                byte[] content = ASCIIEncoding.ASCII.GetBytes(postBody);
                stream.Write(content, 0, content.Length);
            }
            var response = request.GetResponse();
            string responsetext = response.ToString();

            
        }

        public static string getTweets()
        {
            // oauth application keys
            var oauth_token = OAuth_Token_Keeper.oauth_token;
            var oauth_token_secret = OAuth_Token_Keeper.oauth_token_secret;
            var oauth_consumer_key = OAuth_Token_Keeper.oauth_consumer_key;
            var oauth_consumer_secret = OAuth_Token_Keeper.oauth_consumer_secret;

            // oauth implementation details
            var oauth_version = "1.0";
            var oauth_signature_method = "HMAC-SHA1";

            // unique request details
            var oauth_nonce = Convert.ToBase64String(
                new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var timeSpan = DateTime.UtcNow
                - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            // message api details
            //var status = "Updating status via REST API if this works";
            var resource_url = "https://api.twitter.com/1.1/statuses/user_timeline.json";

            // create oauth signature
            var baseFormat = "count=1&oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                            "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&screen_name=HackathonRamrod";

            var baseString = string.Format(baseFormat,
                                        oauth_consumer_key,
                                        oauth_nonce,
                                        oauth_signature_method,
                                        oauth_timestamp,
                                        oauth_token,
                                        oauth_version
                                        //Uri.EscapeDataString(status)
                                        );

            baseString = string.Concat("GET&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                                    "&", Uri.EscapeDataString(oauth_token_secret));

            string oauth_signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            // create the request header
            var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                               "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                               "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                               "oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                                    Uri.EscapeDataString(oauth_nonce),
                                    Uri.EscapeDataString(oauth_signature_method),
                                    Uri.EscapeDataString(oauth_timestamp),
                                    Uri.EscapeDataString(oauth_consumer_key),
                                    Uri.EscapeDataString(oauth_token),
                                    Uri.EscapeDataString(oauth_signature),
                                    Uri.EscapeDataString(oauth_version)
                            );


            // make the request
            //var postBody = "status=" + Uri.EscapeDataString(status);

            ServicePointManager.Expect100Continue = false;
            resource_url += "?screen_name=HackathonRamrod&count=1";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers.Add("Authorization", authHeader);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            var response = (HttpWebResponse) request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            var objText = reader.ReadToEnd();

            return objText;
        }

        public static string humanTweet(string machineTweet)
        {
            string realtweet = "";
            int created = machineTweet.IndexOf("{\"created_at\":") + 15;
            int tweetid = machineTweet.IndexOf("\"id\":", created) + 5;
            int endtweetid = machineTweet.IndexOf(",", tweetid);
            int tweet = machineTweet.LastIndexOf("\"text\":\"") + 8;
            int endTweet = machineTweet.LastIndexOf("\",\"source\"");

            int createdcount = (tweetid - 7) - created;
            int tweetidcount = endtweetid - tweetid;
            int tweetcount = endTweet - tweet;

            realtweet += machineTweet.Substring(created, createdcount);
            realtweet += "\n";
            realtweet += machineTweet.Substring(tweetid, tweetidcount);
            realtweet += "\n";
            realtweet += machineTweet.Substring(tweet, tweetcount);

            return realtweet;
        }
    }
}
