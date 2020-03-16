using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace VkBotApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VkBotController : ControllerBase
    {
        public static List<JsonCore.VK.MessageNew.CoreResponse> MessageNewQueue = new List<JsonCore.VK.MessageNew.CoreResponse>();

        // POST api/botigor
        [HttpPost]
        public ActionResult<string> Post(JsonCore.VK.MessageNew.CoreResponse value)
        {
            switch (value.Type)
            {
                case "confirmation":
                    switch (value.Group_Id)
                    {
                        case 178653938:
                            return AdminSettings.VK_CONFORMATION_KEY;

                        default:
                            LogCore.Log("Unknown group id", JsonConvert.SerializeObject(value), EnumData.LogTypeCommand.Error, EnumData.LogSourceCommand.VK);
                            break;
                    }

                    break;

                case "message_new":
                    lock (MessageNewQueue)
                        MessageNewQueue.Add(value);
                    break;

                default:
                    LogCore.Log("Unknown message type", JsonConvert.SerializeObject(value), EnumData.LogTypeCommand.Error, EnumData.LogSourceCommand.VK);
                    break;
            }
            return "ok";
        }
    }
}