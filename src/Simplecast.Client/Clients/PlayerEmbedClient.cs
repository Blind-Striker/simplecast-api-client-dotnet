using System.Threading.Tasks;
using Simplecast.Client.Contracts;
using Simplecast.Client.Core;
using Simplecast.Client.Core.Helpers;
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
            Ensure.GreaterThanZero(podcastId, nameof(podcastId));
            Ensure.GreaterThanZero(episodeId, nameof(episodeId));

            ApiResponse<Embed> apiResponse = await _restApiClient.GetApiResponseAsync<Embed>(UrlPathBuilder.GetPlayerEmbedUrl(podcastId, episodeId));

            return apiResponse;
        }

        public async Task<Embed> GetPlayerEmbedAsync(int podcastId, int episodeId)
        {
            ApiResponse<Embed> apiResponse = await GetPlayerEmbedResponseAsync(podcastId, episodeId);

            return apiResponse.GetModel();
        }
    }
}