using System;
using System.Threading.Tasks;

namespace VkBotApi
{
    public static class ErrorUtils
    {
        public static void Error(this Exception e)
        {
            Task.Factory.StartNew((() =>
            {
                try
                {
                    LogCore.Log(e.Message, $"{e}\n{e.TargetSite}\n",
                        EnumData.LogTypeCommand.Error, EnumData.LogSourceCommand.Other);
                    //MessageBox.Show(e.Message, "Error!", MessageBoxButtons.OK);
                }
                catch (AccessViolationException)
                {
                }
                catch (Exception ex)
                {
                    Error(ex);
                }
            }));
        }
    }
}