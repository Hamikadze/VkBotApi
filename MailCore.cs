using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using S22.Imap;

namespace VkBotApi
{
    internal class MailCore
    {
        public static void Save()
        {
            try
            {
                int log = $"[...] Saving file {FileCore.PathMailData.GetNameFromPath()}".DActionLog();
                if (MailSecureList.Any())
                {
                    var json = JsonConvert.SerializeObject(MailSecureList, Formatting.Indented);
                    FileCore.PathMailData.SaveToFile(json);
                }
                log.DActionLog();
            }
            catch (Exception ex)
            {
                ex.Error();
            }
        }

        public static void Read()
        {
            try
            {
                int log = $"[...] Loading file {FileCore.PathMailData.GetNameFromPath()}".DActionLog();
                MailSecureList.Clear();
                if (!File.Exists(FileCore.PathMailData))
                    FileCore.PathMailData.ClearFile();
                var json = FileCore.PathMailData.ReadFromFile();

                if (string.IsNullOrWhiteSpace(json) || json == "\r\n")
                {
                    log.DActionLog();
                    return;
                }
                MailSecureList = JsonConvert.DeserializeObject<List<MailSecureItem>>(json);
                log.DActionLog();
            }
            catch (Exception ex)
            {
                ex.Error();
            }
        }

        public static Dictionary<int, ImapClient> ImapClients = new Dictionary<int, ImapClient>();
        public static Dictionary<int, AutoResetEvent> AutoResetEvents = new Dictionary<int, AutoResetEvent>();
        public static Dictionary<int, Thread> Threads = new Dictionary<int, Thread>();

        public static void InitializeInboxes()
        {
            try
            {
                lock (MailSecureList)
                    foreach (var mailSecureItem in MailSecureList)
                    {
                        try
                        {
                            var hash = mailSecureItem.GetHashCode();
                            ImapClients.Add(hash, new ImapClient(mailSecureItem.Host, mailSecureItem.Port, mailSecureItem.Username, mailSecureItem.Password, AuthMethod.Login, mailSecureItem.Ssl));

                            AutoResetEvents.Add(hash, new AutoResetEvent(false));
                            Action action = () => Initialize(mailSecureItem);
                            var thread = new Thread(action.Invoke) { IsBackground = true };
                            Threads.Add(hash, thread);
                            thread.Start();
                        }
                        catch (Exception e)
                        {
                            e.Error();
                        }
                    }
            }
            catch (Exception e)
            {
                e.Error();
                throw;
            }
        }

        public static void Initialize(MailSecureItem mailSecureItem)
        {
            LogCore.Log($"Initializing mail [{mailSecureItem.Username}]", null, EnumData.LogTypeCommand.System,
                EnumData.LogSourceCommand.Mail);
            while (true)
            {
                try
                {
                    var hash = mailSecureItem.GetHashCode();
                    ImapClients[hash]?.Dispose();
                    ImapClients.Remove(hash);
                    ImapClients.Add(hash, new ImapClient(mailSecureItem.Host, mailSecureItem.Port, mailSecureItem.Username, mailSecureItem.Password, AuthMethod.Login, mailSecureItem.Ssl));
                    ImapClients[hash].IdleError += (sender, args) =>
                    {
                        args.Exception.Error();
                        AutoResetEvents[hash].Set();
                    };

                    ImapClients[hash].NewMessage += (sender, args) =>
                    {
                        try
                        {
                            var letter = args.Client.GetMessage(args.MessageUID, FetchOptions.NoAttachments);
                            SiteUtils.SendMessage(SiteData.VK.GetAccessToken(), mailSecureItem.rCommandItem, letter.ToString(mailSecureItem), "", "", "", true, false);
                        }
                        catch (Exception ex)
                        {
                            SiteUtils.SendMessage(SiteData.VK.GetAccessToken(), mailSecureItem.rCommandItem, "Было получено новое сообщение, но его загрузка завершилась неудачей!");
                            ex.Error();
                        }
                    };

                    AutoResetEvents[hash].WaitOne();
                }
                catch (Exception e)
                {
                    e.Error();
                }
            }
        }

