namespace XtremeIdiots.Portal.CommonLib.Models
{
    public class OnEventBase
    {
        public DateTime EventGeneratedUtc { get; set; }
        public string? GameType { get; set; }
        public string? ServerId { get; set; }
    }
}
