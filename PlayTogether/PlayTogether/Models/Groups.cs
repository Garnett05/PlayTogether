namespace PlayTogether.Models
{
    public class Groups
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image_url { get; set; }
        public string numberPlayer { get; set; }
        public string idGame { get; set; }
        public string idUserGroupLeader { get; set; }
    }
}
