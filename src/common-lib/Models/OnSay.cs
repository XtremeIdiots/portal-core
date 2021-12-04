namespace XtremeIdiots.Portal.CommonLib.Models
{
    public class OnSay
    {
        public OnSay(string username, string guid, string message, string type)
        {
            Username = username;
            Guid = guid;
            Message = message;
            Type = type;
        }

        public string Username { get; set; }
        public string Guid { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
    }
}
