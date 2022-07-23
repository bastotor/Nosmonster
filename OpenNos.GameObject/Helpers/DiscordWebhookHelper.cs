using Newtonsoft.Json;
using OpenNos.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.GameObject.Helpers
{
    [JsonObject]
    internal interface IEmbedDimension
    {
        #region Properties

        [JsonProperty("height")]
        int Height { get; set; }

        [JsonProperty("width")]
        int Width { get; set; }

        #endregion Properties
    }

    [JsonObject]
    internal interface IEmbedIconProxyUrl
    {
        #region Properties

        [JsonProperty("proxy_icon_url")]
        string ProxyIconUrl { get; set; }

        #endregion Properties
    }

    [JsonObject]
    internal interface IEmbedIconUrl
    {
        #region Properties

        [JsonProperty("icon_url")]
        string IconUrl { get; set; }

        #endregion Properties
    }

    [JsonObject]
    internal interface IEmbedProxyUrl
    {
        #region Properties

        [JsonProperty("proxy_url")]
        string ProxyUrl { get; set; }

        #endregion Properties
    }

    [JsonObject]
    internal interface IEmbedUrl
    {
        #region Properties

        [JsonProperty("url")]
        string Url { get; set; }

        #endregion Properties
    }

    public static class DiscordWebhookHelper
    {
        #region Members

        private static readonly HttpClient _httpClient;
        private static readonly string _webhookUrlGM;
        private static readonly string _webhookUrlBazar;
        private static readonly string _webhookUrl;
        private static readonly string _webhookUrlLog;
        private static readonly string _webhookUrlLogPVP;
        private static readonly string _webhookUrlEventRaid;
        private static string _webhookUrlEventRaidEnd;
        private static readonly string _webhookUrlEvent;
        private static readonly string _webhookUrlStatServer;
        private static readonly string _webhookUrlDrop;
        private static readonly string _webhookUrlPickUp;
        private static readonly string _webhookUrlEchange;
        private static readonly string _webhookUrlEventSay; // TchatGlobal
        private static readonly string _webhookUrlEventSay2; // TchatFafa
        private static readonly string _webhookUrlEventSay3; // Whisper
        private static readonly string _webhookUrlEventSay4; // TchatGroup

        #endregion Members

        #region Instantiation

        static DiscordWebhookHelper()
        {
            _httpClient = new HttpClient();
            _webhookUrlBazar = "https://discord.com/api/webhooks/948237173981855804/SypXrXsPV9B5yTVzCxf0HDC6MCWnHaFk89hiHY4SAFbP2eMoXg62zAvUXUIKw7p8m-cT";
            _webhookUrl = "";
            _webhookUrlLog = "";
            _webhookUrlLogPVP = "https://discord.com/api/webhooks/948239282135531530/Jdwkw__5KzNOZXxayQIFUeAx-U4z4E4o3QybAEKAEcb7r9GtJuwt78ckqogzTF-Nmu3q";
            _webhookUrlEventRaidEnd = "";
            _webhookUrlEventRaid = "https://discord.com/api/webhooks/855732361134997507/jtWjQFz1_umCFM35f1mXVa1ir3mpWXaYZvbyZ9AVCAkaOPOhx92C4sqLvUbDvKSHwLXY";
            _webhookUrlEvent = "";
            _webhookUrlStatServer = "https://discord.com/api/webhooks/951587016888438914/Xx20SEG_ZmNrd3nyrFy3re-33f52LhqB91yswtgjtu9erNjLKXW9mTn6RzBGtfvR-TQy";
            _webhookUrlPickUp = "https://discord.com/api/webhooks/951585471128027196/CKCBrG5il17rKxSRBrm1HdQLV8ZbCP_-iY11NT44kZUVsItgIydLRo0qi9xs3lgV5tUG";
            _webhookUrlDrop = "https://discord.com/api/webhooks/951585337577201724/k6wCh946UPtoLrU4tngnl7e54_P0yv520QOo9Y9FWPA3lol_A_hW_81-VGz3pLUrbpsU";
            _webhookUrlEchange = "https://discord.com/api/webhooks/951585199722987561/O7v1cnoVSt-wQDQfKPrsiBJTazl3dFoptPWGOUn0Jpu7IMVtJ_GTdrMOh1A7PHvIbW9z";
            _webhookUrlEventSay = "https://discord.com/api/webhooks/948327714417442826/9aN0-KcShcO1CWXx_YaYcPoubobK1oYjU94gjY5v3nMxNZqlWT0W4RFfFokE4_4wa4Te";
            _webhookUrlEventSay2 = "https://discord.com/api/webhooks/948330154181140490/EondRmhTovRSjzuhDLeXC8jKeHya53oHxn8vFhT8Mo8cegV_urE7FxxefO04pxV4S8YI";
            _webhookUrlEventSay3 = "https://discord.com/api/webhooks/948327133405671525/b-bM1I7tf4r9_5oCHpaBMhP8hIW8ALE-NswuqpDvgZLs60DDl0BfisSDGrJRGbzdjfBn";
            _webhookUrlEventSay4 = "https://discord.com/api/webhooks/951587593424887808/ZBlc8wkoF5213Yr7UdqTo6f6RaldRtDfS9z2knwMO8EC9akU403M2GbbwREToHosV1Fp";
            _webhookUrlGM = "";
        }

        #endregion Instantiation

        #region Methods

        internal static Embed GenerateEmbed(string reason, string username, string adminName, DateTime dateEnd, PenaltyType penalty)
        {
            int color = 0;
            switch (penalty)
            {
                case PenaltyType.Banned:
                    color = 16711680;
                    break;

                case PenaltyType.Muted:
                    color = 16772608;
                    break;

                case PenaltyType.Warning:
                    color = 16754432;
                    break;

                case PenaltyType.BlockExp:
                    color = 47871;
                    break;

                case PenaltyType.BlockFExp:
                    color = 34047;
                    break;

                case PenaltyType.BlockRep:
                    color = 13500671;
                    break;
            }
            List<EmbedField> fields = new List<EmbedField>
            {
                new EmbedField { Name = "User:", Value = "```" + username + "```" },
                new EmbedField { Name = "Punished for:", Value = "```" + reason + "```" },
                new EmbedField { Name = "Punished till:", Value = "```" + dateEnd.ToString() + "```" }
            };
            return new Embed
            {
                Title = "Penalty Type:",
                Description = "```" + penalty + "```",
                Color = color,
                Author = new EmbedAuthor { Name = adminName },
                Fields = fields
            };
        }

        public static async Task<HttpResponseMessage> DiscordLogGM(string message)
        {
            StringContent msg =
                    new StringContent(
                        JsonConvert.SerializeObject(new WebhookObject
                        {
                            Content = "AdminLog" + " Discord Info:\n```" + message + "```",
                        }), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(_webhookUrlGM, msg).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> DiscordEventNosBazar(string message)
        {
            StringContent msg =
                    new StringContent(
                        JsonConvert.SerializeObject(new WebhookObject
                        {
                            Content = "NosBazar" + " New item on Bazaar!:\n```" + message + "```",
                        }), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(_webhookUrlBazar, msg).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> DiscordEventRaid(string message)
        {
            StringContent msg =
                    new StringContent(
                        JsonConvert.SerializeObject(new WebhookObject
                        {
                            Content = "Raid" + " Open:\n```" + message + "```",
                        }), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(_webhookUrlEventRaid, msg).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> DiscordEventRaidEnd(string message)
        {
            StringContent msg =
                    new StringContent(
                        JsonConvert.SerializeObject(new WebhookObject
                        {
                            Content = "Raid" + " Completed:\n```" + message + "```",
                        }), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(_webhookUrlEventRaidEnd, msg).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> DiscordEvent(string message)
        {
            StringContent msg =
                    new StringContent(
                        JsonConvert.SerializeObject(new WebhookObject
                        {
                            Content = "Admin" + " Discord Info:\n```" + message + "```",
                        }), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(_webhookUrl, msg).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> DiscordStatServer(string message)
        {
            StringContent msg =
                    new StringContent(
                        JsonConvert.SerializeObject(new WebhookObject
                        {
                            Content = "Admin" + " Discord Info:\n```" + message + "```",
                        }), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(_webhookUrlStatServer, msg).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> DiscordEchange(string message)
        {
            StringContent msg =
                    new StringContent(
                        JsonConvert.SerializeObject(new WebhookObject
                        {
                            Content = "Admin" + " Discord Info:\n```" + message + "```",
                        }), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(_webhookUrlEchange, msg).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> DiscordDrop(string message)
        {
            StringContent msg =
                    new StringContent(
                        JsonConvert.SerializeObject(new WebhookObject
                        {
                            Content = "Admin" + " Discord Info:\n```" + message + "```",
                        }), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(_webhookUrlDrop, msg).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> DiscordPickUp(string message)
        {
            StringContent msg =
                    new StringContent(
                        JsonConvert.SerializeObject(new WebhookObject
                        {
                            Content = "Admin" + " Discord Info:\n```" + message + "```",
                        }), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(_webhookUrlPickUp, msg).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> DiscordEventSay(string message)
        {
            StringContent msg =
                    new StringContent(
                        JsonConvert.SerializeObject(new WebhookObject
                        {
                            Content = "Admin" + " Discord Info:\n```" + message + "```",
                        }), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(_webhookUrlEventSay, msg).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> DiscordEventSay2(string message)
        {
            StringContent msg =
                    new StringContent(
                        JsonConvert.SerializeObject(new WebhookObject
                        {
                            Content = "Admin" + " Discord Info:\n```" + message + "```",
                        }), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(_webhookUrlEventSay2, msg).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> DiscordEventSay3(string message)
        {
            StringContent msg =
                    new StringContent(
                        JsonConvert.SerializeObject(new WebhookObject
                        {
                            Content = "Admin" + " Discord Info:\n```" + message + "```",
                        }), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(_webhookUrlEventSay3, msg).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> DiscordEventSay4(string message)
        {
            StringContent msg =
                    new StringContent(
                        JsonConvert.SerializeObject(new WebhookObject
                        {
                            Content = "Admin" + " Discord Info:\n```" + message + "```",
                        }), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(_webhookUrlEventSay4, msg).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> DiscordEventT(string message)
        {
            StringContent msg =
                    new StringContent(
                        JsonConvert.SerializeObject(new WebhookObject
                        {
                            Content = "Event" + " Discord Info:\n```" + message + "```",
                        }), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(_webhookUrlEvent, msg).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> DiscordEventlog(string message)
        {
            StringContent msg =
                    new StringContent(
                        JsonConvert.SerializeObject(new WebhookObject
                        {
                            Content = "AdminLog" + " Discord Info:\n```" + message + "```",
                        }), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(_webhookUrlLog, msg).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> DiscordEventlogPVP(string message)
        {
            StringContent msg =
                    new StringContent(
                        JsonConvert.SerializeObject(new WebhookObject
                        {
                            Content = "Score" + " ServerScore:\n```" + message + "```",
                        }), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(_webhookUrlLogPVP, msg).ConfigureAwait(false);
        }

        #endregion Methods
    }

    [JsonObject]
    internal class Embed : IEmbedUrl
    {
        #region Properties

        [JsonProperty("author")]
        public EmbedAuthor Author { get; set; }

        [JsonProperty("color")]
        public int Color { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("fields")]
        public List<EmbedField> Fields { get; set; } = new List<EmbedField>();

        [JsonProperty("footer")]
        public EmbedFooter Footer { get; set; }

        [JsonProperty("image")]
        public EmbedImage Image { get; set; }

        [JsonProperty("provider")]
        public EmbedProvider Provider { get; set; }

        [JsonProperty("thumbnail")]
        public EmbedThumbnail Thumbnail { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset? TimeStamp { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; } = "rich";

        public string Url { get; set; }

        [JsonProperty("video")]
        public EmbedVideo Video { get; set; }

        #endregion Properties
    }

    [JsonObject]
    internal class EmbedAuthor : EmbedUrl, IEmbedIconUrl, IEmbedIconProxyUrl
    {
        #region Properties

        public string IconUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public string ProxyIconUrl { get; set; }

        #endregion Properties
    }

    [JsonObject]
    internal class EmbedField
    {
        #region Properties

        [JsonProperty("inline")]
        public bool Inline { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        #endregion Properties
    }

    [JsonObject]
    internal class EmbedFooter : IEmbedIconUrl, IEmbedIconProxyUrl
    {
        #region Properties

        public string IconUrl { get; set; }

        public string ProxyIconUrl { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        #endregion Properties
    }

    [JsonObject]
    internal class EmbedImage : EmbedProxyUrl, IEmbedDimension
    {
        #region Properties

        public int Height { get; set; }

        public int Width { get; set; }

        #endregion Properties
    }

    [JsonObject]
    internal class EmbedProvider : EmbedUrl
    {
        #region Properties

        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion Properties
    }

    [JsonObject]
    internal abstract class EmbedProxyUrl : EmbedUrl, IEmbedProxyUrl
    {
        #region Properties

        public string ProxyUrl { get; set; }

        #endregion Properties
    }

    [JsonObject]
    internal class EmbedThumbnail : EmbedProxyUrl, IEmbedDimension
    {
        #region Properties

        public int Height { get; set; }

        public int Width { get; set; }

        #endregion Properties
    }

    [JsonObject]
    internal abstract class EmbedUrl : IEmbedUrl
    {
        #region Properties

        public string Url { get; set; }

        #endregion Properties
    }

    [JsonObject]
    internal class EmbedVideo : EmbedUrl, IEmbedDimension
    {
        #region Properties

        public int Height { get; set; }

        public int Width { get; set; }

        #endregion Properties
    }

    [JsonObject]
    internal class WebhookObject
    {
        #region Properties

        [JsonProperty("avatar_url")] public string AvatarUrl { get; set; }

        [JsonProperty("content")] public string Content { get; set; }

        [JsonProperty("embeds")] public List<Embed> Embeds { get; set; } = new List<Embed>();

        [JsonProperty("tts")] public bool IsTTS { get; set; }

        [JsonProperty("username")] public string Username { get; set; }

        #endregion Properties
    }
}