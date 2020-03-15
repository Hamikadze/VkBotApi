using System;

namespace VkBotApi
{
    public class SiteData
    {
        internal class Google
        {
            public static int i = 0;

            public static void ChangeId()
            {
                i++;
                if (i == Math.Min(AdminSettings.GOOGLE_CUSTOM_SEARCH_IDs.Count, AdminSettings.GOOGLE_SERVER_KEYS.Count))
                    i = 0;
            }

            public static string ImageCustomSearchApi = "https://www.googleapis.com/customsearch/v1";

            public static string Domain = "www.google.ru";

            public static string MainUrl = "http://" + Domain + "/";
            public static string MainUrlS = "https://" + Domain + "/";
            public static string SiteSearch = MainUrlS + "search";
        }

        internal class Yandex
        {
            public static string coreUrl =
                $"https://asr.yandex.net/asr_xml?uuid={AdminSettings.YANDEX_UUID}&key={AdminSettings.YANDEX_API_KEY}&topic=queries";
        }

        internal class Wiki
        {
            public static string ApiUrl = "https://ru.wikipedia.org/w/api.php";
        }

        internal class Awesome
        {
            public static string MainUrlS = "https://everypixel.com/aesthetics";
            public static string ApiUrl = "https://quality.api.everypixel.com/v1/quality";
        }

        internal class VK
        {
            public static string ApiVer = "5.80";

            public static string GetAccessToken()
            {
                return AdminSettings.VK_ACCESS_TOKEN;
            }

            public static string Domain = "vk.com";
            public static string MainUrl = "http://" + Domain + "/";
            public static string MMainUrlS = "https://m." + Domain + "/";
            public static string MainUrlS = "https://" + Domain + "/";
            public static string ApiUrl = "https://api." + Domain + "/";
            public static string OAuthUrl = "https://oauth." + Domain + "/";
            public static string GroupByGid = MainUrlS + "club{0}";
            public static string UserByUid = MainUrlS + "id{0}";

            public static string
                ById = MainUrlS + "wall{0}_{1}";

            public static string NewsFeed = MainUrlS + "feed";
            public static string PhotoById = "";
            public static string OauthBlank = OAuthUrl + "blank.html";

            public static string OAuthAuth(string clientId) => OAuthUrl + $"authorize?client_id={clientId}&display=page&redirect_uri={OauthBlank}&scope=groups,photos,audio,video,wall,messages,notifications,offline&response_type=token&v={ApiVer}";

            public static string ApiMethodUrl = ApiUrl + "method/";
            public static string ApiNewsFeedSearch = ApiMethodUrl + "newsfeed.search";
            public static string ApiGroupInfo = ApiMethodUrl + "groups.getById";
            public static string ApiPhotoUploadServer = ApiMethodUrl + "photos.getOwnerPhotoUploadServer";
            public static string ApiPhotosSaveOwnerPhoto = ApiMethodUrl + "photos.saveOwnerPhoto";
            public static string ApiWallDelete = ApiMethodUrl + "wall.delete";
            public static string ApiVideoInfo = ApiMethodUrl + "video.get";
            public static string ApiPhotosGet = ApiMethodUrl + "photos.get";
            public static string ApiPhotosDelete = ApiMethodUrl + "photos.delete";
            public static string ApiCaptchaImage = ApiMethodUrl + "captcha.php?sid={0}&s=1";
            public static string ApiGetConversations = ApiMethodUrl + "messages.getConversations";
            public static string ApiAudioSearch = MainUrlS + "al_audio.php";
            public static string ApiMageAsReadMessage = ApiMethodUrl + "messages.markAsRead";
            public static string ApiGetMessagesUploadServer = ApiMethodUrl + "photos.getMessagesUploadServer";
            public static string ApiMessagesSend = ApiMethodUrl + "messages.send";
            public static string ApiSaveMessagesPhoto = ApiMethodUrl + "photos.saveMessagesPhoto";
            public static string ApiMessagesSetActivity = ApiMethodUrl + "messages.setActivity";
            public static string ApiAccountSetOnline = ApiMethodUrl + "account.setOnline";
            public static string ApiNotificationsGet = ApiMethodUrl + "notifications.get";
            public static string ApiMessagesGetDialogs = ApiMethodUrl + "messages.getDialogs";
            public static string ApiUsersGet = ApiMethodUrl + "users.get";
            public static string ApiGetShortLink = ApiMethodUrl + "utils.getShortLink";
        }
    }
}