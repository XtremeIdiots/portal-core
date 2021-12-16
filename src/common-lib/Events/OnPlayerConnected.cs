using XtremeIdiots.Portal.CommonLib.Events;

namespace XtremeIdiots.Portal.CommonLib.Models
{
    public class OnPlayerConnected : OnEventBase
    {
        public string? Username { get; set; }
        public string? Guid { get; set; }
        public string? IpAddress { get; set; }
    }
}