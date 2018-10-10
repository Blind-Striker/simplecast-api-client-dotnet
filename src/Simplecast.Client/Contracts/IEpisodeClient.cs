using System.Collections.Generic;
using System.Threading.Tasks;
using Simplecast.Client.Core.Responses;
using Simplecast.Client.Models;

namespace Simplecast.Client.Contracts
{
    public interface IEpisodeClient
    {
        Task<ApiResponse<List<Episode>>> GetEpisodesResponseAsync(int podcastId);

        Task<ApiResponse<Episode>> GetEpisodeByIdResponseAsync(int podcastId, int episodeId);

        Task<List<Episode>> GetEpisodesAsync(int podcastId);

        Task<Episode> GetEpisodeByIdAsync(int podcastId, int episodeId);
    }
}