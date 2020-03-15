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

        public static void Log(string message, string fullmessage, EnumData.LogTypeCommand mtype, EnumData.LogSourceCommand logSource)
        {
            Task.Factory.StartNew(() =>
            {
                var logItem = new LogListCollect.LogItem(DateTime.Now, message, fullmessage, mtype, logSource);
                LogListCollect.LogItemsWriteList.Add(logItem);
                LogListCollect.LogItemsShowList.Add(logItem);
            });
            //logCore.AddLogEventArgs(null, new LogEventArgs(message, fullmessage, mtype, logSource));
        }

        public static string GetType(EnumData.LogTypeCommand tc, out Color rColor)
        {
            switch (tc)
            {
                case EnumData.LogTypeCommand.Error:
                    rColor = Color.DarkRed;
                    return "ERROR";

                case EnumData.LogTypeCommand.Info:
                    rColor = Color.DarkGreen;
                    return "INFO";

                case EnumData.LogTypeCommand.InfoSuccess:
                    rColor = Color.LimeGreen;
                    return "INFO";

                case EnumData.LogTypeCommand.Message:
                    rColor = Color.Magenta;
                    return "INFO";

                case EnumData.LogTypeCommand.CMessage:
                    rColor = Color.DarkMagenta;
                    return "INFO";

                case EnumData.LogTypeCommand.Attention:
                    rColor = Color.DarkCyan;
                    return "ATTENTION";

                case EnumData.LogTypeCommand.System:
                    rColor = Color.Black;
                    return "SYSTEM";
            }

            rColor = Color.Gray;
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
                            Color rColor;
                            LogCore.GetType(logItem.MType, out rColor);
                            if (string.IsNullOrWhiteSpace(logItem.FullMessage))
                                logItem.FullMessage = logItem.Message;
                            if (logItem.MType != EnumData.LogTypeCommand.None)
                            {
                                FileCore.PathLog.SaveToFile($"{LogCore.GetType(logItem.MType, out rColor)} [{LogCore.GetSource(logItem.LogSource)}] ({logItem.DateTime:dd.MM.yyyy HH:mm:ss:fffff}) {logItem.FullMessage}", true);
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
        public static List<LogItem> LogItemsShowList = new List<LogItem>();

        public class LogItem
        {
            public LogItem(DateTime dateTime, string message, string fullmessage, EnumData.LogTypeCommand mtype, EnumData.LogSourceCommand logSource)
            {
                DateTime = dateTime;
                Message = message;
                FullMessage = fullmessage;
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