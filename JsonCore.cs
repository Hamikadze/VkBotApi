using System.Collections.Generic;
using Newtonsoft.Json;

namespace VkBotApi
{
    public class JsonCore
    {
        public class Awsome
        {
            public class Quality
            {
                [JsonProperty("score")]
                public double Score { get; set; }
            }

            public class CoreResponse
            {
                [JsonProperty("quality")]
                public Quality Quality { get; set; }

                [JsonProperty("status")]
                public string Status { get; set; }
            }
        }

        internal class Wiki
        {
            internal class Normalized
            {
                [JsonProperty("from")]
                public string From { get; set; }

                [JsonProperty("to")]
                public string To { get; set; }
            }

            internal class Redirect
            {
                [JsonProperty("from")]
                public string From { get; set; }

                [JsonProperty("to")]
                public string To { get; set; }
            }

            internal class Id
            {
                [JsonProperty("pageid")]
                public int Pageid { get; set; }

                [JsonProperty("ns")]
                public int Ns { get; set; }

                [JsonProperty("title")]
                public string Title { get; set; }

                [JsonProperty("extract")]
                public string Extract { get; set; }
            }

            internal class Query
            {
                [JsonProperty("normalized")]
                public Normalized[] Normalized { get; set; }

                [JsonProperty("redirects")]
                public Redirect[] Redirects { get; set; }

                [JsonProperty("pages")]
                public IDictionary<string, Id> Pages { get; set; }
            }

            internal class Response
            {
                [JsonProperty("batchcomplete")]
                public string Batchcomplete { get; set; }

                [JsonProperty("query")]
                public Query Query { get; set; }
            }
        }

        internal class Joke
        {
            internal class JokeResponse
            {
                [JsonProperty("content")]
                public string Content { get; set; }
            }
        }

        internal class Google
        {
            internal class UrlIS
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("template")]
                public string Template { get; set; }
            }

            internal class RequestIS
            {
                [JsonProperty("title")]
                public string Title { get; set; }

                [JsonProperty("totalResults")]
                public string TotalResults { get; set; }

                [JsonProperty("searchTerms")]
                public string SearchTerms { get; set; }

                [JsonProperty("count")]
                public int Count { get; set; }

                [JsonProperty("startIndex")]
                public int StartIndex { get; set; }

                [JsonProperty("inputEncoding")]
                public string InputEncoding { get; set; }

                [JsonProperty("outputEncoding")]
                public string OutputEncoding { get; set; }

                [JsonProperty("safe")]
                public string Safe { get; set; }

                [JsonProperty("cx")]
                public string Cx { get; set; }

                [JsonProperty("searchType")]
                public string SearchType { get; set; }
            }

            internal class NextPageIS
            {
                [JsonProperty("title")]
                public string Title { get; set; }

                [JsonProperty("totalResults")]
                public string TotalResults { get; set; }

                [JsonProperty("searchTerms")]
                public string SearchTerms { get; set; }

                [JsonProperty("count")]
                public int Count { get; set; }

                [JsonProperty("startIndex")]
                public int StartIndex { get; set; }

                [JsonProperty("inputEncoding")]
                public string InputEncoding { get; set; }

                [JsonProperty("outputEncoding")]
                public string OutputEncoding { get; set; }

                [JsonProperty("safe")]
                public string Safe { get; set; }

                [JsonProperty("cx")]
                public string Cx { get; set; }

                [JsonProperty("searchType")]
                public string SearchType { get; set; }
            }

            internal class QueriesIS
            {
                [JsonProperty("request")]
                public RequestIS[] Request { get; set; }

                [JsonProperty("nextPage")]
                public NextPageIS[] NextPage { get; set; }
            }

            internal class ContextIS
            {
                [JsonProperty("title")]
                public string Title { get; set; }
            }

            internal class SearchInformationIS
            {
                [JsonProperty("searchTime")]
                public double SearchTime { get; set; }

                [JsonProperty("formattedSearchTime")]
                public string FormattedSearchTime { get; set; }

                [JsonProperty("totalResults")]
                public string TotalResults { get; set; }

                [JsonProperty("formattedTotalResults")]
                public string FormattedTotalResults { get; set; }
            }

            internal class ImageIS
            {
                [JsonProperty("contextLink")]
                public string ContextLink { get; set; }

                [JsonProperty("height")]
                public int Height { get; set; }

                [JsonProperty("width")]
                public int Width { get; set; }

                [JsonProperty("byteSize")]
                public int ByteSize { get; set; }

                [JsonProperty("thumbnailLink")]
                public string ThumbnailLink { get; set; }

                [JsonProperty("thumbnailHeight")]
                public int ThumbnailHeight { get; set; }

                [JsonProperty("thumbnailWidth")]
                public int ThumbnailWidth { get; set; }
            }

            internal class ItemIS
            {
                [JsonProperty("kind")]
                public string Kind { get; set; }

                [JsonProperty("title")]
                public string Title { get; set; }

                [JsonProperty("htmlTitle")]
                public string HtmlTitle { get; set; }

                [JsonProperty("link")]
                public string Link { get; set; }

                [JsonProperty("displayLink")]
                public string DisplayLink { get; set; }

                [JsonProperty("snippet")]
                public string Snippet { get; set; }

                [JsonProperty("htmlSnippet")]
                public string HtmlSnippet { get; set; }

                [JsonProperty("mime")]
                public string Mime { get; set; }

                [JsonProperty("image")]
                public ImageIS Image { get; set; }
            }

