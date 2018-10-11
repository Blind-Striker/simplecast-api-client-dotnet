using System.Net.Http;
using Simplecast.Client.Clients;
using Simplecast.Client.Contracts;
using Simplecast.Client.Core;

namespace Simplecast.Client.Standalone
{
    public class SimplecastClientStandalone : ISimplecastClientContext
    {
        public SimplecastClientStandalone(IEpisodeClient episodeClient, IPlayerEmbedClient playerEmbedClient, IPodcastClient podcastClient, IStatisticClient statisticClient)
        {
            EpisodeClient = episodeClient;
            PlayerEmbedClient = playerEmbedClient;
            PodcastClient = podcastClient;
            StatisticClient = statisticClient;
        }

        public IEpisodeClient EpisodeClient { get; }
        public IPlayerEmbedClient PlayerEmbedClient { get; }
        public IPodcastClient PodcastClient { get; }
        public IStatisticClient StatisticClient { get; }

        public static ISimplecastClientContext Create(string baseUrl, string apiKey, HttpClient httpClient = null)
        {
            return Create(new ApiOptions(apiKey, baseUrl), httpClient);
        }

        public static ISimplecastClientContext Create(ApiOptions apiOptions, HttpClient httpClient = null)
        {
            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }

            IRestApiClient restApiClient = new RestApiClient(httpClient, apiOptions);
            ISimplecastClientContext apiClientContext = new SimplecastClientStandalone(
                new EpisodeClient(restApiClient),
                new PlayerEmbedClient(restApiClient),
                new PodcastClient(restApiClient),
                new StatisticClient(restApiClient));

            return apiClientContext;
        }
    }
}
