using System.Collections.Generic;
using System.Threading.Tasks;
using Simplecast.Client.Core.Responses;
using Simplecast.Client.Models;

namespace Simplecast.Client.Contracts
{
    public interface IPodcastClient
    {
        Task<ApiResponse<List<Podcast>>> GetPodcastsResponseAsync();

        Task<ApiResponse<Podcast>> GetPodcastByIdResponseAsync(int podcastId);

        Task<List<Podcast>> GetPodcastsAsync();

        Task<Podcast> GetPodcastByIdAsync(int id);
    }
}