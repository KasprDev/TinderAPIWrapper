using Newtonsoft.Json;
using SharpTinder;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TinderClient.Tinder;
using TinderClient.Tinder.Custom;

namespace TinderClient
{
    class API
    {
        public bool UseProxy = false;
        public bool LogRequests = true;
        public bool LocationOnLogin = false;
        public string Lon { get; set; }
        public string Lat { get; set; }
        public string ProxyIP { get; set; }
        public int ProxyPort { get; set; }
        public string Platform { get; set; }
        private HttpClient _client { get; set; }
        private string URL = "https://api.gotinder.com/";
        public TinderInformation Information { get; private set; }
        public string AuthToken { get; set; }

        public API()
        {
            _client = new HttpClient();
        }

        NameValueCollection GetHeaders(bool gzip = false)
        {
            var headers = new NameValueCollection();

            headers.Add("app_version", "1025400");
            headers.Add("tinder-version", "2.54.0");

            if (Platform == "web")  { headers.Add("User-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:53.0) Gecko/20100101 Firefox/53.0"); }
            if (Platform == "ios") { headers.Add("User-agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 10_3_1 like Mac OS X) AppleWebKit/603.1.30 (KHTML, like Gecko) Version/10.0 Mobile/14E304 Safari/602.1"); }
            if (Platform == "android") { headers.Add("User-agent", "Mozilla/5.0 (Linux; Android 6.0.1; SAMSUNG SM-G570Y Build/MMB29K) AppleWebKit/537.36 (KHTML, like Gecko) SamsungBrowser/4.0 Chrome/44.0.2403.133 Mobile Safari/537.36"); }
            
            
            if (gzip)
            {
                headers.Add("accept", "application/json");
                headers.Add("accept-encoding", "gzip, deflate, br");
                headers.Add("accept-language", "en-US,en;q=0.9,la;q=0.8");
            }

            headers.Add("content-type", "application/json");
            headers.Add("vary", "Accept-Encoding");
            headers.Add("X-Auth-Token", AuthToken);

            return headers;
        }

        /// <summary>
        /// Send a PUT request to the Tinder server.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> SendPut(string path, object data)
        {
            var client = new WebClient();

            try
            {
                if (UseProxy)
                {
                    WebProxy p = new WebProxy("http://" + ProxyIP + ":" + ProxyPort.ToString());
                    client.Proxy = p;
                }

                client.Headers.Clear();
                client.Headers.Add(GetHeaders());

                var resp = await client.UploadStringTaskAsync(URL + path, WebRequestMethods.Http.Put, JsonConvert.SerializeObject(data));

                if (LogRequests)
                {
                    File.AppendAllText("request-log.txt", path + " - " + resp + Environment.NewLine + Environment.NewLine);
                }

                return resp;
            }
            catch (WebException)
            {
                return null;
            }
        }

