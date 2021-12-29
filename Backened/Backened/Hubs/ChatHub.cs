using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Backened.Data;
using Backened.Models;

namespace Backened.Hubs
{
    public class ChatHub : Hub
    {
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
        public DataAccess _objData = null;
        public async Task sendMessage(Message _message)
        {
            //Receive Message
            List<string> receiverConnectionIds = _connections.getConnections(_message.receiverId).ToList<string>();
            if (receiverConnectionIds.Count() > 0)
            {
                //Save Received Message
                try
                {
                    _objData = new DataAccess(new SignalRChatContext());
                    _message.isPrivate = true;
                    _message.connectionId = String.Join(",", receiverConnectionIds);
                    await _objData.saveChatDetails(_message);
                    await Clients.Clients(receiverConnectionIds).SendAsync("ReceiveMessage", _message);
                }
                catch (Exception)
                { }
            }
        }
        public override async Task OnConnectedAsync()
        {
            var httpCtx = Context.GetHttpContext();
            if (httpCtx != null)
            {
                try
                {
                    //Add Logged User
                    var userName = httpCtx.Request.Query["user"].ToString();
                    var connId = Context.ConnectionId.ToString();
                    _connections.Add(userName, connId);
                    //Update Client
                    await Clients.All.SendAsync("UpdateUserList", _connections.toJson());
                }
                catch (Exception)
                { }
            }
        }
        public override async Task OnDisconnectedAsync(Exception e)
        {
            var httpCtx = Context.GetHttpContext();
            if (httpCtx != null)
            {
                //Remove Logged User
                var userName = httpCtx.Request.Query["user"];
                _connections.Remove(userName, Context.ConnectionId);
                //Update Client
                await Clients.All.SendAsync("UpdateUserList", _connections.toJson());
            }
        }
    }
}
