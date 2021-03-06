using Microsoft.Extensions.Options;
using RestSharp;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryApiClient.ChatMessagesApi;

public class ChatMessagesApiClient : BaseApiClient, IChatMessagesApiClient
{
    public ChatMessagesApiClient(IOptions<RepositoryApiClientOptions> options) : base(options)
    {
    }

    public async Task CreateChatMessage(string accessToken, ChatMessage chatMessage)
    {
        var request = CreateRequest("repository/chat-messages", Method.Post, accessToken);
        request.AddJsonBody(new List<ChatMessage> {chatMessage});

        await ExecuteAsync(request);
    }
}