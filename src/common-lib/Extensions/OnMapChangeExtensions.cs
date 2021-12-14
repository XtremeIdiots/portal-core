using XtremeIdiots.Portal.CommonLib.Models;

namespace XtremeIdiots.Portal.CommonLib.Extensions
{
    public static class OnMapChangeExtensions
    {
        public static bool IsValid(this OnMapChange onMapChangeEvent)
        {
            if (string.IsNullOrWhiteSpace(onMapChangeEvent.GameName))
                return false;

            if (string.IsNullOrWhiteSpace(onMapChangeEvent.MapName))
                return false;

            return true;
        }

        public static bool IsEventStale(this OnMapChange onMapChangeEvent)
        {
            if (DateTime.UtcNow - onMapChangeEvent.EventGeneratedUtc > TimeSpan.FromMinutes(1))
                return true;

            return false;
        }
    }
}
