using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;
using Backened.Data;
using Backened.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backened.Controllers
{
    [Route("api/[controller]"), Produces("application/json"), EnableCors("_allowSpecificOrigins")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private DataAccess _objChat = null;
        private object resdata = null;
        private object result = null;
        public ChatController()
        {
            this._objChat = new DataAccess(new SignalRChatContext());
        }
        [HttpGet("getchat")]
        public async Task<object> userChat([FromQuery] string param)
        {
            try
            {
                if (param != string.Empty)
                {
                    dynamic data = JsonConvert.DeserializeObject(param);
                    Chat model = JsonConvert.DeserializeObject<Chat>(data.ToString());
                    if (model != null)
                    {
                        resdata = await _objChat.getChatDetails(model);
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            result = new
            {
                resdata
            };
            return result;
        }
    }
}
