using System;
using System.Collections.Generic;

namespace VkBotApi
{
    internal class DataStorage
    {
        public static string VK_UserID { get; set; }
        public static List<JsonCore.VK.MessageGD> DialogsList = null;
        public static Dictionary<int, DateTime> LastAnonymousMessage = new Dictionary<int, DateTime>();
    }
}