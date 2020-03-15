using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace VkBotApi
{
    internal class SiteUtils
    {
        public static List<JsonCore.VK.MessageGD> GetDialogs(string accessToken)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                RequestParams requestParams = new RequestParams
                {
                    ["owner_id"] = DataStorage.VK_UserID,
                    ["count"] = 100,
                    ["access_token"] = accessToken,
                    ["v"] = SiteData.VK.ApiVer,
                };
                var response =
                    httpClient.GetStringAsync(SiteData.VK.ApiMessagesGetDialogs.ConvertRequestParams(requestParams))
                        .Result;
                var desResponse = JsonConvert.DeserializeObject<JsonCore.VK.GetDialogsResponse>(response);
                return desResponse.Response.Items.Where(i => i.Message.ChatId != 0).Select(i => i.Message).ToList();
            }
            catch (Exception ex)
            {
                ex.Error();
                return null;
            }
        }

        public static string GetPhotoUploadServerUrl(string accessToken)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                RequestParams requestParams = new RequestParams
                {
                    ["owner_id"] = DataStorage.VK_UserID,
                    ["access_token"] = accessToken,
                    ["v"] = SiteData.VK.ApiVer,
                };
                var response =
                    httpClient.GetStringAsync(SiteData.VK.ApiPhotoUploadServer.ConvertRequestParams(requestParams))
                        .Result;
                var desResponse = JsonConvert.DeserializeObject<JsonCore.VK.GetOwnerPhotoUploadServerResponse>(response);
                return desResponse.Response.UploadUrl;
            }
            catch (Exception ex)
            {
                ex.Error();
                return null;
            }
        }

        public static JsonCore.VK.UploadPhotoInfoResponse UploadPhotoToServerAsync(string url, Image<Rgba32> image)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                RequestParams requestParams = new RequestParams
                {
                    ["_square_crop"] = $"0,0,{image.Height}"
                };
                using var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
                using MemoryStream ms = new MemoryStream();
                image.Save(ms, new PngEncoder());
                ms.Position = 0;
                content.Add(new StreamContent(ms), "photo",
                    "image.jpg");
                content.Add(new StringContent(requestParams[0]));
                using var message = httpClient.PostAsync(url, content).Result;
                var response = message.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<JsonCore.VK.UploadPhotoInfoResponse>(response);
            }
            catch (Exception ex)
            {
                ex.Error();
                return null;
            }
        }

        public static bool SaveOwnerPhoto(string accessToken, JsonCore.VK.UploadPhotoInfoResponse photoInfo)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                do
                {
                    RequestParams requestParams = new RequestParams
                    {
                        ["server"] = photoInfo.Server,
                        ["hash"] = photoInfo.Hash,
                        ["photo"] = photoInfo.Photo,
                        ["access_token"] = accessToken,
                        ["v"] = SiteData.VK.ApiVer,
                    };
                    var response = httpClient.GetStringAsync(SiteData.VK.ApiPhotosSaveOwnerPhoto.ConvertRequestParams(requestParams)).Result;
                    var desResponse = JsonConvert.DeserializeObject<JsonCore.VK.SaveOwnerPhotoResponse>(response);
                    if (desResponse.Error?.ErrorCode == 14)
                    {
                        LogCore.Log(desResponse.Error.ErrorMsg, response, EnumData.LogTypeCommand.Error, EnumData.LogSourceCommand.VK);
                        continue;
                    }
                    DeletePost(accessToken, desResponse.Response.PostId);
                    return desResponse.Response.Saved == 1;
                } while (true);
            }
            catch (Exception ex)
            {
                ex.Error();
                return false;
            }
        }

        public static string SaveMessagePhoto(string accessToken, JsonCore.VK.UploadPhotoInfoResponse photoInfo)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                do
                {
                    RequestParams requestParams = new RequestParams
                    {
                        ["server"] = photoInfo.Server,
                        ["hash"] = photoInfo.Hash,
                        ["photo"] = photoInfo.Photo,
                        ["access_token"] = accessToken,
                        ["v"] = SiteData.VK.ApiVer,
                    };
                    var response =
                        httpClient.GetStringAsync(SiteData.VK.ApiSaveMessagesPhoto.ConvertRequestParams(requestParams)).Result;
                    var desResponse = JsonConvert.DeserializeObject<JsonCore.VK.SaveMessagePhotoResponse>(response);
                    if (desResponse.Error?.ErrorCode == 14)
                    {
                        LogCore.Log(desResponse.Error.ErrorMsg, response, EnumData.LogTypeCommand.Error, EnumData.LogSourceCommand.VK);
                        continue;
                    }
                    return $"{desResponse.Response[0].OwnerId}_{desResponse.Response[0].Id}";
                } while (true);
            }
            catch (Exception ex)
            {
                ex.Error();
                return string.Empty;
            }
        }

        public static bool DeletePost(string accessToken, int postId)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                RequestParams requestParams = new RequestParams
                {
                    ["owner_id"] = DataStorage.VK_UserID,
                    ["post_id"] = postId,
                    ["access_token"] = accessToken,
                    ["v"] = SiteData.VK.ApiVer,
                };
                var response = httpClient.GetStringAsync(SiteData.VK.ApiWallDelete.ConvertRequestParams(requestParams)).Result;
                if (string.Equals(response, "{\"response\":1}", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else
                {
                    throw new Exception(response);
                }
            }
            catch (Exception ex)
            {
                ex.Error();
                return false;
            }
        }

        public static IEnumerable<int> GetPhotos(string accessToken, string albumId = "profile")
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                RequestParams requestParams = new RequestParams
                {
                    ["owner_id"] = DataStorage.VK_UserID,
                    ["album_id"] = albumId,
                    ["rev"] = 0,
                    ["count"] = 100,
                    ["access_token"] = accessToken,
                    ["v"] = SiteData.VK.ApiVer,
                };
                var response = httpClient.GetStringAsync(SiteData.VK.ApiPhotosGet.ConvertRequestParams(requestParams)).Result;
                var desResponse = JsonConvert.DeserializeObject<JsonCore.VK.GetPhotosResponse>(response);
                if (desResponse.Response == null)
                    throw new Exception(response);
                desResponse.Response.Items = desResponse.Response.Items.OrderByDescending(i => i.Date).ToArray();
                return desResponse.Response.Count > 1 ? desResponse.Response.Items.Select(i => i.Id) : null;
            }
            catch (Exception ex)
            {
                ex.Error();
                return null;
            }
        }

        public static bool DeletePhoto(string accessToken, int photoId)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                RequestParams requestParams = new RequestParams
                {
                    ["owner_id"] = DataStorage.VK_UserID,
                    ["photo_id"] = photoId,
                    ["access_token"] = accessToken,
                    ["v"] = SiteData.VK.ApiVer,
                };
                var response = httpClient.GetStringAsync(SiteData.VK.ApiPhotosDelete.ConvertRequestParams(requestParams)).Result;
                if (string.Equals(response, "{\"response\":1}", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else
                {
                    throw new Exception(response);
                }
            }
            catch (Exception ex)
            {
                ex.Error();
                return false;
            }
        }

        public static string GetMessageAttachmentUrl(string accessToken)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                RequestParams requestParams = new RequestParams
                {
                    ["access_token"] = accessToken,
                    ["v"] = SiteData.VK.ApiVer,
                };
                var response = httpClient.GetStringAsync(SiteData.VK.ApiGetMessagesUploadServer.ConvertRequestParams(requestParams)).Result;
                var desResponse = JsonConvert.DeserializeObject<JsonCore.VK.MessagesUploadServerResponse>(response);
                if (desResponse.Response == null)
                    throw new Exception(response);
                return desResponse.Response.UploadUrl;
            }
            catch (Exception ex)
            {
                ex.Error();
                return null;
            }
        }

        public static JsonCore.VK.ResponseUG UsersGet(string accessToken, string userId)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                RequestParams requestParams = new RequestParams
                {
                    ["access_token"] = accessToken,
                    ["v"] = SiteData.VK.ApiVer,
                    ["user_ids"] = userId,
                    ["name_case"] = "nom",
                    ["fields"] = "can_write_private_message,blacklisted",
                };
                var response = httpClient.GetStringAsync(SiteData.VK.ApiUsersGet.ConvertRequestParams(requestParams)).Result;
                var desResponse = JsonConvert.DeserializeObject<JsonCore.VK.UsersGetResponse>(response);
                if (desResponse.Response == null)
                    throw new Exception(response);
                if (desResponse.Error != null)
                    throw new Exception(response);
                return desResponse.Response.First();
            }
            catch (Exception ex)
            {
                ex.Error();
                return null;
            }
        }

        public static string AudioSearch(string accessToken, string query)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                RequestParams requestParams = new RequestParams
                {
                    ["owner_id"] = DataStorage.VK_UserID,
                    ["q"] = query,
                    ["auto_complete"] = 0,
                    ["count"] = 10,
                    ["sort"] = 2,
                    ["access_token"] = accessToken,
                    ["v"] = SiteData.VK.ApiVer,
                };
                var response = httpClient.GetStringAsync(SiteData.VK.ApiAudioSearch.ConvertRequestParams(requestParams)).Result;
                var desResponse = JsonConvert.DeserializeObject<JsonCore.VK.AudioSearchResponse>(response);
                if (desResponse.Response == null || !desResponse.Response.Items.Any())
                    throw new Exception(response);
                return $"{desResponse.Response.Items[0].OwnerId}_{desResponse.Response.Items[0].Id}";
            }
            catch (Exception ex)
            {
                ex.Error();
                return string.Empty;
            }
        }

        public static bool AccountSetOnline(string accessToken)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                RequestParams requestParams = new RequestParams
                {
                    ["access_token"] = accessToken,
                    ["v"] = SiteData.VK.ApiVer,
                };
                var response = httpClient.GetStringAsync(SiteData.VK.ApiAccountSetOnline.ConvertRequestParams(requestParams)).Result;
                if (string.Equals(response, "{\"response\":1}", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else
                {
                    throw new Exception(response);
                }
            }
            catch (Exception ex)
            {
                ex.Error();
                return false;
            }
        }

        public static bool MakeAsReadMessage(string accessToken, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                int peerId = rCommandItem.PeerId;
                RequestParams requestParams = new RequestParams
                {
                    ["start_message_id"] = rCommandItem.Id,
                    ["peer_id"] = peerId,
                    ["access_token"] = accessToken,
                    ["v"] = SiteData.VK.ApiVer,
                };
                var response = httpClient.GetStringAsync(SiteData.VK.ApiMageAsReadMessage.ConvertRequestParams(requestParams)).Result;
                if (string.Equals(response, "{\"response\":1}", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else
                {
                    throw new Exception(response);
                }
            }
            catch (Exception ex)
            {
                ex.Error();
                return false;
            }
        }

        public static Image<Rgba32> GetImageByQuery(string accessToken, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
        {
            try
            {
                var query = rCommandItem.Message.Trim().ToLower().Replace("  ", " ");
                if (string.IsNullOrWhiteSpace(query)) throw new Exception("");
                var imagePath = $"{FileCore.PathImages}\\{query.GetDeterministicHashCode()}.png";
                try
                {
                    return Image.Load<Rgba32>(imagePath);
                }
                catch (FileNotFoundException)
                {
                    using HttpClient httpClient = new HttpClient();
                    RequestParams requestParams = new RequestParams
                    {
                        ["q"] = HttpUtility.UrlEncode(query),
                        ["key"] = AdminSettings.GOOGLE_SERVER_KEYS[SiteData.Google.i],
                        ["cx"] = AdminSettings.GOOGLE_CUSTOM_SEARCH_IDs[SiteData.Google.i],
                        ["searchType"] = "image",
                        ["safe"] = "off",
                    };
                    var url = SiteData.Google.ImageCustomSearchApi.ConvertRequestParams(requestParams);
                    string response = string.Empty;
                    try
                    {
                        response = httpClient.GetStringAsync(url).Result;
                    }
                    catch (Exception)
                    {
                        SiteData.Google.ChangeId();
                        SendMessage(accessToken, rCommandItem, "[ВНИМАНИЕ] Возможно был достигнут лимит запросов к Google custom api search! Была произведена смена ключей доступа. Повторите ваш запрос.");
                        return new Image<Rgba32>(0, 0);
                    }
                    var desResponse = JsonConvert.DeserializeObject<JsonCore.Google.ImageSearchResponse>(response);
                    if (desResponse.Url == null)
                    {
                        SiteData.Google.ChangeId();
                        SendMessage(accessToken, rCommandItem, "[ВНИМАНИЕ] Возможно был достигнут лимит запросов к Google custom api search! Была произведена смена ключей доступа. Повторите ваш запрос.");
                        return new Image<Rgba32>(0, 0);
                    }
                    if (!desResponse.Items.Any())
                        throw new Exception(response);
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadFile(new Uri(desResponse.Items.First(i => i.Mime.CheckImageType()).Link), imagePath);
                    }
                    var image = Image.Load<Rgba32>(imagePath);
                    if (image.Height > 720)
                    {
                        int height = 720;
                        int width = (int)(720 / (double)image.Height * image.Width);
                        image.Mutate(i => i.Resize(width, height));
                    }
                    return image;
                }
            }
            catch (Exception ex)
            {
                ex.Error();
                return new Image<Rgba32>(0, 0);
            }
        }

        public static bool SendMessage(string accessToken, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem, string message, string attachment = "", string captchaSid = "", string captchaImage = "", bool doNormmalize = true, bool forwardMessages = true)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                if (string.IsNullOrWhiteSpace(message) && string.IsNullOrWhiteSpace(attachment))
                {
                    return false;
                }

                if (message.Length > 4000)
                {
                    message = message.Substring(0, 4000) + "\n[...]\n[Просмотрите полную версию письма в почтовом ящике!]";
                }
                var requestParams = new Dictionary<string, string>
                {
                    {"dont_parse_links", "0"},
                    {"owner_id" , DataStorage.VK_UserID},
                    {"access_token" , accessToken},
                    {"v" , SiteData.VK.ApiVer},
                    { "message" ,doNormmalize ? ConvertUtils.NormalizeVkMessage(message) : message},
                };
                if (rCommandItem.Id != -1 && forwardMessages)
                {
                    requestParams.Add("forward_messages", rCommandItem.Id.ToString());
                }

                if (!string.IsNullOrWhiteSpace(attachment))
                    requestParams.Add("attachment", attachment);

                var target = new KeyValuePair<string, string>("peer_id", rCommandItem.PeerId.ToString());
                requestParams.Add(target.Key, target.Value);
                var content = new FormUrlEncodedContent(requestParams);
                var response = httpClient.PostAsync(SiteData.VK.ApiMessagesSend, content).Result.Content.ReadAsStringAsync().Result;
                if (Regex.IsMatch(response, "{\"response\":[0-9]+}"))
                {
                    return true;
                }
                var desResponse = JsonConvert.DeserializeObject<JsonCore.VK.SaveOwnerPhotoResponse>(response);
                if (desResponse.Error.ErrorCode == 14)
                {
                    return SendMessage(accessToken, rCommandItem, message, attachment, desResponse.Error.CaptchaSid, desResponse.Error.CaptchaImg);
                }
                throw new Exception(response);
            }
            catch (Exception ex)
            {
                ex.Error();
                return false;
            }
        }

        public static string GetShortLink(string accessToken, string url)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                var urlEncoded = HttpUtility.UrlEncode(url);
                RequestParams requestParams = new RequestParams
                {
                    ["url"] = urlEncoded,
                    ["private"] = 1,
                    ["access_token"] = accessToken,
                    ["v"] = SiteData.VK.ApiVer,
                };
                var response = httpClient.GetStringAsync(SiteData.VK.ApiGetShortLink.ConvertRequestParams(requestParams)).Result;
                var desResponse = JsonConvert.DeserializeObject<JsonCore.VK.GetShortLink.CoreResponse>(response);
                if (desResponse.Response.Url == url)
                {
                    return desResponse.Response.ShortUrl;
                }
                else
                {
                    throw new Exception(response);
                }
            }
            catch (Exception ex)
            {
                ex.Error();
                return "";
            }
        }

        public static bool MessagesSetActivity(string accessToken, int peerId)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                RequestParams requestParams = new RequestParams
                {
                    ["user_id"] = DataStorage.VK_UserID,
                    ["type"] = "typing",
                    ["peer_id"] = peerId,
                    ["access_token"] = accessToken,
                    ["v"] = SiteData.VK.ApiVer,
                };
                var response = httpClient.GetStringAsync(SiteData.VK.ApiMessagesSetActivity.ConvertRequestParams(requestParams)).Result;
                if (string.Equals(response, "{\"response\":1}", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else
                {
                    throw new Exception(response);
                }
            }
            catch (Exception ex)
            {
                ex.Error();
                return false;
            }
        }

        public static string GetJoke()
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                RequestParams requestParams = new RequestParams
                {
                    ["CType"] = 1,
                };
                var url = "http://rzhunemogu.ru/RandJSON.aspx";
                var response = httpClient.GetStringAsync(url.ConvertRequestParams(requestParams)).Result;
                var desResponse = JsonConvert.DeserializeObject<JsonCore.Joke.JokeResponse>(response);
                return desResponse.Content;
            }
            catch (Exception ex)
            {
                ex.Error();
                return string.Empty;
            }
        }

        public static string RecognizeTextFromAudio(byte[] audioData)
        {
            try
            {
                WebRequest request = WebRequest.Create(SiteData.Yandex.coreUrl);
                request.Method = "post";
                request.ContentType = "audio/x-mpeg-3";
                request.Timeout = 60000;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(audioData, 0, audioData.Length);
                }

                using WebResponse response = request.GetResponse();
                using (Stream dataStream = response.GetResponseStream())
                {
                    using StreamReader reader = new StreamReader(dataStream);
                    string result = reader.ReadToEnd();
                    return Regex.Match(result, "(?<=<variant.*>)(.)*(?=</variant>)").ToString();
                }
            }
            catch (Exception ex)
            {
                ex.Error();
                return string.Empty;
            }
        }

        public static string WikiSearch(string query)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                RequestParams requestParams = new RequestParams
                {
                    ["action"] = "query",
                    ["prop"] = "extracts",
                    ["titles"] = query,
                    ["explaintext"] = true,
                    ["redirects"] = true,
                    ["format"] = "json",
                };
                var response = httpClient.GetStringAsync(SiteData.Wiki.ApiUrl.ConvertRequestParams(requestParams)).Result;
                var desResponse = JsonConvert.DeserializeObject<JsonCore.Wiki.Response>(response);
                if (string.IsNullOrWhiteSpace(desResponse.Query?.Pages.First().Value.Extract))
                    return $"Информация по запросу {query} не найдена!";
                string[] sentence = desResponse.Query.Pages.First()
                    .Value.Extract.Split('.', '!', '?');
                int lenght = 0;
                for (int i = 0; i < 6 && sentence.Length > i; i++)
                {
                    if (sentence[i].Contains("=="))
                    {
                        lenght += sentence[i].Substring(0, sentence[i].IndexOf("==", StringComparison.Ordinal)).Length;
                        break;
                    }
                    lenght += sentence[i].Length + 1;
                    if (lenght > 4000) break;
                }

                var extract = desResponse.Query.Pages.First()
                    .Value.Extract;
                lenght = lenght > extract.Length ? extract.Length : lenght;
                return extract.Substring(0, lenght).Trim();
                //return $"{des_response.Response.Items[0].Url}";
            }
            catch (Exception ex)
            {
                ex.Error();
                return string.Empty;
            }
        }

        private static string _awesomeToken;

        public static string GetAwesomeToken(out bool status)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                var response = httpClient.GetAsync(SiteData.Awesome.MainUrlS).Result.Content.ReadAsStringAsync().Result;
                var token = Regex.Match(response, "(?<=access_token\" value=\")(.)*(?=\" type)").Value;
                if (!string.IsNullOrWhiteSpace(token))
                {
                    _awesomeToken = token;
                    status = true;
                    return token;
                }
                throw new Exception(response);
            }
            catch (Exception ex)
            {
                ex.Error();
                status = false;
                _awesomeToken = string.Empty;
                return $"Произошла ошибка, Awesome token НЕ был получен!";
            }
        }

        public static string AwesomeCheck(string imageUrl, out bool status)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                var token = GetAwesomeToken(out status);
                if (!status) throw new Exception(token);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                RequestParams rp = new RequestParams()
                {
                    ["url"] = HttpUtility.UrlEncode(imageUrl),
                };
                var response = httpClient.GetAsync(SiteData.Awesome.ApiUrl.ConvertRequestParams(rp)).Result.Content.ReadAsStringAsync().Result;
                var desResponse = JsonConvert.DeserializeObject<JsonCore.Awsome.CoreResponse>(response);
                if (!string.IsNullOrWhiteSpace(desResponse?.Status) && desResponse.Status == "ok" && desResponse?.Quality != null)
                {
                    status = true;
                    return Math.Round(desResponse.Quality.Score * 100, 2).ToString();
                }
                throw new Exception(response);
            }
            catch (Exception ex)
            {
                ex.Error();
                status = false;
                _awesomeToken = string.Empty;
                return $"Произошла ошибка, изображение НЕ было обработано!";
            }
        }
    }
}