using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Simplecast.Client;
using Simplecast.Client.Clients;
using Simplecast.Client.Contracts;
using Simplecast.Client.Core;
using Simplecast.Client.FilterModels;

namespace Simplecast.Sample
{
    class Program
    {
        internal static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ApiOptions apiOptions = new ApiOptions("sc_NkZ1SRPUu_JaUgHrlKj1pg", "https://api.simplecast.com/v1/");

            var services = new ServiceCollection();
            services.AddSingleton(apiOptions);
            services.AddHttpClient<IRestApiClient, RestApiClient>();
            services.AddTransient<IPodcastClient, PodcastClient>();
            services.AddTransient<IEpisodeClient, EpisodeClient>();
            services.AddTransient<IPlayerEmbedClient, PlayerEmbedClient>();
            services.AddTransient<IStatisticClient, StatisticClient>();

            var buildServiceProvider = services.BuildServiceProvider();

            var podcastClient = buildServiceProvider.GetRequiredService<IPodcastClient>();
            var episodeClient = buildServiceProvider.GetRequiredService<IEpisodeClient>();
            var playerEmbedClient = buildServiceProvider.GetRequiredService<IPlayerEmbedClient>();
            var statisticClient = buildServiceProvider.GetRequiredService<IStatisticClient>();

            var podcastsResponse = await podcastClient.GetPodcastsResponseAsync();
            var podcastResponse = await podcastClient.GetPodcastByIdResponseAsync(podcastsResponse.Model[0].Id);

            var podcastId = podcastResponse.Model.Id;

            var episodesResponse = await episodeClient.GetEpisodesResponseAsync(podcastId);
            var episodeResponse = await episodeClient.GetEpisodeByIdResponseAsync(podcastId, episodesResponse.Model[0].Id);

            var episodeId = episodeResponse.Model.Id;

            var playerEmbedResponse = await playerEmbedClient.GetPlayerEmbedResponseAsync(podcastId, episodeId);

            var podcastStatisticResponse = await statisticClient.GetPodcastStatisticResponseAsync(podcastId);

            var podcastOverallStatisticResponse = await statisticClient.GetPodcastStatisticOverallResponseAsync(podcastId); // TimeFrame.Recent - default

            podcastOverallStatisticResponse = await statisticClient.GetPodcastStatisticOverallResponseAsync(podcastId, new OverallStatisticFilter(TimeFrame.All));
            podcastOverallStatisticResponse = await statisticClient.GetPodcastStatisticOverallResponseAsync(podcastId, new OverallStatisticFilter(TimeFrame.Year));
            podcastOverallStatisticResponse = await statisticClient.GetPodcastStatisticOverallResponseAsync(podcastId, new OverallStatisticFilter(TimeFrame.Custom, DateTime.Now, DateTime.Now.AddDays(-3)));

            var episodeStatisticResponse = await statisticClient.GetEpisodeStatisticResponseAsync(podcastId, new EpisodeStatisticFilter(episodeId)); // TimeFrame.Recent - default

            episodeStatisticResponse = await statisticClient.GetEpisodeStatisticResponseAsync(podcastId, new EpisodeStatisticFilter(episodeId, TimeFrame.All));
            episodeStatisticResponse = await statisticClient.GetEpisodeStatisticResponseAsync(podcastId, new EpisodeStatisticFilter(episodeId, TimeFrame.Year));
            episodeStatisticResponse = await statisticClient.GetEpisodeStatisticResponseAsync(podcastId, new EpisodeStatisticFilter(episodeId, TimeFrame.Custom, DateTime.Now, DateTime.Now.AddDays(-3)));
        }
    }
}
