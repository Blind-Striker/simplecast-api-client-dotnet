using System.Collections.Generic;
using System.Threading.Tasks;
using Simplecast.Client.Contracts;
using Simplecast.Client.Core;
using Simplecast.Client.Core.Helpers;
using Simplecast.Client.Core.Responses;
using Simplecast.Client.Models;

namespace Simplecast.Client.Clients
{
    public class EpisodeClient : IEpisodeClient
    {
        private readonly IRestApiClient _restApiClient;

        public EpisodeClient(IRestApiClient restApiClient)
        {
            _restApiClient = restApiClient;
        }

        public async Task<ApiResponse<List<Episode>>> GetEpisodesResponseAsync(int podcastId)
        {
            Ensure.GreaterThanZero(podcastId, nameof(podcastId));

            ApiResponse<List<Episode>> apiResponse = await _restApiClient.GetApiResponseAsync<List<Episode>>(UrlPathBuilder.GetEpisodesUrl(podcastId));

            return apiResponse;
        }

        public async Task<ApiResponse<Episode>> GetEpisodeByIdResponseAsync(int podcastId, int episodeId)
        {
            Ensure.GreaterThanZero(podcastId, nameof(podcastId));
            Ensure.GreaterThanZero(episodeId, nameof(episodeId));

            ApiResponse<Episode> apiResponse = await _restApiClient.GetApiResponseAsync<Episode>(UrlPathBuilder.GetEpisodeByIdUrl(podcastId, episodeId));

            return apiResponse;
        }

        public async Task<List<Episode>> GetEpisodesAsync(int podcastId)
        {
            ApiResponse<List<Episode>> apiResponse = await GetEpisodesResponseAsync(podcastId);

            return apiResponse.GetModel();
        }

        public async Task<Episode> GetEpisodeByIdAsync(int podcastId, int episodeId)
        {
            ApiResponse<Episode> apiResponse = await GetEpisodeByIdResponseAsync(podcastId, episodeId);

            return apiResponse.GetModel();
        }
    }
}