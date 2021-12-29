using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Backened.Models;

namespace Backened.Data
{
    public class DataAccess
    {
        private SignalRChatContext _ctx = null;
        public DataAccess(SignalRChatContext context)
        {
            _ctx = context;
        }
        public async Task<object> addLoginDetails(Login model)
        {
            var msg = (dynamic)null;
            try
            {
                foreach (Signup val in _ctx.Signups.ToList())
                {
                    if (val.userName == model.userName && val.userPassword == model.userPassword)
                    {
                        await _ctx.Logins.AddAsync(model);
                        await _ctx.SaveChangesAsync();
                        msg = "Login Successfully!";
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
                System.Console.WriteLine(e);
                msg = null;
            }
            return msg;
        }
        public async Task<object> getLoginDetails()
        {
            var loggedUser = (dynamic)null;
            try
            {
                loggedUser = await (from value in _ctx.Logins
                                    select value).ToListAsync();
                await _ctx.SaveChangesAsync();
            }
            catch (Exception e)
            {
                e.ToString();
                System.Console.WriteLine(e);
                loggedUser = null;
            }
            return loggedUser;
        }
        public async Task<object> deleteLoginDetails(string userName)
        {
            var msg = (dynamic)null;
            try
            {
                Login retrieve = await _ctx.Logins.FirstAsync(val => val.userName == userName);
                _ctx.Logins.Remove(retrieve);
                await _ctx.SaveChangesAsync();
                msg = "Logout Successfully";
            }
            catch (Exception e)
            {
                e.ToString();
                System.Console.WriteLine(e);
                msg = null;
            }
            return msg;
        }
        public async Task<object> addSignupDetails(Signup model)
        {
            
            var msg = (dynamic)null;
            try
            {
                await _ctx.Signups.AddAsync(model);
                await _ctx.SaveChangesAsync();
                msg = "New User Added Successfully";
            }
            catch (Exception e)
            {
                e.ToString();
                System.Console.WriteLine(e);
                msg = null;
            }
            return msg;
        }

        public async Task<object> getSignupDetails()
        {
            var signup = (dynamic)null;
            try
            {
                signup = await (from value in _ctx.Signups
                                select value).ToListAsync();
                await _ctx.SaveChangesAsync();
            }
            catch (Exception e)
            {
                e.ToString();
                System.Console.WriteLine(e);
                signup = null;
            }
            return signup;
        }
        public async Task<object> updateSignupDetails(string userName, string loginStatus)
        {

            var msg = (dynamic)null;
            try
            {
                Signup retrieve = await _ctx.Signups.FirstAsync(val => val.userName == userName);
                retrieve.loginStatus = loginStatus;
                await _ctx.SaveChangesAsync();
                msg = "Updated Login Status Successfully";
            }
            catch (Exception e)
            {
                e.ToString();
                System.Console.WriteLine(e);
                msg = null;
            }
            return msg;
        }
        public async Task<object> saveChatDetails(Message _model)
        {
            string msg = string.Empty;
            try
            {
                Chat model = new Chat()
                {
                    chatId = _ctx.Chats.DefaultIfEmpty().Max(x => x == null ? 0 : x.chatId) + 1,
                    connectionId = _model.connectionId,
                    senderId = _model.senderId,
                    receiverId = _model.receiverId,
                    message = _model.message,
                    messageStatus = _model.messageStatus,
                    messageDate = _model.messageDate
                };
                _ctx.Chats.Add(model);
                await _ctx.SaveChangesAsync();
                msg = "Saved Data";
            }
            catch (Exception e)
            {
                msg = "Error : " + e.ToString();
            }
            return msg;
        }
        public Task<List<Chat>> getChatDetails(Chat model)
        {
            return Task.Run(() =>
            {
                List<Chat> chat = null;
                try
                {
                    chat = (from value in _ctx.Chats
                            where (value.senderId == model.senderId && value.receiverId == model.receiverId) || (value.senderId == model.receiverId && value.receiverId == model.senderId)
                            select value).ToList();
                    _ctx.SaveChanges();
                }
                catch (Exception e)
                {
                    e.ToString();
                    System.Console.WriteLine(e);
                    chat = null;
                }
                return chat;
            });
        }
    }
}
