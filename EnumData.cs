namespace VkBotApi
{
    public class EnumData
    {
        public enum SiteDomain
        {
            Vk,
            Other
        }

        public enum LogTypeCommand
        {
            Error,
            Attention, Message, CMessage,
            Info,
            InfoSuccess,
            System,
            None
        }

        public enum LogSourceCommand
        {
            VK,
            Google,
            Other,
            Mail
        }

        public enum MessageCommand
        {
            Image,
            CommandUser,
            CommandAdmin,
            Music,
            Wiki,
            Unknown
        }

        public enum UserCmd
        {
            Joke,
            Weather,
            Rsp,
            Random,
            Awesome,
            AnonymousSendGroup,
            Help,
            Unknown,
            VoiceMessage,
            MailCheck,
        }

        public enum AdminCmd
        {
            ImageChange,
            TempImageChange,
            ImageCacheDelete,
            MailAdd,
            MailRemove,
        }

        public enum RspItems
        {
            Rock,
            Scissors,
            Paper
        }
    }
}