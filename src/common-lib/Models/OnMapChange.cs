namespace XtremeIdiots.Portal.CommonLib.Models
{
    public class OnMapChange
    {
        public OnMapChange(string gameName, string gameType, string mapName)
        {
            GameName = gameName;
            GameType = gameType;        
            MapName = mapName;
        }

        public string GameName { get; set; }
        public string GameType { get; set; }
        public string MapName { get; set; }
    }
}
