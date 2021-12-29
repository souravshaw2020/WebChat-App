using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backened.Models
{
    public partial class Chat
    {
        public long chatId { get; set; }
        public string connectionId { get; set; }
        public string senderId { get; set; }
        public string receiverId { get; set; }
        public string message { get; set; }
        public string messageStatus { get; set; }
        public DateTime? messageDate { get; set; }
        public bool? isPrivate { get; set; }
    }
}