            internal class ImageSearchResponse
            {
                [JsonProperty("kind")]
                public string Kind { get; set; }

                [JsonProperty("url")]
                public UrlIS Url { get; set; }

                [JsonProperty("queries")]
                public QueriesIS Queries { get; set; }

                [JsonProperty("context")]
                public ContextIS Context { get; set; }

                [JsonProperty("searchInformation")]
                public SearchInformationIS SearchInformation { get; set; }

                [JsonProperty("items")]
                public ItemIS[] Items { get; set; }
            }
        }

        public class VK
        {
            public class MessageNew
            {
                public class Size
                {
                    [JsonProperty("type")]
                    public string Type { get; set; }

                    [JsonProperty("url")]
                    public string Url { get; set; }

                    [JsonProperty("width")]
                    public int Width { get; set; }

                    [JsonProperty("height")]
                    public int Height { get; set; }
                }

                public class Photo
                {
                    [JsonProperty("id")]
                    public int Id { get; set; }

                    [JsonProperty("album_id")]
                    public int Album_Id { get; set; }

                    [JsonProperty("owner_id")]
                    public int Owner_Id { get; set; }

                    [JsonProperty("sizes")]
                    public Size[] Sizes { get; set; }

                    [JsonProperty("text")]
                    public string Text { get; set; }

                    [JsonProperty("date")]
                    public int Date { get; set; }

                    [JsonProperty("access_key")]
                    public string Access_Key { get; set; }
                }

                public class Audio
                {
                    [JsonProperty("id")]
                    public int Id { get; set; }

                    [JsonProperty("owner_id")]
                    public int Owner_Id { get; set; }

                    [JsonProperty("artist")]
                    public string Artist { get; set; }

                    [JsonProperty("title")]
                    public string Title { get; set; }

                    [JsonProperty("duration")]
                    public int Duration { get; set; }

                    [JsonProperty("date")]
                    public int Date { get; set; }

                    [JsonProperty("url")]
                    public string Url { get; set; }

                    [JsonProperty("genre_id")]
                    public int Genre_Id { get; set; }

                    [JsonProperty("is_hq")]
                    public bool Is_Hq { get; set; }
                }

                public class Attachment
                {
                    [JsonProperty("type")]
                    public string Type { get; set; }

                    [JsonProperty("photo")]
                    public Photo Photo { get; set; }

                    [JsonProperty("audio")]
                    public Audio Audio { get; set; }

                    [JsonProperty("doc")]
                    public Doc Doc { get; set; }
                }

                public class FwdMessage
                {
                    [JsonProperty("date")]
                    public int Date { get; set; }

                    [JsonProperty("from_id")]
                    public int From_Id { get; set; }

                    [JsonProperty("text")]
                    public string Text { get; set; }

                    [JsonProperty("attachments")]
                    public Attachment[] Attachments { get; set; }

                    [JsonProperty("update_time")]
                    public int Update_Time { get; set; }
                }

                public class Object
                {
                    [JsonProperty("date")]
                    public int Date { get; set; }

                    [JsonProperty("from_id")]
                    public int From_Id { get; set; }

                    [JsonProperty("id")]
                    public int Id { get; set; }

                    [JsonProperty("out")]
                    public int Out { get; set; }

                    [JsonProperty("peer_id")]
                    public int Peer_Id { get; set; }

                    [JsonProperty("text")]
                    public string Text { get; set; }

                    [JsonProperty("conversation_message_id")]
                    public int Conversation_Message_Id { get; set; }

                    [JsonProperty("fwd_messages")]
                    public List<FwdMessage> Fwd_Messages { get; set; }

                    [JsonProperty("important")]
                    public bool Important { get; set; }

                    [JsonProperty("random_id")]
                    public int Random_Id { get; set; }

                    [JsonProperty("attachments")]
                    public List<Attachment> Attachments { get; set; }

                    [JsonProperty("is_hidden")]
                    public bool Is_Hidden { get; set; }
                }

                public class AudioMsg
                {
                    [JsonProperty("duration")]
                    public int Duration { get; set; }

                    [JsonProperty("waveform")]
                    public int[] Waveform { get; set; }

                    [JsonProperty("link_ogg")]
                    public string Link_Ogg { get; set; }

                    [JsonProperty("link_mp3")]
                    public string Link_Mp3 { get; set; }
                }

                public class Preview
                {
                    [JsonProperty("audio_msg")]
                    public AudioMsg Audio_Msg { get; set; }
                }

                public class Doc
                {
                    [JsonProperty("id")]
                    public int Id { get; set; }

                    [JsonProperty("owner_id")]
                    public int Owner_Id { get; set; }

                    [JsonProperty("title")]
                    public string Title { get; set; }

                    [JsonProperty("size")]
                    public int Size { get; set; }

                    [JsonProperty("ext")]
                    public string Ext { get; set; }

                    [JsonProperty("url")]
                    public string Url { get; set; }

                    [JsonProperty("date")]
                    public int Date { get; set; }

                    [JsonProperty("type")]
                    public int Type { get; set; }

                    [JsonProperty("preview")]
                    public Preview Preview { get; set; }

                    [JsonProperty("access_key")]
                    public string Access_Key { get; set; }
                }

                public class CoreResponse
                {
                    [JsonProperty("type")]
                    public string Type { get; set; }

                    [JsonProperty("object")]
                    public Object Object { get; set; }

                    [JsonProperty("group_id")]
                    public int Group_Id { get; set; }

                    [JsonProperty("error")]
                    public VK.Error Error { get; set; }
                }
            }

            internal class GetShortLink

            {
                public class Response
                {
                    [JsonProperty("short_url")]
                    public string ShortUrl { get; set; }

                    [JsonProperty("url")]
                    public string Url { get; set; }

                    [JsonProperty("key")]
                    public string Key { get; set; }

                    [JsonProperty("access_key")]
                    public string AccessKey { get; set; }
                }

                public class CoreResponse
                {
                    [JsonProperty("response")]
                    public Response Response { get; set; }

                    [JsonProperty("error")]
                    public Error Error { get; set; }
                }
            }

            internal class ResultGOPUSR
            {
                [JsonProperty("upload_url")]
                public string UploadUrl { get; set; }
            }

            internal class GetOwnerPhotoUploadServerResponse
            {
                [JsonProperty("response")]
                public ResultGOPUSR Response { get; set; }
            }

            internal class UploadPhotoInfoResponse
            {
                [JsonProperty("server")]
                public int Server { get; set; }

                [JsonProperty("photo")]
                public string Photo { get; set; }

                [JsonProperty("mid")]
                public int Mid { get; set; }

                [JsonProperty("hash")]
                public string Hash { get; set; }

                [JsonProperty("message_code")]
                public int MessageCode { get; set; }

                [JsonProperty("profile_aid")]
                public int ProfileAid { get; set; }
            }

            internal class ResultSOPR
            {
                [JsonProperty("photo_hash")]
                public string PhotoHash { get; set; }

                [JsonProperty("photo_src")]
                public string PhotoSrc { get; set; }

                [JsonProperty("photo_src_big")]
                public string PhotoSrcBig { get; set; }

                [JsonProperty("photo_src_small")]
                public string PhotoSrcSmall { get; set; }

                [JsonProperty("saved")]
                public int Saved { get; set; }

                [JsonProperty("post_id")]
                public int PostId { get; set; }
            }

            public class RequestParam
            {
                [JsonProperty("key")]
                public string Key { get; set; }

                [JsonProperty("value")]
                public string Value { get; set; }
            }

            public class Error
            {
                [JsonProperty("error_code")]
                public int ErrorCode { get; set; }

                [JsonProperty("error_msg")]
                public string ErrorMsg { get; set; }

                [JsonProperty("request_params")]
                public RequestParam[] RequestParams { get; set; }

                [JsonProperty("captcha_sid")]
                public string CaptchaSid { get; set; }

                [JsonProperty("captcha_img")]
                public string CaptchaImg { get; set; }
            }

            internal class SaveOwnerPhotoResponse
            {
                [JsonProperty("error")]
                public Error Error { get; set; }

                [JsonProperty("response")]
                public ResultSOPR Response { get; set; }
            }

            internal class ItemGP
            {
                [JsonProperty("id")]
                public int Id { get; set; }

                [JsonProperty("album_id")]
                public int AlbumId { get; set; }

                [JsonProperty("owner_id")]
                public int OwnerId { get; set; }

                [JsonProperty("photo_75")]
                public string Photo75 { get; set; }

                [JsonProperty("photo_130")]
                public string Photo130 { get; set; }

                [JsonProperty("photo_604")]
                public string Photo604 { get; set; }

                [JsonProperty("photo_807")]
                public string Photo807 { get; set; }

                [JsonProperty("width")]
                public int Width { get; set; }

                [JsonProperty("height")]
                public int Height { get; set; }

                [JsonProperty("text")]
                public string Text { get; set; }

                [JsonProperty("date")]
                public int Date { get; set; }

                [JsonProperty("post_id")]
                public int PostId { get; set; }
            }

            internal class ResultGP
            {
                [JsonProperty("count")]
                public int Count { get; set; }

                [JsonProperty("items")]
                public ItemGP[] Items { get; set; }
            }

            internal class GetPhotosResponse
            {
                [JsonProperty("response")]
                public ResultGP Response { get; set; }

                [JsonProperty("error")]
                public Error Error { get; set; }
            }

            internal class Photo
            {
                [JsonProperty("id")]
                public int Id { get; set; }

                [JsonProperty("album_id")]
                public int AlbumId { get; set; }

                [JsonProperty("owner_id")]
                public int OwnerId { get; set; }

                [JsonProperty("photo_75")]
                public string Photo75 { get; set; }

                [JsonProperty("photo_130")]
                public string Photo130 { get; set; }

                [JsonProperty("photo_604")]
                public string Photo604 { get; set; }

                [JsonProperty("photo_807")]
                public string Photo807 { get; set; }

                [JsonProperty("photo_1280")]
                public string Photo1280 { get; set; }

                [JsonProperty("width")]
                public int Width { get; set; }

                [JsonProperty("height")]
                public int Height { get; set; }

                [JsonProperty("text")]
                public string Text { get; set; }

                [JsonProperty("date")]
                public int Date { get; set; }

                [JsonProperty("access_key")]
                public string AccessKey { get; set; }

                [JsonProperty("lat")]
                public double? Lat { get; set; }

                [JsonProperty("long")]
                public double? Long { get; set; }

                [JsonProperty("post_id")]
                public int PostId { get; set; }
            }

            internal class Video
            {
                [JsonProperty("id")]
                public int Id { get; set; }

                [JsonProperty("owner_id")]
                public int OwnerId { get; set; }

                [JsonProperty("title")]
                public string Title { get; set; }

                [JsonProperty("duration")]
                public int Duration { get; set; }

                [JsonProperty("description")]
                public string Description { get; set; }

                [JsonProperty("date")]
                public int Date { get; set; }

                [JsonProperty("views")]
                public int Views { get; set; }

                [JsonProperty("comments")]
                public int Comments { get; set; }

                [JsonProperty("width")]
                public int Width { get; set; }

                [JsonProperty("height")]
                public int Height { get; set; }

                [JsonProperty("photo_130")]
                public string Photo130 { get; set; }

                [JsonProperty("photo_320")]
                public string Photo320 { get; set; }

                [JsonProperty("photo_800")]
                public string Photo800 { get; set; }

                [JsonProperty("access_key")]
                public string AccessKey { get; set; }

                [JsonProperty("can_add")]
                public int CanAdd { get; set; }

                [JsonProperty("is_private")]
                public int IsPrivate { get; set; }

                [JsonProperty("platform")]
                public string Platform { get; set; }

                [JsonProperty("can_edit")]
                public int CanEdit { get; set; }
            }

            internal class GetConversations
            {
                public class Peer
                {
                    [JsonProperty("id")]
                    public int Id { get; set; }

                    [JsonProperty("type")]
                    public string Type { get; set; }

                    [JsonProperty("local_id")]
                    public int LocalId { get; set; }
                }

                public class CanWrite
                {
                    [JsonProperty("allowed")]
                    public bool Allowed { get; set; }

                    [JsonProperty("reason")]
                    public int? Reason { get; set; }
                }

                public class Photo
                {
                    [JsonProperty("photo_50")]
                    public string Photo50 { get; set; }

                    [JsonProperty("photo_100")]
                    public string Photo100 { get; set; }

                    [JsonProperty("photo_200")]
                    public string Photo200 { get; set; }

                    [JsonProperty("id")]
                    public int Id { get; set; }

                    [JsonProperty("album_id")]
                    public int AlbumId { get; set; }

                    [JsonProperty("owner_id")]
                    public int OwnerId { get; set; }

                    [JsonProperty("sizes")]
                    public Size[] Sizes { get; set; }

                    [JsonProperty("text")]
                    public string Text { get; set; }

                    [JsonProperty("date")]
                    public int Date { get; set; }

                    [JsonProperty("access_key")]
                    public string AccessKey { get; set; }
                }

                public class Link
                {
                    [JsonProperty("url")]
                    public string Url { get; set; }

                    [JsonProperty("title")]
                    public string Title { get; set; }

                    [JsonProperty("caption")]
                    public string Caption { get; set; }

                    [JsonProperty("description")]
                    public string Description { get; set; }
                }

                public class Attachment
                {
                    [JsonProperty("type")]
                    public string Type { get; set; }

                    [JsonProperty("link")]
                    public Link Link { get; set; }

                    [JsonProperty("video")]
                    public Video Video { get; set; }

                    [JsonProperty("wall")]
                    public Wall Wall { get; set; }

                    [JsonProperty("photo")]
                    public Photo Photo { get; set; }

                    [JsonProperty("doc")]
                    public Doc Doc { get; set; }

                    [JsonProperty("sticker")]
                    public Sticker Sticker { get; set; }
                }

                public class PinnedMessage
                {
                    [JsonProperty("id")]
                    public int Id { get; set; }

                    [JsonProperty("date")]
                    public int Date { get; set; }

                    [JsonProperty("from_id")]
                    public int FromId { get; set; }

                    [JsonProperty("peer_id")]
                    public int PeerId { get; set; }

                    [JsonProperty("text")]
                    public string Text { get; set; }

                    [JsonProperty("conversation_message_id")]
                    public int ConversationMessageId { get; set; }

                    [JsonProperty("attachments")]
                    public Attachment[] Attachments { get; set; }

                    [JsonProperty("fwd_messages")]
                    public object[] FwdMessages { get; set; }
                }

                public class ChatSettings
                {
                    [JsonProperty("title")]
                    public string Title { get; set; }

                    [JsonProperty("members_count")]
                    public int MembersCount { get; set; }

                    [JsonProperty("state")]
                    public string State { get; set; }

                    [JsonProperty("photo")]
                    public Photo Photo { get; set; }

                    [JsonProperty("active_ids")]
                    public int[] ActiveIds { get; set; }

                    [JsonProperty("pinned_message")]
                    public PinnedMessage PinnedMessage { get; set; }
                }

                public class Conversation
                {
                    [JsonProperty("peer")]
                    public Peer Peer { get; set; }

                    [JsonProperty("in_read")]
                    public int InRead { get; set; }

                    [JsonProperty("out_read")]
                    public int OutRead { get; set; }

                    [JsonProperty("last_message_id")]
                    public int LastMessageId { get; set; }

                    [JsonProperty("unread_count")]
                    public int UnreadCount { get; set; }

                    [JsonProperty("can_write")]
                    public CanWrite CanWrite { get; set; }

                    [JsonProperty("chat_settings")]
                    public ChatSettings ChatSettings { get; set; }
                }

                public class Size
                {
                    [JsonProperty("type")]
                    public string Type { get; set; }

                    [JsonProperty("url")]
                    public string Url { get; set; }

                    [JsonProperty("width")]
                    public int Width { get; set; }

                    [JsonProperty("height")]
                    public int Height { get; set; }
                }

                public class AudioMsg
                {
                    [JsonProperty("duration")]
                    public int Duration { get; set; }

                    [JsonProperty("waveform")]
                    public int[] Waveform { get; set; }

                    [JsonProperty("link_ogg")]
                    public string LinkOgg { get; set; }

                    [JsonProperty("link_mp3")]
                    public string LinkMp3 { get; set; }
                }

                public class Preview
                {
                    [JsonProperty("audio_msg")]
                    public AudioMsg AudioMsg { get; set; }
                }

                public class Doc
                {
                    [JsonProperty("id")]
                    public int Id { get; set; }

                    [JsonProperty("owner_id")]
                    public int OwnerId { get; set; }

                    [JsonProperty("title")]
                    public string Title { get; set; }

                    [JsonProperty("size")]
                    public int Size { get; set; }

                    [JsonProperty("ext")]
                    public string Ext { get; set; }

                    [JsonProperty("url")]
                    public string Url { get; set; }

                    [JsonProperty("date")]
                    public int Date { get; set; }

                    [JsonProperty("type")]
                    public int Type { get; set; }

                    [JsonProperty("preview")]
                    public Preview Preview { get; set; }

                    [JsonProperty("access_key")]
                    public string AccessKey { get; set; }
                }

                public class FwdMessage
                {
                    [JsonProperty("date")]
                    public int Date { get; set; }

                    [JsonProperty("from_id")]
                    public int FromId { get; set; }

                    [JsonProperty("text")]
                    public string Text { get; set; }

                    [JsonProperty("attachments")]
                    public List<Attachment> Attachments { get; set; }

                    [JsonProperty("update_time")]
                    public int UpdateTime { get; set; }

                    [JsonProperty("fwd_messages")]
                    public FwdMessage[] FwdMessages { get; set; }
                }

                public class Video
                {
                    [JsonProperty("id")]
                    public int Id { get; set; }

                    [JsonProperty("owner_id")]
                    public int OwnerId { get; set; }

                    [JsonProperty("title")]
                    public string Title { get; set; }

                    [JsonProperty("duration")]
                    public int Duration { get; set; }

                    [JsonProperty("description")]
                    public string Description { get; set; }

                    [JsonProperty("date")]
                    public int Date { get; set; }

                    [JsonProperty("comments")]
                    public int Comments { get; set; }

                    [JsonProperty("views")]
                    public int Views { get; set; }

                    [JsonProperty("width")]
                    public int Width { get; set; }

                    [JsonProperty("height")]
                    public int Height { get; set; }

                    [JsonProperty("photo_130")]
                    public string Photo130 { get; set; }

                    [JsonProperty("photo_320")]
                    public string Photo320 { get; set; }

                    [JsonProperty("photo_800")]
                    public string Photo800 { get; set; }

                    [JsonProperty("access_key")]
                    public string AccessKey { get; set; }

                    [JsonProperty("first_frame_320")]
                    public string FirstFrame320 { get; set; }

                    [JsonProperty("first_frame_160")]
                    public string FirstFrame160 { get; set; }

                    [JsonProperty("first_frame_130")]
                    public string FirstFrame130 { get; set; }

                    [JsonProperty("first_frame_800")]
                    public string FirstFrame800 { get; set; }

                    [JsonProperty("can_add")]
                    public int CanAdd { get; set; }
                }

                public class PostSource
                {
                    [JsonProperty("type")]
                    public string Type { get; set; }
                }

                public class Comments
                {
                    [JsonProperty("count")]
                    public int Count { get; set; }

                    [JsonProperty("can_post")]
                    public int CanPost { get; set; }

                    [JsonProperty("groups_can_post")]
                    public bool GroupsCanPost { get; set; }
                }

                public class Likes
                {
                    [JsonProperty("count")]
                    public int Count { get; set; }

                    [JsonProperty("user_likes")]
                    public int UserLikes { get; set; }

                    [JsonProperty("can_like")]
                    public int CanLike { get; set; }

                    [JsonProperty("can_publish")]
                    public int CanPublish { get; set; }
                }

                public class Reposts
                {
                    [JsonProperty("count")]
                    public int Count { get; set; }

                    [JsonProperty("user_reposted")]
                    public int UserReposted { get; set; }
                }

                public class Views
                {
                    [JsonProperty("count")]
                    public int Count { get; set; }
                }

                public class Wall
                {
                    [JsonProperty("id")]
                    public int Id { get; set; }

                    [JsonProperty("from_id")]
                    public int FromId { get; set; }

                    [JsonProperty("to_id")]
                    public int ToId { get; set; }

                    [JsonProperty("date")]
                    public int Date { get; set; }

                    [JsonProperty("post_type")]
                    public string PostType { get; set; }

                    [JsonProperty("text")]
                    public string Text { get; set; }

                    [JsonProperty("marked_as_ads")]
                    public int MarkedAsAds { get; set; }

                    [JsonProperty("attachments")]
                    public Attachment[] Attachments { get; set; }

                    [JsonProperty("post_source")]
                    public PostSource PostSource { get; set; }

                    [JsonProperty("comments")]
                    public Comments Comments { get; set; }

                    [JsonProperty("likes")]
                    public Likes Likes { get; set; }

                    [JsonProperty("reposts")]
                    public Reposts Reposts { get; set; }

                    [JsonProperty("views")]
                    public Views Views { get; set; }

                    [JsonProperty("access_key")]
                    public string AccessKey { get; set; }
                }

                public class Image
                {
                    [JsonProperty("url")]
                    public string Url { get; set; }

                    [JsonProperty("width")]
                    public int Width { get; set; }

                    [JsonProperty("height")]
                    public int Height { get; set; }
                }

                public class ImagesWithBackground
                {
                    [JsonProperty("url")]
                    public string Url { get; set; }

                    [JsonProperty("width")]
                    public int Width { get; set; }

                    [JsonProperty("height")]
                    public int Height { get; set; }
                }

                public class Sticker
                {
                    [JsonProperty("product_id")]
                    public int ProductId { get; set; }

                    [JsonProperty("sticker_id")]
                    public int StickerId { get; set; }

                    [JsonProperty("images")]
                    public Image[] Images { get; set; }

                    [JsonProperty("images_with_background")]
                    public ImagesWithBackground[] ImagesWithBackground { get; set; }
                }

                public class Action
                {
                    [JsonProperty("type")]
                    public string Type { get; set; }

                    [JsonProperty("member_id")]
                    public int MemberId { get; set; }
                }

                public class LastMessage
                {
                    [JsonProperty("date")]
                    public int Date { get; set; }

                    [JsonProperty("from_id")]
                    public int FromId { get; set; }

                    [JsonProperty("id")]
                    public int Id { get; set; }

                    [JsonProperty("out")]
                    public int Out { get; set; }

                    [JsonProperty("peer_id")]
                    public int PeerId { get; set; }

                    [JsonProperty("text")]
                    public string Text { get; set; }

                    [JsonProperty("conversation_message_id")]
                    public int ConversationMessageId { get; set; }

                    [JsonProperty("fwd_messages")]
                    public List<FwdMessage> FwdMessages { get; set; }

                    [JsonProperty("important")]
                    public bool Important { get; set; }

                    [JsonProperty("random_id")]
                    public int RandomId { get; set; }

                    [JsonProperty("attachments")]
                    public List<Attachment> Attachments { get; set; }

                    [JsonProperty("is_hidden")]
                    public bool IsHidden { get; set; }

                    [JsonProperty("update_time")]
                    public int? UpdateTime { get; set; }

                    [JsonProperty("action")]
                    public Action Action { get; set; }

                    [JsonProperty("payload")]
                    public string Payload { get; set; }
                }

                public class Item
                {
                    [JsonProperty("conversation")]
                    public Conversation Conversation { get; set; }

                    [JsonProperty("last_message")]
                    public LastMessage LastMessage { get; set; }
                }

                public class Profile
                {
                    [JsonProperty("id")]
                    public int Id { get; set; }

                    [JsonProperty("first_name")]
                    public string FirstName { get; set; }

                    [JsonProperty("last_name")]
                    public string LastName { get; set; }

                    [JsonProperty("sex")]
                    public int Sex { get; set; }

                    [JsonProperty("screen_name")]
                    public string ScreenName { get; set; }

                    [JsonProperty("photo_50")]
                    public string Photo50 { get; set; }

                    [JsonProperty("photo_100")]
                    public string Photo100 { get; set; }

                    [JsonProperty("online")]
                    public int Online { get; set; }

                    [JsonProperty("online_mobile")]
                    public int OnlineMobile { get; set; }

                    [JsonProperty("online_app")]
                    public string OnlineApp { get; set; }

                    [JsonProperty("deactivated")]
                    public string Deactivated { get; set; }
                }

                public class Response
                {
                    [JsonProperty("count")]
                    public int Count { get; set; }

                    [JsonProperty("items")]
                    public List<Item> Items { get; set; }

                    [JsonProperty("unread_count")]
                    public int UnreadCount { get; set; }

                    [JsonProperty("profiles")]
                    public Profile[] Profiles { get; set; }
                }

                public class CoreResponse
                {
                    [JsonProperty("error")]
                    public Error Error { get; set; }

                    [JsonProperty("response")]
                    public Response Response { get; set; }
                }
            }

            internal class ResponseMUS
            {
                [JsonProperty("upload_url")]
                public string UploadUrl { get; set; }

                [JsonProperty("album_id")]
                public int AlbumId { get; set; }

                [JsonProperty("user_id")]
                public int UserId { get; set; }
            }

            internal class MessagesUploadServerResponse
            {
                [JsonProperty("response")]
                public ResponseMUS Response { get; set; }

                [JsonProperty("error")]
                public Error Error { get; set; }
            }

            internal class PhotoInfoResponse
            {
                [JsonProperty("photo")]
                public string Photo { get; set; }

                [JsonProperty("sizes")]
                public object[][] Sizes { get; set; }

                [JsonProperty("kid")]
                public string Kid { get; set; }

                [JsonProperty("debug")]
                public string Debug { get; set; }
            }

            internal class ResponseSMP
            {
                [JsonProperty("id")]
                public int Id { get; set; }

                [JsonProperty("album_id")]
                public int AlbumId { get; set; }

                [JsonProperty("owner_id")]
                public int OwnerId { get; set; }

                [JsonProperty("photo_75")]
                public string Photo75 { get; set; }

                [JsonProperty("photo_130")]
                public string Photo130 { get; set; }

                [JsonProperty("photo_604")]
                public string Photo604 { get; set; }

                [JsonProperty("photo_807")]
                public string Photo807 { get; set; }

                [JsonProperty("photo_1280")]
                public string Photo1280 { get; set; }

                [JsonProperty("width")]
                public int Width { get; set; }

                [JsonProperty("height")]
                public int Height { get; set; }

                [JsonProperty("text")]
                public string Text { get; set; }

                [JsonProperty("date")]
                public int Date { get; set; }
            }

            internal class SaveMessagePhotoResponse
            {
                [JsonProperty("response")]
                public ResponseSMP[] Response { get; set; }

                [JsonProperty("error")]
                public Error Error { get; set; }
            }

            internal class ItemAS
            {
                [JsonProperty("id")]
                public int Id { get; set; }

                [JsonProperty("owner_id")]
                public int OwnerId { get; set; }

                [JsonProperty("artist")]
                public string Artist { get; set; }

                [JsonProperty("title")]
                public string Title { get; set; }

                [JsonProperty("duration")]
                public int Duration { get; set; }

                [JsonProperty("date")]
                public int Date { get; set; }

                [JsonProperty("url")]
                public string Url { get; set; }

                [JsonProperty("lyrics_id")]
                public int LyricsId { get; set; }

                [JsonProperty("genre_id")]
                public int GenreId { get; set; }
            }

            internal class ResponseAS
            {
                [JsonProperty("count")]
                public int Count { get; set; }

                [JsonProperty("items")]
                public ItemAS[] Items { get; set; }
            }

            internal class AudioSearchResponse
            {
                [JsonProperty("response")]
                public ResponseAS Response { get; set; }
            }

            internal class AttachmentGN
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("video")]
                public Video Video { get; set; }

                [JsonProperty("photo")]
                public Photo Photo { get; set; }
            }

            internal class PostSourceGN
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("data")]
                public string Data { get; set; }

                [JsonProperty("platform")]
                public string Platform { get; set; }
            }

            internal class CommentsGN
            {
                [JsonProperty("count")]
                public int Count { get; set; }

                [JsonProperty("can_post")]
                public int CanPost { get; set; }
            }

            internal class LikesGN
            {
                [JsonProperty("count")]
                public int Count { get; set; }

                [JsonProperty("user_likes")]
                public int UserLikes { get; set; }

                [JsonProperty("can_like")]
                public int CanLike { get; set; }

                [JsonProperty("can_publish")]
                public int CanPublish { get; set; }
            }

            internal class ParentGN
            {
                [JsonProperty("id")]
                public int Id { get; set; }

                [JsonProperty("from_id")]
                public int FromId { get; set; }

                [JsonProperty("to_id")]
                public int ToId { get; set; }

                [JsonProperty("date")]
                public int Date { get; set; }

                [JsonProperty("post_type")]
                public string PostType { get; set; }

                [JsonProperty("text")]
                public string Text { get; set; }

                [JsonProperty("can_delete")]
                public int CanDelete { get; set; }

                [JsonProperty("attachments")]
                public AttachmentGN[] Attachments { get; set; }

                [JsonProperty("post_source")]
                public PostSourceGN PostSource { get; set; }

                [JsonProperty("comments")]
                public CommentsGN Comments { get; set; }

                [JsonProperty("likes")]
                public LikesGN Likes { get; set; }

                [JsonProperty("album_id")]
                public int? AlbumId { get; set; }

                [JsonProperty("owner_id")]
                public int? OwnerId { get; set; }

                [JsonProperty("photo_75")]
                public string Photo75 { get; set; }

                [JsonProperty("photo_130")]
                public string Photo130 { get; set; }

                [JsonProperty("photo_604")]
                public string Photo604 { get; set; }

                [JsonProperty("width")]
                public int? Width { get; set; }

                [JsonProperty("height")]
                public int? Height { get; set; }

                [JsonProperty("post_id")]
                public int? PostId { get; set; }

                [JsonProperty("can_comment")]
                public int? CanComment { get; set; }

                [JsonProperty("photo_807")]
                public string Photo807 { get; set; }

                [JsonProperty("photo_1280")]
                public string Photo1280 { get; set; }

                [JsonProperty("photo_2560")]
                public string Photo2560 { get; set; }

                [JsonProperty("lat")]
                public double? Lat { get; set; }

                [JsonProperty("long")]
                public double? Long { get; set; }

                [JsonProperty("reply_to_user")]
                public int? ReplyToUser { get; set; }

                [JsonProperty("reply_to_comment")]
                public int? ReplyToComment { get; set; }

                [JsonProperty("photo")]
                public Photo Photo { get; set; }
            }

            internal class ItemsGN
            {
                [JsonProperty("from_id")]
                public int FromId { get; set; }
            }

            internal class FeedbackGN
            {
                [JsonProperty("count")]
                public int Count { get; set; }

                [JsonProperty("items")]
                public ItemsGN[] Items { get; set; }

                [JsonProperty("id")]
                public int? Id { get; set; }

                [JsonProperty("to_id")]
                public int? ToId { get; set; }

                [JsonProperty("from_id")]
                public object FromId { get; set; }

                [JsonProperty("post_type")]
                public string PostType { get; set; }

                [JsonProperty("text")]
                public string Text { get; set; }

                [JsonProperty("can_delete")]
                public int? CanDelete { get; set; }

                [JsonProperty("attachments")]
                public AttachmentGN[] Attachments { get; set; }

                [JsonProperty("post_source")]
                public PostSourceGN PostSource { get; set; }

                [JsonProperty("comments")]
                public CommentsGN Comments { get; set; }

                [JsonProperty("likes")]
                public LikesGN Likes { get; set; }

                [JsonProperty("date")]
                public int? Date { get; set; }

                [JsonProperty("reply_to_user")]
                public int? ReplyToUser { get; set; }

                [JsonProperty("reply_to_comment")]
                public int? ReplyToComment { get; set; }
            }

            internal class ReplyGN
            {
                [JsonProperty("id")]
                public int Id { get; set; }

                [JsonProperty("date")]
                public string Date { get; set; }

                [JsonProperty("text")]
                public string Text { get; set; }
            }

            internal class ItemGN
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("date")]
                public int Date { get; set; }

                [JsonProperty("parent")]
                public ParentGN Parent { get; set; }

                [JsonProperty("feedback")]
                public FeedbackGN Feedback { get; set; }

                [JsonProperty("reply")]
                public ReplyGN Reply { get; set; }
            }

            internal class ProfileGN
            {
                [JsonProperty("id")]
                public int Id { get; set; }

                [JsonProperty("first_name")]
                public string FirstName { get; set; }

                [JsonProperty("last_name")]
                public string LastName { get; set; }

                [JsonProperty("sex")]
                public int Sex { get; set; }

                [JsonProperty("screen_name")]
                public string ScreenName { get; set; }

                [JsonProperty("photo_50")]
                public string Photo50 { get; set; }

                [JsonProperty("photo_100")]
                public string Photo100 { get; set; }

                [JsonProperty("online")]
                public int Online { get; set; }

                [JsonProperty("online_app")]
                public string OnlineApp { get; set; }

                [JsonProperty("online_mobile")]
                public int? OnlineMobile { get; set; }
            }

            internal class ResponseGN
            {
                [JsonProperty("count")]
                public int Count { get; set; }

                [JsonProperty("items")]
                public ItemGN[] Items { get; set; }

                [JsonProperty("profiles")]
                public ProfileGN[] Profiles { get; set; }

                [JsonProperty("groups")]
                public object[] Groups { get; set; }

                [JsonProperty("last_viewed")]
                public int LastViewed { get; set; }
            }

            internal class GetNotificationsResponse
            {
                [JsonProperty("response")]
                public ResponseGN Response { get; set; }
            }

            internal class GetDialogsResponse
            {
                [JsonProperty("response")]
                public ResponseGD Response { get; set; }
            }

            internal class AttachmentGD
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("photo")]
                public Photo Photo { get; set; }
            }

            internal class FwdMessageGD
            {
                [JsonProperty("user_id")]
                public int UserId { get; set; }

                [JsonProperty("date")]
                public int Date { get; set; }

                [JsonProperty("body")]
                public string Body { get; set; }

                [JsonProperty("attachments")]
                public AttachmentGD[] Attachments { get; set; }
            }

            internal class MessageGD
            {
                [JsonProperty("id")]
                public int Id { get; set; }

                [JsonProperty("date")]
                public int Date { get; set; }

                [JsonProperty("out")]
                public int Out { get; set; }

                [JsonProperty("user_id")]
                public int UserId { get; set; }

                [JsonProperty("read_state")]
                public int ReadState { get; set; }

                [JsonProperty("title")]
                public string Title { get; set; }

                [JsonProperty("body")]
                public string Body { get; set; }

                [JsonProperty("chat_id")]
                public int ChatId { get; set; }

                [JsonProperty("chat_active")]
                public int[] ChatActive { get; set; }

                [JsonProperty("users_count")]
                public int UsersCount { get; set; }

                [JsonProperty("admin_id")]
                public int AdminId { get; set; }

                [JsonProperty("photo_50")]
                public string Photo50 { get; set; }

                [JsonProperty("photo_100")]
                public string Photo100 { get; set; }

                [JsonProperty("photo_200")]
                public string Photo200 { get; set; }

                [JsonProperty("random_id")]
                public int? RandomId { get; set; }

                [JsonProperty("attachments")]
                public AttachmentGD[] Attachments { get; set; }

                [JsonProperty("fwd_messages")]
                public FwdMessageGD[] FwdMessages { get; set; }

                [JsonProperty("action")]
                public string Action { get; set; }

                [JsonProperty("action_mid")]
                public int? ActionMid { get; set; }
            }

            internal class ItemGD
            {
                [JsonProperty("unread")]
                public int Unread { get; set; }

                [JsonProperty("message")]
                public MessageGD Message { get; set; }

                [JsonProperty("in_read")]
                public int InRead { get; set; }

                [JsonProperty("out_read")]
                public int OutRead { get; set; }
            }

            internal class ResponseGD
            {
                [JsonProperty("count")]
                public int Count { get; set; }

                [JsonProperty("unread_dialogs")]
                public int UnreadDialogs { get; set; }

                [JsonProperty("items")]
                public ItemGD[] Items { get; set; }
            }

            internal class ResponseUG
            {
                [JsonProperty("id")]
                public int Id { get; set; }

                [JsonProperty("first_name")]
                public string FirstName { get; set; }

                [JsonProperty("last_name")]
                public string LastName { get; set; }

                [JsonProperty("can_write_private_message")]
                public int CanWritePrivateMessage { get; set; }

                [JsonProperty("blacklisted")]
                public int Blacklisted { get; set; }
            }

            internal class UGRequestParam
            {
                [JsonProperty("key")]
                public string Key { get; set; }

                [JsonProperty("value")]
                public string Value { get; set; }
            }

            internal class UGError
            {
                [JsonProperty("error_code")]
                public int ErrorCode { get; set; }

                [JsonProperty("error_msg")]
                public string ErrorMsg { get; set; }

                [JsonProperty("request_params")]
                public UGRequestParam[] RequestParams { get; set; }
            }

            internal class UsersGetResponse
            {
                [JsonProperty("response")]
                public ResponseUG[] Response { get; set; }

                [JsonProperty("error")]
                public UGError Error { get; set; }
            }
        }
    }
}