using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace VkBotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VkBotController : ControllerBase
    {
        public static List<JsonCore.VK.MessageNew.CoreResponse> MessageNewQueue = new List<JsonCore.VK.MessageNew.CoreResponse>();

        // POST api/botigor
        [HttpPost]
        public ActionResult<string> Post(CoreResponse value)
        {
            switch (value.Type)
            {
                case "confirmation":
                    switch (value.GroupId)
                    {
                        case 178653938:
                            return AdminSettings.VK_CONFORMATION_KEY;

                        default:
                            LogCore.Log("Unknown group id", JsonConvert.SerializeObject(value), EnumData.LogTypeCommand.Error, EnumData.LogSourceCommand.VK);
                            break;
                    }

                    break;

                case "message_new":
                    var raw_response = JsonConvert.SerializeObject(value);
                    var des_response = JsonConvert.DeserializeObject<JsonCore.VK.MessageNew.CoreResponse>(raw_response);
                    lock (MessageNewQueue)
                        MessageNewQueue.Add(des_response);
                    break;

                default:
                    LogCore.Log("Unknown message type", JsonConvert.SerializeObject(value), EnumData.LogTypeCommand.Error, EnumData.LogSourceCommand.VK);
                    break;
            }
            return "ok";
        }

        public class CoreResponse
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("object")]
            public object Object { get; set; }

            [JsonProperty("group_id")]
            public int GroupId { get; set; }
        }

        public class Object
        {
            [JsonProperty("user_id")]
            public int UserId { get; set; }

            [JsonProperty("join_type")]
            public string JoinType { get; set; }
        }
    }
}