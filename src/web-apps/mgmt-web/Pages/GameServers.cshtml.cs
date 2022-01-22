using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace XtremeIdiots.Portal.MgmtWeb.Pages;

[Authorize(Roles = "ApplicationUser")]
public class GameServersModel : PageModel
{
    public void OnGet()
    {
    }
}