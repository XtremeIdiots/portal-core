using RestSharp;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryApiClient.ChatMessagesApi;

public class ChatMessagesApiClient : BaseApiClient, IChatMessagesApiClient
{
    public ChatMessagesApiClient(string apimBaseUrl, string apimSubscriptionKey)
        : base(apimBaseUrl, apimSubscriptionKey)
    {
    }

    public async Task CreateChatMessage(string accessToken, ChatMessage chatMessage)
    {
        var request = CreateRequest("repository/chat-messages", Method.Post, accessToken);
        request.AddJsonBody(new List<ChatMessage> {chatMessage});

        await ExecuteAsync(request);
    }
}