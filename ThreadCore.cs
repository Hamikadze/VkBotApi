using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using VkBotApi.Controllers;

namespace VkBotApi
{
    public class ThreadCore
    {
        public static Thread CheckingMessagesThread = new Thread(delegate () { });
        public static bool CheckingMessagesThreadStop;
        public static Thread CommandExecutionThread = new Thread(delegate () { });
        public static bool CommandExecutionThreadStop;
        public static bool SetOnlineThreadStop;
        public static bool CheckMailThreadStop;
        private static Thread _setOnlineThread = new Thread(delegate () { });
        private static readonly Thread _checkMailThread = new Thread(delegate () { });
        public static Thread ChangePhotosThread = new Thread(delegate () { });
        public static bool ChangePhotosThreadStop;
        public static bool UploadingPhotos = false;

        public static void CheckingMessagesThread_Start()
        {
            if (CheckingMessagesThread.IsAlive) return;
            CheckingMessagesThread = new Thread(delegate ()
            {
                do
                {
                    try
                    {
                        if (VkBotController.MessageNewQueue.Count <= 0 || VkBotController.MessageNewQueue[0] == null)
                        {
                            Thread.Sleep(200);
                            continue;
                        }
                        var message_new = VkBotController.MessageNewQueue[0];
                        VkBotController.MessageNewQueue.Remove(message_new);
                        message_new.Object.Text = Regex.Replace(message_new.Object.Text, @"\[(.)+?\|(.)+?\]", "").Trim();

                        var tempMessage = ((message_new.Object.Text.Length >= 2 && RMessagesData.RCommandsMarkers.Keys.Contains(message_new.Object.Text[0])) ||
                                           message_new.Object.VkCheckVoiceMessage() != string.Empty) ? message_new : null;

                        if (tempMessage != null)
                        {
                            string message_text = tempMessage.Object.Text.Trim();
                            if (message_text.Length <= 1) continue;
                            var messageCmd = RMessagesData.RCommandsMarkers[message_text[0]];
                            message_text = message_text.Substring(1).Trim();
                            var strCommand = message_text.Substring(0, message_text.Contains(" ") ? message_text.IndexOf(" ", StringComparison.Ordinal) : message_text.Length).Trim();
                            var tempReceivedItem =
                                new RMessagesData.RCmdsListCollect.RCommandsItem(
                                    tempMessage.Object.Date.ToDateTimeFromUnix(), message_text, strCommand, tempMessage?.Object.Attachments, tempMessage.Object.FromId, tempMessage.Object.PeerId, tempMessage.Object.Id,
                                    messageCmd, tempMessage.Object);
                            RMessagesData.RCmdsListCollect.RCmdsList.Add(tempReceivedItem);

                            LogCore.Log(message_text, JsonConvert.SerializeObject(tempMessage),
                                EnumData.LogTypeCommand.Info, EnumData.LogSourceCommand.VK);
                        }
                        Thread.Sleep(200);
                    }
                    catch (Exception ex)
                    {
                        ex.Error();
                    }
                } while (!CheckingMessagesThreadStop);
                CheckingMessagesThreadStop = false;
                CheckingMessagesThread.Abort();
            })
            { IsBackground = true };
            CheckingMessagesThread.Start();
        }

        public static void CheckingMessagesThread_Stop()
        {
            if (CheckingMessagesThread.IsAlive)
            {
                CheckingMessagesThreadStop = true;
            }
        }

