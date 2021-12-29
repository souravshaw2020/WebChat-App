using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backened.Models
{
    public partial class Login
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
    }
}
