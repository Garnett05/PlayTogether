using System;
using System.Collections.Generic;
using System.Text;

namespace PlayTogether.Network
{
    public static class Constants
    {
        public static string GetGameById(string gameId)
        {
            //return $"http://192.168.1.63:3000/games/{gameId}";
            return "https://localhost:44311/api/games/{gameId}";
        }
        public static string GetAllGames()
        {
        //return "https://localhost:44311/api/games";
        return "http://192.168.1.63:3000/games";
        }
        public static string GetAllGroups()
        {            
            return "http://192.168.1.63:3000/groups";
        }
    }
}