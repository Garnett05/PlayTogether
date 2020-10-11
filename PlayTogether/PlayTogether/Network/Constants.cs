﻿using System;
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
            return "http://192.168.1.63:3000/games";
            //return "https://flipperapp.azurewebsites.net/api/games";
        }
        public static string GetAllGroups()
        {            
            return "http://192.168.1.63:3000/groups";
        }
        public static string GetAllUsers()
        {
            return "http://192.168.1.63:3000/users";
        }
        public static string GetUserById(string user)
        {
            return $"http://192.168.1.63:3000/users/{user}";
        }
        public static string DeleteGame(int id)
        {
            return $"https://flipperapp.azurewebsites.net/api/games/{id}";
        }
    }
}