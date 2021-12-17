using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryFunc
{
    public class ChatMessageRepository
    {
        public PortalDbContext Context { get; }

        public ChatMessageRepository(PortalDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [FunctionName("CreateChatMessage")]
        public async Task<IActionResult> CreateChatMessage([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            ChatMessage chatMessage;
            try
            {
                chatMessage = JsonConvert.DeserializeObject<ChatMessage>(requestBody);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }

            await Context.ChatMessages.AddAsync(chatMessage);
            await Context.SaveChangesAsync();

            return new OkObjectResult(chatMessage);
        }
    }
}
