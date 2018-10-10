using System.Threading.Tasks;
using Simplecast.Client.Contracts;
using Simplecast.Client.Core.Responses;
using Simplecast.Client.Models;

namespace Simplecast.Client.Clients
{
    public class PlayerEmbedClient : IPlayerEmbedClient
    {
        private readonly IRestApiClient _restApiClient;

        public PlayerEmbedClient(IRestApiClient restApiClient)
        {
            _restApiClient = restApiClient;
        }

        public async Task<ApiResponse<Embed>> GetPlayerEmbedResponseAsync(int podcastId, int episodeId)
        {
            ApiResponse<Embed> apiResponse =
                await _restApiClient.GetApiResponseAsync<Embed>(UrlPathBuilder.GetPlayerEmbedUrl(podcastId, episodeId));

            return apiResponse;
        }

        public async Task<ApiResponse<Embed>> GetPlayerEmbedAsync(int podcastId, int episodeId)
        {
            ApiResponse<Embed> apiResponse = await GetPlayerEmbedResponseAsync(podcastId, episodeId);

            return apiResponse;
        }
    }
}