using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThriffSignUp.Model
{
    public class Account
    {
        public int sellerid { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string address { get; set; }
    }
}
