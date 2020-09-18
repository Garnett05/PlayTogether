using System;
using System.Collections.Generic;
using System.Text;

namespace PlayTogether.Network
{
    public static class Constants
    {
        public static string GetGameById(string gameId)
        {
            return $"http://192.168.1.63:3000/games/{gameId}";
        }
        public static string GetAllGames()
        {            
            return "http://192.168.1.63:3000/games";
        }
    }
}