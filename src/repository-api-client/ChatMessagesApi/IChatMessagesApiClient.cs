using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryApiClient.ChatMessagesApi;

public interface IChatMessagesApiClient
{
    Task CreateChatMessage(string accessToken, ChatMessage chatMessage);
}