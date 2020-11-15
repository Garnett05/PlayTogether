using System;
using System.Collections.Generic;
using System.Text;

namespace PlayTogether.Models
{
    public class Users
    {
        public int id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public string nickname { get; set; }
        public string email { get; set; }
        public string psw { get; set; }
        public string imageUrl { get; set; }
    }
}
