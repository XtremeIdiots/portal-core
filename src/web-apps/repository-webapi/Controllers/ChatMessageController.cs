﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryWebApi.Controllers;

[ApiController]
[Authorize(Roles = "ServiceAccount,MgmtWebAdminUser")]
[Route("api/ChatMessage")]
public class ChatMessageController : ControllerBase
{
    public ChatMessageController(PortalDbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public PortalDbContext Context { get; }

    [HttpPost(Name = "ChatMessage")]
    public async Task<IActionResult> CreateChatMessage()
    {
        var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();

        ChatMessage chatMessage;
        try
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            chatMessage = JsonConvert.DeserializeObject<ChatMessage>(requestBody);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex);
        }

        if (chatMessage == null) return new BadRequestResult();

        await Context.ChatMessages.AddAsync(chatMessage);
        await Context.SaveChangesAsync();

        return new OkObjectResult(chatMessage);
    }
}