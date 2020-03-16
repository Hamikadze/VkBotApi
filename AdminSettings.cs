using System.Collections.Generic;

namespace VkBotApi
{
    public class AdminSettings
    {
        public const string TOTP_KEY = "";

        public const string VK_CONFORMATION_KEY = "";
        public const string VK_ACCESS_TOKEN = "";

        public static string YANDEX_API_KEY = "";
        public static string YANDEX_UUID = "";

        public static readonly List<string> GOOGLE_SERVER_KEYS = new List<string>()
        {
            "",
        };

        public static readonly List<string> GOOGLE_CUSTOM_SEARCH_IDs = new List<string>()
        {
            ":",
        };
    }
}