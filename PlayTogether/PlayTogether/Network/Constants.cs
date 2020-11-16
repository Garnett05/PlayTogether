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
            //return "http://192.168.1.59:3000/games";
            return "https://flipperapp.azurewebsites.net/api/games";
        }
        public static string GetAllGroups()
        {
            ///return "http://192.168.1.59:3000/groups";
            return "https://flipperapp.azurewebsites.net/api/groups";
        }
        public static string DeleteGroup(int id)
        {
            //return $"http://192.168.1.59:3000/groups/{id}";
            return $"https://flipperapp.azurewebsites.net/api/groups/{id}";
        }
        public static string GetAllUsers()
        {
            //return "http://192.168.1.59:3000/users";
            return "https://flipperapp.azurewebsites.net/api/users";
        }
        public static string GetUserById(int user)
        {
            //return $"http://192.168.1.59:3000/users/{user}";
            return $"https://flipperapp.azurewebsites.net/api/users/{user}";
        }
        public static string DeleteGame(int id)
        {
            return $"https://flipperapp.azurewebsites.net/api/games/{id}";
        }
        public static string GetAllGroupsxUsers()
        {
            //return "http://192.168.1.59:3000/groupsxusers";
            return "https://flipperapp.azurewebsites.net/api/groupsxusers";
        }
        public static string DeleteGroupxUser(int id)
        {
            //return $"http://192.168.1.59:3000/groupsxusers/{id}";
            return $"https://flipperapp.azurewebsites.net/api/groupsxusers/{id}";
        }
        public static string GetIcons()
        {
            //return "http://192.168.1.59:3000/gamesIcons";
            return "https://flipperapp.azurewebsites.net/api/iconsgroup";
        }
        public static string GetUsersIcons()
        {
            //return "http://192.168.1.59:3000/usersIcons";
            return "https://flipperapp.azurewebsites.net/api/iconsuser";
        }
        public static string GetMessages()
        {
            //return "http://192.168.1.59:3000/messages";
            return "https://flipperapp.azurewebsites.net/api/messages";
        }
    }
}