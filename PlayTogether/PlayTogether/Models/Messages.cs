using System;
using System.Collections.Generic;
using System.Text;

namespace PlayTogether.Models
{
    public class Messages
    {
        public int IdMsg { get; set; }
        public int IdGroup { get; set; }
        public int IdUser { get; set; }
        public string Msg { get; set; }
    }
}