        public static bool LoginMail(string host, int port, string username, string password, bool ssl, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem, string accessToken)
        {
            try
            {
                LogCore.Log("Login mail...", null, EnumData.LogTypeCommand.System,
                    EnumData.LogSourceCommand.Mail);
                ImapClient imapClient = null;
                try
                {
                    imapClient = new ImapClient(host, port, username, password, AuthMethod.Login, ssl);
                }
                catch (InvalidCredentialsException ice)
                {
                    LogCore.Log(ice.Message, null, EnumData.LogTypeCommand.Attention,
                        EnumData.LogSourceCommand.Mail);
                    var answer = ice.Message;
                    var loginLink = Regex.Match(answer, @"http[^ \]]+").Value;
                    if (!string.IsNullOrWhiteSpace(loginLink))
                    {
                        var shortLink = SiteUtils.GetShortLink(accessToken, loginLink);
                        answer = answer.Replace(loginLink, shortLink);
                        if (answer.Contains("WEBALERT"))
                        {
                            SiteUtils.SendMessage(accessToken, rCommandItem,
                                $"Подтвердите вход в почту или разрешить небезопасным приложениям доступ к аккаунту. Ответ сервера :\n{answer}");
                        }
                        else
                        {
                            if (answer.Contains("ALERT"))
                            {
                                SiteUtils.SendMessage(accessToken, rCommandItem,
                                    $"Ошибка вода в почту. Ответ сервера :\n{answer}");
                            }
                        }
                    }
                }

                if (imapClient != null)
                {
                    if (imapClient.Authed)
                    {
                        var mailSecureItem = new MailSecureItem(host, port, ssl, username, password, rCommandItem);
                        MailSecureList.Add(mailSecureItem);
                        Save();
                        var hash = mailSecureItem.GetHashCode();
                        ImapClients.Add(hash, imapClient);
                        AutoResetEvents.Add(hash, new AutoResetEvent(false));
                        Action action = () => Initialize(mailSecureItem);
                        var thread = new Thread(action.Invoke) { IsBackground = true };
                        Threads.Add(hash, thread);
                        thread.Start();
                        LogCore.Log("Login complete!", null, EnumData.LogTypeCommand.System,
                            EnumData.LogSourceCommand.Mail);
                    }
                    else
                    {
                        LogCore.Log("Login NOT complete!", null, EnumData.LogTypeCommand.Attention,
                            EnumData.LogSourceCommand.Mail);
                    }

                    return imapClient.Authed;
                }

                return false;
            }
            catch (Exception ex)
            {
                ex.Error();
                return false;
            }
        }

        public static List<MailSecureItem> MailSecureList = new List<MailSecureItem>();

        public class MailSecureItem
        {
            public MailSecureItem(string host, int port, bool ssl, string username, string password, RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem)
            {
                Host = host;
                Port = port;
                Ssl = ssl;
                Username = username;
                Password = password;
                this.rCommandItem = rCommandItem;
            }

            public string Host { get; set; }
            public int Port { get; set; }
            public bool Ssl { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public RMessagesData.RCmdsListCollect.RCommandsItem rCommandItem;
        }

        public static List<MailMessage> GetMail(MailSecureItem data)
        {
            try
            {
                List<MailMessage> messages = new List<MailMessage>();
                var msgs =
                    ImapClients[data.GetHashCode()].Search(
                        SearchCondition.Unseen()).Reverse();
                messages.AddRange(ImapClients[data.GetHashCode()].GetMessages(msgs.Take(10), FetchOptions.NoAttachments));

                return messages;
            }
            catch (InvalidOperationException ex)
            {
                ex.Error();
                return null;
            }
            catch (IndexOutOfRangeException ex)
            {
                ex.Error();
                return null;
            }
            catch (Exception ex)
            {
                ex.Error();
                return null;
            }
        }
    }
}