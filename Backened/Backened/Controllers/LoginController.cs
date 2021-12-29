using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
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
    public class LoginController : ControllerBase
    {
        private DataAccess _objSignin = null;
        private object resdata = null;
        private object result = null;

    public LoginController()
        {
            this._objSignin = new DataAccess(new SignalRChatContext());
        }
        [HttpPost("signin")]
        public async Task<object> login([FromBody] Login model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest();
                }
                //Login
                resdata = await _objSignin.addLoginDetails(model);
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
        [HttpGet("loggeduser")]
        public async Task<object> getlogin()
        {
            try
            {
                //Get Logged User
                resdata = await _objSignin.getLoginDetails();
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
        [HttpDelete("deleteduser/{userName}")]
        public async Task<object> deleteLogin(string userName)
        {
            try
            {
                //Delete Logged User
                resdata = await _objSignin.deleteLoginDetails(userName);
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
