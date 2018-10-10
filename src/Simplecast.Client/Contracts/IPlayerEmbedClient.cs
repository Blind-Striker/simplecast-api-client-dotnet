using System.Threading.Tasks;
using Simplecast.Client.Core.Responses;
using Simplecast.Client.Models;

namespace Simplecast.Client.Contracts
{
    public interface IPlayerEmbedClient
    {
        Task<ApiResponse<Embed>> GetPlayerEmbedResponseAsync(int podcastId, int episodeId);

        Task<ApiResponse<Embed>> GetPlayerEmbedAsync(int podcastId, int episodeId);
    }
}