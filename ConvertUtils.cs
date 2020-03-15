using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace VkBotApi
{
    internal static class ConvertUtils
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0,
       DateTimeKind.Utc);

        private static readonly string Const_TwitterDateTemplate = "ddd MMM dd HH:mm:ss +ffff yyyy";

        public static DateTime ToDateTimeFromUnix(this int time) => Epoch.AddSeconds(time);

        public static DateTime ToDateTimeFromTwitter(this string time)
            => DateTime.ParseExact(time, Const_TwitterDateTemplate, new CultureInfo("en-US"));

        //"2016-06-02T13:28:11.000Z"
        public static DateTime ToDateTimeFromTheScore(this string time) => DateTime.ParseExact(time, "yyyy-MM-ddTHH:mm:ss.000Z", new CultureInfo("en-US"));

        public static MemoryStream ToStreamFromString(this string value) =>
            new MemoryStream(Encoding.UTF8.GetBytes(value));

        public static string ToStringFromStream(this MemoryStream stream) => Encoding.UTF8.GetString(stream.GetBuffer());

        public static string ConvertRequestParams(this string baseUrl, RequestParams requestParams)
        {
            baseUrl += "?";
            baseUrl += string.Join("&", requestParams);
            return baseUrl;
        }

        public static string FwdCut(this MailMessage letter)
        {
            var body = letter.Body;
            return Regex.Replace(body, @"^>>(.)*", "", RegexOptions.Multiline);
        }

        public static string ToString(this MailMessage letter, MailCore.MailSecureItem mailSecureItem)
        {
            return
                $"Почтовый ящик : [{mailSecureItem.Username}]\nНовое письмо от : {letter.From}\nТема : {letter.Subject}\n{letter.FwdCut()}";
        }

        public static string NormalizeVkMessage(string message)
        {
            try
            {
                message = message.Replace("#", " sharp ");
                return message;
            }
            catch (Exception ex)
            {
                ex.Error();
                return string.Empty;
            }
        }

        private static Dictionary<int, string> DActionDic = new Dictionary<int, string>();

        public static int DActionLog(this string action)
        {
            var hashCode = action.GetHashCode();
            if (DActionDic.ContainsKey(hashCode))
                DActionDic.Remove(hashCode);
            DActionDic.Add(hashCode, action);
            LogCore.Log(action, null, EnumData.LogTypeCommand.Info, EnumData.LogSourceCommand.Other);
            return action.GetHashCode();
        }

        public static void DActionLog(this int hash)
        {
            if (!DActionDic.ContainsKey(hash)) return;
            string action = DActionDic[hash];
            LogCore.Log(action.Replace("...", "OK"), null, EnumData.LogTypeCommand.InfoSuccess, EnumData.LogSourceCommand.Other);
            DActionDic.Remove(hash);
        }

        public static string GetNameFromPath(this string path) => new FileInfo(path).Name;

        public static bool CheckImageType(this string str)
        {
            return str.Equals("image/png", StringComparison.OrdinalIgnoreCase) || str.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase)
                || str.Equals("image/gif", StringComparison.OrdinalIgnoreCase) || str.Equals("image/jpg", StringComparison.OrdinalIgnoreCase);
        }

        public static int GetDeterministicHashCode(this string str)
        {
            unchecked
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }
    }
}