﻿using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryApiClient.PlayersApi;

public interface IPlayersApiClient
{
    Task<Player?> GetPlayer(string accessToken, string gameType, string guid);
    Task CreatePlayer(string accessToken, Player player);
    Task UpdatePlayer(string accessToken, Player player);
    Task CreateChatMessage(string accessToken, ChatMessage chatMessage);
}