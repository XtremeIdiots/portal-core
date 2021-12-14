﻿namespace XtremeIdiots.Portal.DataLib.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        public string GameType { get; set; }
        public string Username { get; set; }
        public string Guid { get; set; }
        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }
        public string IpAddress { get; set; }
    }
}