namespace XtremeIdiots.Portal.DataLib.Models
{
    public class Player
    {
        public Player(string gameType, string guid, string username)
        {
            Id = System.Guid.NewGuid();
            GameType = gameType;
            Username = username;
            Guid = guid;
            FirstSeen = DateTime.UtcNow;
            LastSeen = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public string GameType { get; set; }
        public string Username { get; set; }
        public string Guid { get; set; }
        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }
        public string? IpAddress { get; set; }
    }
}
