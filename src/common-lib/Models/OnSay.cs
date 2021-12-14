namespace XtremeIdiots.Portal.CommonLib.Models
{
    public class OnSay : OnEventBase
    {
        public string? Username { get; set; }
        public string? Guid { get; set; }
        public string? Message { get; set; }
        public string? Type { get; set; }
    }
}
