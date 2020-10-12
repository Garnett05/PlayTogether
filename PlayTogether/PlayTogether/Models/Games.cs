using System;
using System.Collections.Generic;
using System.Text;

namespace PlayTogether.Models
{
    public class Games
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string imageUrl { get; set; }
        public string minPlayers { get; set; }
        public string maxPlayers { get; set; }
    }
}
