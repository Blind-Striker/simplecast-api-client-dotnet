using System.Collections.Generic;
using System.Threading.Tasks;
using Simplecast.Client.Contracts;
using Simplecast.Client.Core.Helpers;
using Simplecast.Client.Core.Responses;
using Simplecast.Client.Models;

namespace Simplecast.Client.Clients
{
    public class PodcastClient : IPodcastClient
    {
        private readonly IRestApiClient _restApiClient;

        public PodcastClient(IRestApiClient restApiClient)
        {
            _restApiClient = restApiClient;
        }

        public async Task<ApiResponse<List<Podcast>>> GetPodcastsResponseAsync()
        {
            ApiResponse<List<Podcast>> apiResponse =
                await _restApiClient.GetApiResponseAsync<List<Podcast>>(UrlPathBuilder.PodcastUrl);

            return apiResponse;
        }

        public async Task<ApiResponse<Podcast>> GetPodcastByIdResponseAsync(int id)
        {
            ApiResponse<Podcast> apiResponse =
                await _restApiClient.GetApiResponseAsync<Podcast>(UrlPathBuilder.GetPodcastByIdUrl(id));

            return apiResponse;
        }

        public async Task<List<Podcast>> GetPodcastsAsync()
        {
            ApiResponse<List<Podcast>> apiResponse = await GetPodcastsResponseAsync();

            return apiResponse.GetModel();
        }

        public async Task<Podcast> GetPodcastByIdAsync(int id)
        {
            ApiResponse<Podcast> apiResponse = await GetPodcastByIdResponseAsync(id);

            return apiResponse.GetModel();
        }
    }
}