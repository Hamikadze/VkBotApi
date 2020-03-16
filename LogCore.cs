using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace VkBotApi
{
    public class LogCore
    {
        private static readonly object Locker = new object();

        private static Thread _logWriteThread = new Thread((() => { }));

        public static void Log(string message, string fullMessage, EnumData.LogTypeCommand mtype, EnumData.LogSourceCommand logSource)
        {
            Task.Factory.StartNew(() =>
            {
                var logItem = new LogListCollect.LogItem(DateTime.Now, message, fullMessage, mtype, logSource);
                LogListCollect.LogItemsWriteList.Add(logItem);
            });
        }

        public static string GetType(EnumData.LogTypeCommand tc)
        {
            switch (tc)
            {
                case EnumData.LogTypeCommand.Error:
                    return "ERROR";

                case EnumData.LogTypeCommand.Info:
                    return "INFO";

                case EnumData.LogTypeCommand.InfoSuccess:
                    return "INFO";

                case EnumData.LogTypeCommand.Message:
                    return "INFO";

                case EnumData.LogTypeCommand.CMessage:
                    return "INFO";

                case EnumData.LogTypeCommand.Attention:
                    return "ATTENTION";

                case EnumData.LogTypeCommand.System:
                    return "SYSTEM";
            }
            return "OTHER";
        }

        public static string GetSource(EnumData.LogSourceCommand sc)
        {
            switch (sc)
            {
                case EnumData.LogSourceCommand.Other:
                    return "OTHER";

                case EnumData.LogSourceCommand.Google:
                    return "GOOGLE";

                case EnumData.LogSourceCommand.VK:
                    return "VK";

                case EnumData.LogSourceCommand.Mail:
                    return "MAIL";
            }
            return null;
        }

        public static void LogWriteStart()
        {
            lock (Locker)
            {
                if (_logWriteThread.IsAlive) return;
                _logWriteThread = new Thread(() =>
                {
                    //_LogToFileThreadStop = false;
                    do
                    {
                        try
                        {
                            if (LogListCollect.LogItemsWriteList.Count <= 0 || LogListCollect.LogItemsWriteList[0] == null)
                            {
                                Thread.Sleep(250);
                                continue;
                            }
                            var logItem = LogListCollect.LogItemsWriteList[0];
                            if (string.IsNullOrWhiteSpace(logItem.FullMessage))
                                logItem.FullMessage = logItem.Message;
                            if (logItem.MType != EnumData.LogTypeCommand.None)
                            {
                                FileCore.PathLog.SaveToFile($"{GetType(logItem.MType)} [{GetSource(logItem.LogSource)}] ({logItem.DateTime:dd.MM.yyyy HH:mm:ss:fffff}) {logItem.FullMessage}", true);
                            }
                            LogListCollect.LogItemsWriteList.Remove(logItem);
                        }
                        catch (Exception ex)
                        {
                            ex.Error();
                        }
                    } while (true);
                });
                _logWriteThread.IsBackground = true;
                _logWriteThread.Start();
            }
        }
    }

    public static class LogListCollect
    {
        public static List<LogItem> LogItemsWriteList = new List<LogItem>();

        public class LogItem
        {
            public LogItem(DateTime dateTime, string message, string fullMessage, EnumData.LogTypeCommand mtype, EnumData.LogSourceCommand logSource)
            {
                DateTime = dateTime;
                Message = message;
                FullMessage = fullMessage;
                MType = mtype;
                LogSource = logSource;
            }

            public DateTime DateTime { get; set; }
            public string Message { get; set; }
            public string FullMessage { get; set; }
            public EnumData.LogTypeCommand MType { get; set; }
            public EnumData.LogSourceCommand LogSource { get; set; }
        }
    }
}