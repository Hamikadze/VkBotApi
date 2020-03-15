using System;
using System.Collections.Generic;

namespace VkBotApi
{
    internal class RMessagesData
    {
        public static Dictionary<char, EnumData.MessageCommand> RCommandsMarkers = new Dictionary<char, EnumData.MessageCommand>()
        {
            { '!', EnumData.MessageCommand.Image},
            { '/', EnumData.MessageCommand.CommandUser},
            { '$', EnumData.MessageCommand.Music},
            { '~', EnumData.MessageCommand.CommandAdmin},
            { '?', EnumData.MessageCommand.Wiki},
        };

        public static Dictionary<string[], EnumData.UserCmd> UserCommands = new Dictionary<string[], EnumData.UserCmd>()
        {
            { new [] { "скучно", "шутка", "анекдот"}, EnumData.UserCmd.Joke},
            { new [] { "погода" },  EnumData.UserCmd.Weather},
            { new [] { "камень", "ножницы", "бумага" }, EnumData.UserCmd.Rsp },
            { new [] { "random", "rndm", "rnd" }, EnumData.UserCmd.Random },
            { new [] { "awesome", "cool", "wow" }, EnumData.UserCmd.Awesome },
            { new [] { "recognize", "rec" },  EnumData.UserCmd.VoiceMessage} ,
            { new [] { "asend" }, EnumData.UserCmd.AnonymousSendGroup },
            { new [] { "help", "docs", "помощь", "документация", "commands", "команды" }, EnumData.UserCmd.Help },
            { new [] { "почта","проверить почту","mail","check mail","письма"}, EnumData.UserCmd.MailCheck },
        };

        public static Dictionary<string, EnumData.AdminCmd> AdminCommands = new Dictionary<string, EnumData.AdminCmd>()
        {
            { "imgch" , EnumData.AdminCmd.ImageChange},
            { "imgtmpch",  EnumData.AdminCmd.TempImageChange},
            { "imgdel",  EnumData.AdminCmd.ImageCacheDelete},
            { "mail_add",  EnumData.AdminCmd.MailAdd},
            { "mail_remove",  EnumData.AdminCmd.MailRemove},
        };

        public static Dictionary<string, EnumData.RspItems> RspConverter = new Dictionary<string, EnumData.RspItems>
        {
            {"камень", EnumData.RspItems.Rock},
            {"ножницы", EnumData.RspItems.Scissors},
            {"бумага", EnumData.RspItems.Paper},
        };

        public static class RCmdsListCollect
        {
            public static List<RCommandsItem> RCmdsList = new List<RCommandsItem>();

            public class RCommandsItem
            {
                public RCommandsItem(DateTime dateTime, string message, string strCommand, List<JsonCore.VK.MessageNew.Attachment> attachments, int fromId, int peerId, int id, EnumData.MessageCommand mCommand, JsonCore.VK.MessageNew.Object itemConversations)
                {
                    DateTime = dateTime;
                    Message = message;
                    STRCommand = strCommand;
                    Attachments = attachments;
                    FromId = fromId;
                    PeerId = peerId;
                    Id = id;
                    MCommand = mCommand;
                    ItemConversations = itemConversations;
                }

                public DateTime DateTime { get; set; }
                public string Message { get; set; }
                public string STRCommand { get; set; }
                public List<JsonCore.VK.MessageNew.Attachment> Attachments { get; set; }
                public int FromId { get; set; }
                public int PeerId { get; set; }
                public int Id { get; set; }
                public EnumData.MessageCommand MCommand { get; set; }
                public JsonCore.VK.MessageNew.Object ItemConversations { get; set; }
            }
        }
    }
}