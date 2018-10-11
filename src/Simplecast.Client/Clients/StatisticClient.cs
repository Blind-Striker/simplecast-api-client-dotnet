using System.Threading.Tasks;
using Simplecast.Client.Contracts;
using Simplecast.Client.Core;
using Simplecast.Client.Core.Extensions;
using Simplecast.Client.Core.Helpers;
using Simplecast.Client.Core.Responses;
using Simplecast.Client.FilterModels;
using Simplecast.Client.Models;

namespace Simplecast.Client.Clients
{
    public class StatisticClient : IStatisticClient
    {
        private readonly IRestApiClient _restApiClient;

        public StatisticClient(IRestApiClient restApiClient)
        {
            _restApiClient = restApiClient;
        }

        public async Task<ApiResponse<Statistic>> GetPodcastStatisticResponseAsync(int podcastId)
        {
            Ensure.GreaterThanZero(podcastId, nameof(podcastId));

            ApiResponse<Statistic> apiResponse = await _restApiClient.GetApiResponseAsync<Statistic>(UrlPathBuilder.GetPodcastStatisticUrl(podcastId));

            return apiResponse;
        }

        public async Task<ApiResponse<Statistic>> GetPodcastStatisticOverallResponseAsync(
            int podcastId, OverallStatisticFilter overallStatisticFilter = null)
        {
            Ensure.GreaterThanZero(podcastId, nameof(podcastId));

            ApiResponse<Statistic> apiResponse = await _restApiClient.GetApiResponseAsync<Statistic>(
                                                     UrlPathBuilder.GetPodcastStatisticOverallUrl(podcastId),
                                                     overallStatisticFilter?.ToQueryParams());

            return apiResponse;
        }

        public async Task<ApiResponse<Statistic>> GetEpisodeStatisticResponseAsync(
            int podcastId, EpisodeStatisticFilter episodeStatisticFilter)
        {
            Ensure.GreaterThanZero(podcastId, nameof(podcastId));
            Ensure.ArgumentNotNull(episodeStatisticFilter, nameof(episodeStatisticFilter));
            Ensure.GreaterThanZero(episodeStatisticFilter.EpisodeId, nameof(episodeStatisticFilter.EpisodeId));

            ApiResponse<Statistic> apiResponse = await _restApiClient.GetApiResponseAsync<Statistic>(
                                                     UrlPathBuilder.GetPodcastStatisticEpisodeUrl(podcastId),
                                                     episodeStatisticFilter.ToQueryParams());

            return apiResponse;
        }

        public async Task<Statistic> GetPodcastStatisticAsync(int podcastId)
        {
            ApiResponse<Statistic> apiResponse = await GetPodcastStatisticResponseAsync(podcastId);

            return apiResponse.GetModel();
        }

        public async Task<Statistic> GetPodcastStatisticOverallAsync(int podcastId, OverallStatisticFilter overallStatisticFilter = null)
        {
            ApiResponse<Statistic> apiResponse = await GetPodcastStatisticOverallResponseAsync(podcastId, overallStatisticFilter);

            return apiResponse.GetModel();
        }

        public async Task<Statistic> GetEpisodeStatisticAsync(int podcastId, EpisodeStatisticFilter overallStatisticFilter)
        {
            ApiResponse<Statistic> apiResponse = await GetEpisodeStatisticResponseAsync(podcastId, overallStatisticFilter);

            return apiResponse.GetModel();
        }
    }
}