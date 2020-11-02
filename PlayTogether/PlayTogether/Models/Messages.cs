using System;
using System.Collections.Generic;
using System.Text;

namespace PlayTogether.Models
{
    public class Messages
    {
        public int id { get; set; }
        public int id_group { get; set; }
        public int id_user { get; set; }
        public string message { get; set; }
    }
}