        public static void CommandExecutionThread_Start()
        {
            if (CommandExecutionThread.IsAlive) return;
            CommandExecutionThread = new Thread(delegate ()
            {
                do
                {
                    try
                    {
                        if (RMessagesData.RCmdsListCollect.RCmdsList.Count <= 0 || RMessagesData.RCmdsListCollect.RCmdsList[0] == null)
                        {
                            Thread.Sleep(200);
                            continue;
                        }
                        var rCommandItem = RMessagesData.RCmdsListCollect.RCmdsList[0];
                        RMessagesData.RCmdsListCollect.RCmdsList.Remove(rCommandItem);
                        var accessToken = SiteData.VK.GetAccessToken();
                        int peerId = rCommandItem.PeerId;
                        SiteUtils.MessagesSetActivity(accessToken, peerId);
                        SiteUtils.MakeAsReadMessage(accessToken, rCommandItem);
                        Action action = null;
                        switch (rCommandItem.MCommand)
                        {
                            case EnumData.MessageCommand.Image:
                                action = () => MessagesCore.VkSendImageMessage(accessToken, rCommandItem);
                                break;

                            case EnumData.MessageCommand.CommandUser:
                                foreach (var key in RMessagesData.UserCommands.Keys)
                                {
                                    foreach (var part in key)
                                    {
                                        if (!rCommandItem.STRCommand.Equals(part, StringComparison.OrdinalIgnoreCase)) continue;
                                        rCommandItem.Message = rCommandItem.Message.Replace(part, String.Empty).Trim();
                                        switch (RMessagesData.UserCommands[key])
                                        {
                                            case EnumData.UserCmd.Joke:
                                                action = () => MessagesCore.VkSendJokeMessage(accessToken, rCommandItem);
                                                break;

                                            case EnumData.UserCmd.Weather:
                                                break;

                                            case EnumData.UserCmd.Rsp:
                                                action = () => MessagesCore.VkSendRSPMessage(accessToken, rCommandItem);
                                                break;

                                            case EnumData.UserCmd.Random:
                                                action = () => MessagesCore.VkSendRandomValueMessage(accessToken, rCommandItem);
                                                break;

                                            case EnumData.UserCmd.Awesome:

                                                action = () => MessagesCore.VkSendAwesomeMessage(accessToken, rCommandItem);
                                                break;

                                            case EnumData.UserCmd.Help:
                                                action = () => MessagesCore.VkSendHelpMessage(accessToken, rCommandItem);
                                                break;

                                            case EnumData.UserCmd.AnonymousSendGroup:
                                                action = () => MessagesCore.VkSendAnonymousMessage(accessToken, rCommandItem);
                                                break;

                                            case EnumData.UserCmd.VoiceMessage:
                                                action = () => MessagesCore.VkSendVoiceRecognizedMessage(accessToken, rCommandItem);
                                                break;

                                            case EnumData.UserCmd.MailCheck:
                                                action = () => MessagesCore.VkMailCheck(accessToken, rCommandItem);
                                                break;

                                            default:
                                                throw new ArgumentOutOfRangeException();
                                        }
                                    }
                                }
                                break;

                            case EnumData.MessageCommand.Music:
                                action = () => MessagesCore.VkSendMusicMessage(accessToken, rCommandItem);
                                break;

                            case EnumData.MessageCommand.CommandAdmin:
                                foreach (var key in RMessagesData.AdminCommands.Keys)
                                {
                                    rCommandItem.Message = rCommandItem.Message.ToLower();
                                    if (!rCommandItem.STRCommand.Equals(key, StringComparison.OrdinalIgnoreCase)) continue;
                                    rCommandItem.Message = rCommandItem.Message.Replace(key, string.Empty).Trim();
                                    switch (RMessagesData.AdminCommands[key])
                                    {
                                        case EnumData.AdminCmd.ImageChange:
                                            action = () => MessagesCore.VkChangeImage(accessToken, rCommandItem);
                                            break;

                                        case EnumData.AdminCmd.ImageCacheDelete:
                                            action = () => MessagesCore.VkImageCacheDelete(accessToken, rCommandItem);
                                            break;

                                        case EnumData.AdminCmd.TempImageChange:
                                            break;

                                        case EnumData.AdminCmd.MailAdd:
                                            action = () => MessagesCore.VkMailAdd(accessToken, rCommandItem);
                                            break;

                                        case EnumData.AdminCmd.MailRemove:
                                            action = () => MessagesCore.VkMailRemove(accessToken, rCommandItem);
                                            break;

                                        default:
                                            throw new ArgumentOutOfRangeException();
                                    }
                                }
                                break;

                            case EnumData.MessageCommand.Wiki:
                                if (!rCommandItem.Message.All(i => i == '?' || i == ')' || i == '('))
                                    action = () => MessagesCore.VkSendWikiMessage(accessToken, rCommandItem);
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        if (action == null) continue;
                        new Thread(action.Invoke) { IsBackground = true }.Start();
                    }
                    catch (Exception ex)
                    {
                        ex.Error();
                    }
                } while (!CommandExecutionThreadStop);
                CommandExecutionThreadStop = false;
                CommandExecutionThread.Abort();
            })
            { IsBackground = true };
            CommandExecutionThread.Start();
        }

        public static void CommandExecutionThread_Stop()
        {
            if (CommandExecutionThread.IsAlive)
            {
                CommandExecutionThreadStop = true;
            }
        }

        public static void AccountSetOnlineStart()
        {
            if (_setOnlineThread.IsAlive) return;
            _setOnlineThread = new Thread(delegate ()
            {
                do
                {
                    try
                    {
                        var accessToken = SiteData.VK.GetAccessToken();
                        DataStorage.DialogsList = SiteUtils.GetDialogs(accessToken);
                        //SiteUtils.AccountSetOnline(accessToken);
                        Thread.Sleep(300000);
                    }
                    catch (Exception ex)
                    {
                        ex.Error();
                    }
                } while (!SetOnlineThreadStop);
                SetOnlineThreadStop = false;
                _setOnlineThread.Abort();
            })
            { IsBackground = true };

            _setOnlineThread.Start();
        }

        public static void CheckMailThread_Stop()
        {
            if (_checkMailThread.IsAlive)
            {
                CheckMailThreadStop = true;
            }
        }
    }
}