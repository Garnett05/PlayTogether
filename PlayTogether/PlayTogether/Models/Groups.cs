namespace PlayTogether.Models
{
    public class Groups
    {
        public int id { get; set; }
        public string name { get; set; }
        public string imageUrl { get; set; }
        public string numberPlayers { get; set; }
        public string idGame { get; set; }
        public string idUserGroupLeader { get; set; }
    }
}
