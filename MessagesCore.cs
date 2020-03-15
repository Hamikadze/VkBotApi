using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace VkBotApi
{
    internal static class MessagesCore
    {
        public static string VkCheckVoiceMessage(this JsonCore.VK.MessageNew.Object item)
        {
            try
            {
                var attachmentsFwd = item?.FwdMessages?.FindAll(i => i != null && i.Attachments.Any()).SelectMany(i => i.Attachments).ToList();
                var findResultFwd =
                    attachmentsFwd?.Find(l => !string.IsNullOrWhiteSpace(l.Doc?.Preview?.AudioMsg.LinkMp3));
                if (findResultFwd != null)
                {
                    return findResultFwd.Doc.Preview.AudioMsg.LinkMp3;
                }
                var attachments = item?.Attachments;
                var findResult =
                    attachments?.Find(l => !string.IsNullOrWhiteSpace(l.Doc?.Preview?.AudioMsg.LinkMp3));
                if (findResult != null)
                {
                    return findResult.Doc.Preview.AudioMsg.LinkMp3;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                ex.Error();
                return string.Empty;
            }
        }

        public static void VkSendImageMessage(string accessToken, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
        {
            try
            {
                int peerId = rCommandItem.PeerId;
                ThreadCore.UploadingPhotos = true;
                var url = SiteUtils.GetMessageAttachmentUrl(accessToken);
                var attachment = SiteUtils.GetImageByQuery(accessToken, rCommandItem);
                if (attachment == null)
                {
                    string message = $"Изображение по запросу {rCommandItem.Message} не найдено!";
                    SiteUtils.SendMessage(accessToken, rCommandItem, message);
                    LogCore.Log(message, JsonConvert.SerializeObject(rCommandItem),
                        EnumData.LogTypeCommand.CMessage, EnumData.LogSourceCommand.VK);
                    return;
                }
                if (attachment.Size() == new Size(0, 0))
                {
                    LogCore.Log("Смена ключей Google", JsonConvert.SerializeObject(rCommandItem),
                        EnumData.LogTypeCommand.CMessage, EnumData.LogSourceCommand.VK);
                    return;
                }

                SiteUtils.MessagesSetActivity(accessToken, peerId);

                var photos = SiteUtils.UploadPhotoToServerAsync(url, attachment);
                var photoId = SiteUtils.SaveMessagePhoto(accessToken, photos);

                //SiteUtils.MessagesSetActivity(accessToken, peerId);

                SiteUtils.SendMessage(accessToken, rCommandItem, "",
                    $"photo{photoId}");
                ThreadCore.UploadingPhotos = false;
                LogCore.Log(rCommandItem.Message, JsonConvert.SerializeObject(rCommandItem),
                    EnumData.LogTypeCommand.CMessage, EnumData.LogSourceCommand.VK);
            }
            catch (Exception ex)
            {
                ex.Error();
            }
        }

        public static void VkSendMusicMessage(string accessToken, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
        {
            try
            {
                var audioId = SiteUtils.AudioSearch(accessToken, rCommandItem.Message);
                if (audioId == string.Empty)
                {
                    string message = $"Аудио по запросу {rCommandItem.Message} не найдено!";
                    SiteUtils.SendMessage(accessToken, rCommandItem,
                        message);
                    LogCore.Log(message, JsonConvert.SerializeObject(rCommandItem),
                        EnumData.LogTypeCommand.CMessage, EnumData.LogSourceCommand.VK);
                    return;
                }
                SiteUtils.SendMessage(accessToken, rCommandItem, "",
                    $"audio{audioId}");
                LogCore.Log(rCommandItem.Message, JsonConvert.SerializeObject(rCommandItem), EnumData.LogTypeCommand.CMessage, EnumData.LogSourceCommand.VK);
            }
            catch (Exception ex)
            {
                ex.Error();
            }
        }

        public static void VkSendVoiceRecognizedMessage(string accessToken, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    var voiceUrl = rCommandItem.ItemConversations.VkCheckVoiceMessage();
                    if (string.IsNullOrWhiteSpace(voiceUrl))
                    {
                        string message = $"Голосовое сообщение не было распознано! Пустая ссылка аудиосообщения! {voiceUrl}";
                        SiteUtils.SendMessage(accessToken, rCommandItem,
                            message);
                        LogCore.Log(rCommandItem.Message, JsonConvert.SerializeObject(rCommandItem),
                            EnumData.LogTypeCommand.CMessage, EnumData.LogSourceCommand.VK);
                        return;
                    }
                    var audioArray = wc.DownloadData(voiceUrl);
                    //var audioArray = File.ReadAllBytes("speech.ogg");
                    var text = SiteUtils.RecognizeTextFromAudio(audioArray).Trim();
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        string message = $"Голосовое сообщение не было распознано! Пустой текст распознанного аудиосообщения!";
                        SiteUtils.SendMessage(accessToken, rCommandItem,
                            message);
                        LogCore.Log(rCommandItem.Message, JsonConvert.SerializeObject(rCommandItem),
                            EnumData.LogTypeCommand.CMessage, EnumData.LogSourceCommand.VK);
                        return;
                    }
                    SiteUtils.SendMessage(accessToken, rCommandItem, text);
                    LogCore.Log(text, JsonConvert.SerializeObject(rCommandItem), EnumData.LogTypeCommand.CMessage, EnumData.LogSourceCommand.VK);
                }
            }
            catch (Exception ex)
            {
                ex.Error();
            }
        }

        public static void VkSendJokeMessage(string accessToken, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
        {
            try
            {
                SiteUtils.SendMessage(accessToken, rCommandItem, SiteUtils.GetJoke());
                LogCore.Log(rCommandItem.Message, JsonConvert.SerializeObject(rCommandItem), EnumData.LogTypeCommand.CMessage, EnumData.LogSourceCommand.VK);
            }
            catch (Exception ex)
            {
                ex.Error();
            }
        }

        public static void VkChangeImage(string accessToken, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
        {
            try
            {
                if (!ChechTotpCode(accessToken, ref rCommandItem)) return;
                var attachments = rCommandItem?.Attachments?.Where(i => i.Photo != null);
                if (attachments == null) return;
                var attachment = attachments.First();
                string imageLink = attachment.Photo.Sizes.First(i => i.Width * i.Height == attachment.Photo.Sizes.Max(size2 => size2.Height * size2.Width)).Url;
                LogCore.Log(rCommandItem.Message, JsonConvert.SerializeObject(rCommandItem), EnumData.LogTypeCommand.CMessage, EnumData.LogSourceCommand.VK);
                if (imageLink == string.Empty)
                {
                    SiteUtils.SendMessage(accessToken, rCommandItem, "Изображение НЕ было добавлено/изменено!"); return;
                }
                var imagePath = $"{FileCore.PathImages}\\{rCommandItem.Message.GetDeterministicHashCode()}.png";
                File.Delete(imagePath);
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(new Uri(imageLink), imagePath);
                }
                SiteUtils.SendMessage(accessToken, rCommandItem, "Изображение было добавлено/изменено!");
            }
            catch (Exception ex)
            {
                SiteUtils.SendMessage(accessToken, rCommandItem, "Изображение НЕ было добавлено/изменено!");
                ex.Error();
            }
        }

        public static void VkImageCacheDelete(string accessToken, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
        {
            try
            {
                if (!ChechTotpCode(accessToken, ref rCommandItem)) return;
                LogCore.Log(rCommandItem.Message, JsonConvert.SerializeObject(rCommandItem), EnumData.LogTypeCommand.CMessage, EnumData.LogSourceCommand.VK);
                var imagePath = $"{FileCore.PathImages}\\{rCommandItem.Message.GetDeterministicHashCode()}.png";
                File.Delete(imagePath);
                SiteUtils.SendMessage(accessToken, rCommandItem, "Изображение было удалено!");
            }
            catch (Exception ex)
            {
                SiteUtils.SendMessage(accessToken, rCommandItem, "Изображение НЕ было удалено!");
                ex.Error();
            }
        }

        private static string LastConfirmCode { get; set; }

        public static bool ChechTotpCode(string accessToken, ref RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
        {
            try
            {
                string code = rCommandItem.Message.Substring(0, 6);
                Totp totp = new Totp(AdminSettings.TOTP_KEY);
                var totpCode = totp.GetCodeString();
                if (!string.Equals(code, totpCode, StringComparison.OrdinalIgnoreCase) || LastConfirmCode == code)
                {
                    SiteUtils.SendMessage(accessToken, rCommandItem, "Команда НЕ была выполнена! Неверный код подтверждения!");
                    return false;
                }
                LastConfirmCode = code;
                rCommandItem.Message = rCommandItem.Message.Replace(code, string.Empty).Trim();
                return true;
            }
            catch (Exception ex)
            {
                ex.Error();
                return false;
            }
        }

        public static void VkSendAnonymousMessage(string accessToken, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
        {
            try
            {
                if (rCommandItem.Message.IndexOf(" ", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    SiteUtils.SendMessage(accessToken, rCommandItem, "Ошибка в распозновании названия группы!");
                    return;
                }

                Regex r = new Regex(@"\s+");
                var groupData = DataStorage.DialogsList.Find(
                    i => r.Replace(i.Title, @" ").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .All(j => r.Replace(rCommandItem.Message, @" ").IndexOf(j, StringComparison.OrdinalIgnoreCase) >= 0));

                if (string.IsNullOrWhiteSpace(groupData?.Title))
                {
                    SiteUtils.SendMessage(accessToken, rCommandItem, "Ошибка в распозновании названия группы!");
                    return;
                }
                rCommandItem.Message =
                    Regex.Replace(rCommandItem.Message, groupData.Title, "", RegexOptions.IgnoreCase).Trim();

                if (groupData.ChatId == 0)
                {
                    SiteUtils.SendMessage(accessToken, rCommandItem, "Ошибка в распозновании идентификатора группы!");
                    return;
                }

                if (DataStorage.LastAnonymousMessage.ContainsKey(rCommandItem.FromId))
                {
                    if ((DateTime.Now - DataStorage.LastAnonymousMessage[rCommandItem.FromId]).TotalSeconds < 30)
                    {
                        SiteUtils.SendMessage(accessToken, rCommandItem, "Анонимные сообщения можно отправлять ТОЛЬКО один раз в 30 секунд!");
                        //return;
                    }
                }
                else
                {
                    DataStorage.LastAnonymousMessage.Add(rCommandItem.FromId, DateTime.Now);
                }

                var photoId = string.Empty;
                if (rCommandItem?.Attachments != null && rCommandItem.Attachments.Any(i => i.Photo != null))
                {
                    int peerId = rCommandItem.PeerId;
                    ThreadCore.UploadingPhotos = true;
                    var url = SiteUtils.GetMessageAttachmentUrl(accessToken);
                    SiteUtils.MessagesSetActivity(accessToken, peerId);
                    var attachments = rCommandItem?.Attachments?.Where(i => i.Photo != null);
                    var attachment = attachments.First();
                    string imageLink = attachment.Photo.Sizes.First(i => i.Width * i.Height == attachment.Photo.Sizes.Max(size2 => size2.Height * size2.Width)).Url;
                    var imagePath = $"{FileCore.PathImages}\\{imageLink.GetDeterministicHashCode()}.png";
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadFile(new Uri(imageLink), imagePath);
                    }
                    var image = Image.Load<Rgba32>(imagePath);
                    if (image.Height > 720)
                    {
                        int height = 720;
                        int width = (int)(720 / (double)image.Height * image.Width);
                        image.Mutate(i => i.Resize(width, height));
                    }
                    var photos = SiteUtils.UploadPhotoToServerAsync(url, image);
                    photoId = SiteUtils.SaveMessagePhoto(accessToken, photos);

                    SiteUtils.MessagesSetActivity(accessToken, peerId);
                    ThreadCore.UploadingPhotos = false;
                }

                rCommandItem.PeerId = groupData.ChatId;

                var message = $"[АНОНИМНОЕ СООБЩЕНИЕ]"
                              +
                              "\n"
                              +
                              rCommandItem.Message
                                  .Substring(rCommandItem.Message.LastIndexOf("*", StringComparison.OrdinalIgnoreCase) +
                                             1).Trim();
                SiteUtils.MakeAsReadMessage(accessToken, rCommandItem);
                rCommandItem.Id = -1;
                DataStorage.LastAnonymousMessage[rCommandItem.FromId] = DateTime.Now;

                if (string.IsNullOrWhiteSpace(photoId))
                {
                    SiteUtils.SendMessage(accessToken, rCommandItem, message);
                }
                else
                {
                    SiteUtils.SendMessage(accessToken, rCommandItem, message,
                        $"photo{photoId}");
                }
                LogCore.Log(rCommandItem.Message, JsonConvert.SerializeObject(rCommandItem), EnumData.LogTypeCommand.CMessage, EnumData.LogSourceCommand.VK);
            }
            catch (Exception ex)
            {
                ex.Error();
            }
        }

        public static void VkSendRSPMessage(string accessToken, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
        {
            try
            {
                using (RNGCryptoServiceProvider rg = new RNGCryptoServiceProvider())
                {
                    byte[] rno = new byte[5];
                    rg.GetBytes(rno);
                    int randomvalue = BitConverter.ToInt32(rno, 0);
                    Random rnd = new Random(randomvalue);
                    var randomInt = rnd.Next(1, 3);
                    string message = "[{0}!] Я выбрал {1}";
                    var userChoise =
                        RMessagesData.RspConverter[rCommandItem.STRCommand];
                    var botChoise = (EnumData.RspItems)randomInt;
                    var botChoiseStr =
                        RMessagesData.RspConverter.Where(
                            i => i.Value == botChoise).Select(i => i.Key).First();
                    var loseSrt = "ВЫ ПРОИГРАЛИ";
                    var winStr = "ВЫ ПОБЕДИЛИ";
                    if (userChoise == botChoise)
                        message = string.Format(message, "НИЧЬЯ", botChoiseStr);
                    else
                    {
                        switch (userChoise)
                        {
                            case EnumData.RspItems.Rock:
                                if (botChoise == EnumData.RspItems.Paper)
                                    message = string.Format(message, loseSrt, botChoiseStr);
                                if (botChoise == EnumData.RspItems.Scissors)
                                    message = string.Format(message, winStr, botChoiseStr);
                                break;

                            case EnumData.RspItems.Scissors:
                                if (botChoise == EnumData.RspItems.Rock)
                                    message = string.Format(message, loseSrt, botChoiseStr);
                                if (botChoise == EnumData.RspItems.Paper)
                                    message = string.Format(message, winStr, botChoiseStr);
                                break;

                            case EnumData.RspItems.Paper:
                                if (botChoise == EnumData.RspItems.Scissors)
                                    message = string.Format(message, loseSrt, botChoiseStr);
                                if (botChoise == EnumData.RspItems.Rock)
                                    message = string.Format(message, winStr, botChoiseStr);
                                break;
                        }
                    }

                    SiteUtils.SendMessage(accessToken, rCommandItem, message);
                    LogCore.Log(rCommandItem.STRCommand, JsonConvert.SerializeObject(rCommandItem), EnumData.LogTypeCommand.CMessage, EnumData.LogSourceCommand.VK);
                }
            }
            catch (Exception ex)
            {
                ex.Error();
            }
        }

        public static void VkSendRandomValueMessage(string accessToken, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
        {
            try
            {
                string message = rCommandItem.Message;//.Substring(1);
                Random rnd = new Random();
                if (message.Contains("|"))
                {
                    string[] values = Array.ConvertAll(rCommandItem.Message.Split('|'), i => i.Trim());
                    int rndIndex = 0;
                    if (values.Any())
                    {
                        rndIndex = rnd.Next(0, values.Length);
                    }
                    message = values[rndIndex];
                }
                else if (message.Contains("-"))
                {
                    int[] values = Array.ConvertAll(rCommandItem.Message.Split('-'), i => int.Parse(i.Trim()));
                    if (values.Length > 1)
                    {
                        message = rnd.Next(values[0], values[1]).ToString();
                    }
                }
                SiteUtils.SendMessage(accessToken, rCommandItem, message);
                LogCore.Log(rCommandItem.Message, JsonConvert.SerializeObject(rCommandItem),
                    EnumData.LogTypeCommand.CMessage, EnumData.LogSourceCommand.VK);
            }
            catch (Exception ex)
            {
                ex.Error();
            }
        }

        public static void VkSendWikiMessage(string accessToken, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
        {
            try
            {
                var message = SiteUtils.WikiSearch(rCommandItem.Message);
                SiteUtils.SendMessage(accessToken, rCommandItem, message);
                LogCore.Log(rCommandItem.Message, JsonConvert.SerializeObject(rCommandItem), EnumData.LogTypeCommand.CMessage, EnumData.LogSourceCommand.VK);
            }
            catch (Exception ex)
            {
                ex.Error();
            }
        }

        public static void VkSendHelpMessage(string accessToken, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
        {
            try
            {
                var message = $"Для просмотра списка команд перейдите по ссылке :"
                    +
                    "\n"
                    +
                    @"https://vk.com/bot_igor?w=note379452173_11808679";
                SiteUtils.SendMessage(accessToken, rCommandItem, message);
                LogCore.Log(rCommandItem.Message, JsonConvert.SerializeObject(rCommandItem), EnumData.LogTypeCommand.CMessage, EnumData.LogSourceCommand.VK);
            }
            catch (Exception ex)
            {
                ex.Error();
            }
        }

        public static void VkSendAwesomeMessage(string accessToken, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
        {
            try
            {
                var attachments = rCommandItem?.Attachments?.Where(i => i.Photo != null);
                if (attachments == null) return;
                var attachment = attachments.First();
                string imageLink = attachment.Photo.Sizes.First(i => i.Width * i.Height == attachment.Photo.Sizes.Max(size2 => size2.Height * size2.Width)).Url;
                LogCore.Log(rCommandItem.Message, JsonConvert.SerializeObject(rCommandItem), EnumData.LogTypeCommand.CMessage, EnumData.LogSourceCommand.VK);
                if (imageLink == string.Empty)
                {
                    SiteUtils.SendMessage(accessToken, rCommandItem, "Изображение НЕ было найдено!"); return;
                }
                var value = SiteUtils.AwesomeCheck(imageLink, out bool status);
                var message = status ? $"Это изображение восхитительно на [{value}%]!" : value;
                SiteUtils.SendMessage(accessToken, rCommandItem, message);
            }
            catch (Exception ex)
            {
                SiteUtils.SendMessage(accessToken, rCommandItem, "Произошла ошибка, изображение НЕ было обработано!");
                ex.Error();
            }
        }

        public static void VkMailAdd(string accessToken, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
        {
            try
            {
                if (!ChechTotpCode(accessToken, ref rCommandItem)) return;
                LogCore.Log(rCommandItem.Message, JsonConvert.SerializeObject(rCommandItem), EnumData.LogTypeCommand.CMessage, EnumData.LogSourceCommand.VK);
                if (string.IsNullOrWhiteSpace(rCommandItem.Message))
                {
                    SiteUtils.SendMessage(accessToken, rCommandItem, "Пустые данные доступа к почте!");
                    return;
                }

                //rCommandItem.Message = "imap.gmail.com|993|best.homa@gmail.com|_|true";

                var data = rCommandItem.Message.Split('|');
                if (data.Length != 5)
                {
                    SiteUtils.SendMessage(accessToken, rCommandItem, "Недопустимое количество параметров для доступа к почте!");
                    return;
                }
                //string host, int port, string username, string password, bool ssl
                //imap.gmail.com|993|best.homa@gmail.com|fqngkupijdfgafhd|true

                if (MailCore.LoginMail(data[0].Trim(), Int32.Parse(data[1].Trim()), data[2].Trim(), data[3].Trim(),
                    bool.Parse(data[4].Trim()), rCommandItem, accessToken))
                {
                    SiteUtils.SendMessage(accessToken, rCommandItem, "Почта была добавлена!");
                }
                else
                {
                    SiteUtils.SendMessage(accessToken, rCommandItem, "Почта НЕ была добавлена!");
                }
            }
            catch (Exception ex)
            {
                SiteUtils.SendMessage(accessToken, rCommandItem, "Почта НЕ была добавлена! Возникла ошибка! Повторите попытку или обратитесь к администратору!");
                ex.Error();
            }
        }

        public static void VkMailRemove(string accessToken, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
        {
            try
            {
                if (!ChechTotpCode(accessToken, ref rCommandItem)) return;
                var result = MailCore.MailSecureList.RemoveAll(i => i.rCommandItem.PeerId == rCommandItem.PeerId);
                if (result != 0)
                {
                    SiteUtils.SendMessage(accessToken, rCommandItem, "Почта была удалена!");
                    MailCore.Save();
                }
                else
                {
                    SiteUtils.SendMessage(accessToken, rCommandItem, "Почта НЕ была удалена! Проверьте правильность ввода данных!");
                }
            }
            catch (Exception ex)
            {
                SiteUtils.SendMessage(accessToken, rCommandItem, "Почта НЕ была удалена! Возникла ошибка! Повторите попытку или обратитесь к администратору!");
                ex.Error();
            }
        }

        public static void VkMailCheck(string accessToken, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
        {
            try
            {
                var inboxes = MailCore.MailSecureList.FindAll(i => i.rCommandItem.PeerId == rCommandItem.PeerId);
                VkMailCheck(accessToken, inboxes);
            }
            catch (Exception ex)
            {
                SiteUtils.SendMessage(accessToken, rCommandItem, "Почта не была проверена! При повторе ошибки обратитесь к администратору!");
                ex.Error();
            }
        }

        public static void VkMailCheck(string accessToken, List<MailCore.MailSecureItem> mailSecureList)
        {
            try
            {
                foreach (var mailSecureItem in mailSecureList)
                {
                    try
                    {
                        var mail = MailCore.GetMail(mailSecureItem);
                        if (mail.Count == 0)
                        {
                            SiteUtils.SendMessage(accessToken, mailSecureItem.rCommandItem, $"Почтовый ящик : [{mailSecureItem.Username}] не имеет новых писем!", "", "", "", true, false);
                        }
                        else
                        {
                            foreach (var letter in mail)
                            {
                                //var letterBody = Regex.Match(letter.Body,
                                //    @"[\s\S]+?(?=(-|—|\s)+?(Пересылаемое сообщение)(-|—|\s)+?)").Value;
                                SiteUtils.SendMessage(accessToken, mailSecureItem.rCommandItem, letter.ToString(mailSecureItem), "", "", "", true, false);
                                Thread.Sleep(1000);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SiteUtils.SendMessage(accessToken, mailSecureItem.rCommandItem, "Почта не была проверена! При повторе ошибки обратитесь к администратору!", "", "", "", true, false);
                        ex.Error();
                    }
                }
            }
            catch (Exception e)
            {
                e.Error();
            }
        }
    }
}