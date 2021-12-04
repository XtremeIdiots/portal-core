namespace XtremeIdiots.Portal.CommonLib.Models
{
    public class OnPlayerConnected
    {
        public OnPlayerConnected(string username, string guid)
        {
            Username = username;
            Guid = guid;
        }

        public string Username { get; set; }
        public string Guid { get; set; }
    }
}