        /// <summary>
        /// Send a DELETE request to the Tinder server.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> SendDelete(string path, object data)
        {
            var client = new WebClient();

            try
            {
                if (UseProxy)
                {
                    WebProxy p = new WebProxy("http://" + ProxyIP + ":" + ProxyPort.ToString());
                    client.Proxy = p;
                }

                client.Headers.Clear();
                client.Headers.Add(GetHeaders());

                var resp = await client.UploadStringTaskAsync(URL + path, "DELETE", JsonConvert.SerializeObject(data));

                if (LogRequests)
                {
                    File.AppendAllText("request-log.txt", path + " - " + resp + Environment.NewLine + Environment.NewLine);
                }

                return resp;
            }
            catch (WebException)
            {
                MessageBox.Show("Unable to authorize your Tinder session. Please make sure your auth token is valid and try again.", "TinderBot", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Send a POST request to the Tinder server.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> SendPost(string path, object data, bool gZip = false)
        {
            var client = new WebClient();

            try
            {
                if (UseProxy)
                {
                    WebProxy p = new WebProxy(ProxyIP, ProxyPort);
                    p.UseDefaultCredentials = true;
                    client.Proxy = p;
                }

                client.Headers.Clear();
                client.Headers.Add(GetHeaders(gZip));

                var resp = await client.UploadStringTaskAsync(URL + path, JsonConvert.SerializeObject(data));

                if (LogRequests)
                {
                    File.AppendAllText("request-log.txt", path + " - " + resp + Environment.NewLine + Environment.NewLine);
                }

                return resp;
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.ToString(), "TinderBot", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Send a GET request to the Tinder server.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<string> SendGet(string path)
        {
            var client = new WebClient();

            try
            {
                if (UseProxy)
                {
                    //MessageBox.Show($"IP: {ProxyIP} - Port: {ProxyPort}");
                    WebProxy p = new WebProxy(ProxyIP, ProxyPort);
                    p.UseDefaultCredentials = true;
                    client.Proxy = p;
                }

                client.Headers.Clear();
                client.Headers.Add(GetHeaders());

                var resp = await client.DownloadStringTaskAsync(URL + path);

                if (LogRequests)
                {
                    File.AppendAllText("request-log.txt", path + " - " + resp + Environment.NewLine + Environment.NewLine);
                }

                return resp;
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.ToString(), "TinderBot", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }






        /// <summary>
        /// Report a user
        /// </summary>
        /// <param name="userID">The user we want to report's user ID.</param>
        /// <returns></returns>
        public async Task<bool> ReportUser(string userID)
        {
            await SendPost("report/" + userID, new
            {
                cause = 1,
            });

            return true;
        }

        public async Task<FastMatches> GetUsersWhoLikedYou(int amount)
        {
            var data = await SendGet($"v2/fast-match?locale=en&count=20");
            Console.WriteLine(data);

            return JsonConvert.DeserializeObject<FastMatches>(data);
        }

        public async Task<bool> LikeFastMatchUser(long matchId, string userId)
        {
            var data = await SendPost($"like/{userId}?locale=en", new { 
                fast_match = 1,
                s_number = matchId,
                user_traveling = 1
            });

            return true;
        }

        /// <summary>
        /// Get our Tinder recommendations (who we swipe on)
        /// </summary>
        /// <returns></returns>
        public async Task<TinderRecommendation> GetRecommendations()
        {
            var data = await SendGet("v2/recs/core");
            Console.WriteLine(data);

            return JsonConvert.DeserializeObject<TinderRecommendation>(
                data);
        }

        public async Task<bool> SetContactCard(string type, string value)
        {
            var data = await SendPost("v2/profile/contact-card?locale=en", new
            {
                contact_type = type,
                contact_id = value
            }, true);

            return true;
        }

        /// <summary>
        /// Edit the bio of the account we're logged into.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// 
        public async Task<string> EditBio(string text)
        {
            var data = await SendPost("v2/profile?locale=en", new { 
                user = new {
                    bio = text
                }
            }, true);

            return data;
        }

        /// <summary>
        /// Get a list of users we've matched with.
        /// </summary>
        /// <returns></returns>
        public async Task<TinderMatches> GetMatches()
        {
            var data = await SendGet("v2/matches?count=60?message=1");
            Console.WriteLine(data);
            return JsonConvert.DeserializeObject<TinderMatches>(data);
        }


        /// <summary>
        /// Change the user's location to specific lat & lon coordinates.
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public async Task<bool> ChangeLocation(string lat, string lon)
        {
            var data = await SendPost("user/ping", new
            {
                lat = lat,
                lon = lon
            });

            return true;
        }

        /// <summary>
        /// Get all the meta information on the logged in user.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMeta()
        {
            var data = await SendGet("meta");
            Console.WriteLine(data);
            return data;
        }

        public async Task<string> SaveSettings(object setData)
        {
            var data = await SendPost("profile", setData);
            Console.WriteLine(data);
            return data;
        }

        public async Task<User> GetCurrentUser()
        {
            var data = await SendGet("profile");
            Console.WriteLine(data);
            return JsonConvert.DeserializeObject<User>(data);
        }

        public async Task<string> SendContactCard(string type, string username, string matchID)
        {
            var data = await SendPost("user/matches/" + matchID, new
            {
                message = username,
                type = "contact_card",
                contact_type = type
            });

            Console.WriteLine(data);
            return data;
        }

        public async Task<string> EditSpotify(string spotifyID)
        {
            var data = await SendPut("v2/profile/spotify/theme?locale=en", new
            {
                id = spotifyID
            });

            return data;
        }

        public async Task<string> Unmatch(string matchID)
        {
            var data = await SendDelete("user/matches/" + matchID, new { });
            return data;
        }

        public async Task<string> SendMessage(string matchID, string message)
        {
            var data = await SendPost("user/matches/" + matchID, new { 
                message
            });

            Console.WriteLine(data);
            return data;
        }

        public async Task<LikedUser> LikeUser(string userID)
        {
            var data = await SendGet($"like/{userID}?locale=en");

            return JsonConvert.DeserializeObject<LikedUser>(data);
        }
    }
}
