using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryWebApi.Controllers;

[ApiController]
[Authorize(Roles = "ServiceAccount,MgmtWebAdminUser")]
public class ChatMessagesController : ControllerBase
{
    public ChatMessagesController(PortalDbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public PortalDbContext Context { get; }

    [HttpPost]
    [Route("api/chat-messages")]
    public async Task<IActionResult> CreateChatMessage()
    {
        var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();

        List<ChatMessage> chatMessages;
        try
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            chatMessages = JsonConvert.DeserializeObject<List<ChatMessage>>(requestBody);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex);
        }

        if (chatMessages == null) return new BadRequestResult();

        await Context.ChatMessages.AddRangeAsync(chatMessages);
        await Context.SaveChangesAsync();

        return new OkObjectResult(chatMessages);
    }
}