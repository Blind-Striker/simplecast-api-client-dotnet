using System.Threading.Tasks;
using Simplecast.Client.Core.Responses;
using Simplecast.Client.FilterModels;
using Simplecast.Client.Models;

namespace Simplecast.Client.Contracts
{
    public interface IStatisticClient
    {
        Task<ApiResponse<Statistic>> GetPodcastStatisticResponseAsync(int podcastId);
        Task<ApiResponse<Statistic>> GetPodcastStatisticOverallResponseAsync(int podcastId, OverallStatisticFilter overallStatisticFilter = null);
        Task<ApiResponse<Statistic>> GetEpisodeStatisticResponseAsync(int podcastId, EpisodeStatisticFilter episodeStatisticFilter);
        Task<Statistic> GetPodcastStatisticAsync(int podcastId);
        Task<Statistic> GetPodcastStatisticOverallAsync(int podcastId, OverallStatisticFilter overallStatisticFilter = null);
        Task<Statistic> GetEpisodeStatisticAsync(int podcastId, EpisodeStatisticFilter overallStatisticFilter);
    }
}