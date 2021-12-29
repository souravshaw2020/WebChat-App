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
    public class SignupController : ControllerBase
    {
        private DataAccess _objSignup = null;
        private object resdata = null;
        private object result = null;
        public SignupController()
        {
            this._objSignup = new DataAccess(new SignalRChatContext());
        }
        [HttpPost("register")]
        public async Task<object> signup([FromBody]Signup model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest();
                }
                //Signup
                resdata = await _objSignup.addSignupDetails(model);
            }
            catch (Exception e)
            {
                e.ToString();
                System.Console.WriteLine(e);
            }
            result = new
            {
                resdata
            };
            return result;
        }
        [HttpGet("getuser")]
        public async Task<object> getSignup()
        {
            try
            {
                //Get New User
                resdata = await _objSignup.getSignupDetails();
            }
            catch (Exception e)
            {
                e.ToString();
                System.Console.WriteLine(e);
            }
            result = new
            {
                resdata
            };
            return result;
        }
        [HttpPut("update/{userName}={loginStatus}")]
        public async Task<object> updateSignup(string userName, string loginStatus)
        {
            try
            {
                //Update Login Status
                resdata = await _objSignup.updateSignupDetails(userName, loginStatus);
            }
            catch(Exception e)
            {
                e.ToString();
                System.Console.WriteLine(e);
            }
            result = new
            {
                resdata
            };
            return result;
        }
    }
}
