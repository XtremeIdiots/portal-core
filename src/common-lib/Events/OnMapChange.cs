using XtremeIdiots.Portal.CommonLib.Events;

namespace XtremeIdiots.Portal.CommonLib.Models
{
    public class OnMapChange : OnEventBase
    {
        public string? GameName { get; set; }
        public string? MapName { get; set; }
    }
}
