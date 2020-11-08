using System;
using System.Collections.Generic;
using System.Text;

namespace PlayTogether.Network
{
    public static class Constants
    {
        public static string GetGameById(string gameId)
        {
            //return $"http://192.168.1.59:3000/games/{gameId}";
            return "https://localhost:44311/api/games/{gameId}";
        }
        public static string GetAllGames()
        {
            return "http://192.168.1.59:3000/games";
            //return "https://flipperapp.azurewebsites.net/api/games";
        }
        public static string GetAllGroups()
        {            
            return "http://192.168.1.59:3000/groups";
        }
        public static string DeleteGroup(int id)
        {
            return $"http://192.168.1.59:3000/groups/{id}";
        }
        public static string GetAllUsers()
        {
            return "http://192.168.1.59:3000/users";
        }
        public static string GetUserById(string user)
        {
            return $"http://192.168.1.59:3000/users/{user}";
        }
        public static string DeleteGame(int id)
        {
            return $"https://flipperapp.azurewebsites.net/api/games/{id}";
        }
        public static string GetAllGroupsxUsers()
        {
            return "http://192.168.1.59:3000/groupsxusers";
        }
        public static string DeleteGroupxUser(int id)
        {
            return $"http://192.168.1.59:3000/groupsxusers/{id}";
        }
        public static string GetIcons()
        {
            return "http://192.168.1.59:3000/gamesIcons";
        }
        public static string GetUsersIcons()
        {
            return "http://192.168.1.59:3000/usersIcons";
        }
        public static string GetMessages()
        {
            return "http://192.168.1.59:3000/messages";
        }
    }
}