using System.ComponentModel.DataAnnotations;

namespace XtremeIdiots.Portal.MgmtWeb.ViewModels;

public class GameServerViewModel
{
    [StringLength(50)] public string Id { get; set; }

    [Required] [StringLength(50)] public string Title { get; set; }

    [Required] [StringLength(50)] public string GameType { get; set; }

    [Required] [StringLength(50)] public string IpAddress { get; set; }

    public int QueryPort { get; set; }

    // Game Server Secrets
    public string? RconPassword { get; set; }
    public string? FtpUsername { get; set; }
    public string? FtpPassword { get; set; }
}