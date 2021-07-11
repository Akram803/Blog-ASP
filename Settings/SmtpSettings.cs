using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Settings
{
    public class SmtpSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Server { get; set; }
        public int Port { get; set; }
    }
}
