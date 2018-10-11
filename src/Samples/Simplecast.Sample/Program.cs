using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Simplecast.Client;
using Simplecast.Client.Clients;
using Simplecast.Client.Contracts;
using Simplecast.Client.Core;
using Simplecast.Client.Core.Responses;
using Simplecast.Client.FilterModels;
using Simplecast.Client.Models;

namespace Simplecast.Sample
{
    internal class Program
    {
        internal static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var apiOptions = new ApiOptions("<your token>", "https://api.simplecast.com/v1/");

            var services = new ServiceCollection();
            services.AddSingleton(apiOptions);
            services.AddHttpClient<IRestApiClient, RestApiClient>();
            services.AddTransient<IPodcastClient, PodcastClient>();
            services.AddTransient<IEpisodeClient, EpisodeClient>();
            services.AddTransient<IPlayerEmbedClient, PlayerEmbedClient>();
            services.AddTransient<IStatisticClient, StatisticClient>();

            ServiceProvider buildServiceProvider = services.BuildServiceProvider();

            var podcastClient = buildServiceProvider.GetRequiredService<IPodcastClient>();
            var episodeClient = buildServiceProvider.GetRequiredService<IEpisodeClient>();
            var playerEmbedClient = buildServiceProvider.GetRequiredService<IPlayerEmbedClient>();
            var statisticClient = buildServiceProvider.GetRequiredService<IStatisticClient>();

            ApiResponse<List<Podcast>> podcastsResponse = await podcastClient.GetPodcastsResponseAsync();
            ApiResponse<Podcast> podcastResponse = await podcastClient.GetPodcastByIdResponseAsync(podcastsResponse.Model[0].Id);

            int podcastId = podcastResponse.Model.Id;

            ApiResponse<List<Episode>> episodesResponse = await episodeClient.GetEpisodesResponseAsync(podcastId);
            ApiResponse<Episode> episodeResponse = await episodeClient.GetEpisodeByIdResponseAsync(podcastId, episodesResponse.Model[0].Id);

            int episodeId = episodeResponse.Model.Id;

            ApiResponse<Embed> playerEmbedResponse = await playerEmbedClient.GetPlayerEmbedResponseAsync(podcastId, episodeId);

            ApiResponse<Statistic> podcastStatisticResponse = await statisticClient.GetPodcastStatisticResponseAsync(podcastId);

            ApiResponse<Statistic> podcastOverallStatisticResponse = await statisticClient.GetPodcastStatisticOverallResponseAsync(podcastId); // TimeFrame.Recent - default

            podcastOverallStatisticResponse = await statisticClient.GetPodcastStatisticOverallResponseAsync(podcastId, new OverallStatisticFilter(TimeFrame.All));
            podcastOverallStatisticResponse = await statisticClient.GetPodcastStatisticOverallResponseAsync(podcastId, new OverallStatisticFilter(TimeFrame.Year));
            podcastOverallStatisticResponse = await statisticClient.GetPodcastStatisticOverallResponseAsync(podcastId, new OverallStatisticFilter(TimeFrame.Custom, DateTime.Now.AddDays(-3), DateTime.Now));

            ApiResponse<Statistic> episodeStatisticResponse = await statisticClient.GetEpisodeStatisticResponseAsync(podcastId, new EpisodeStatisticFilter(episodeId)); // TimeFrame.Recent - default

            episodeStatisticResponse = await statisticClient.GetEpisodeStatisticResponseAsync(podcastId, new EpisodeStatisticFilter(episodeId, TimeFrame.All));
            episodeStatisticResponse = await statisticClient.GetEpisodeStatisticResponseAsync(podcastId, new EpisodeStatisticFilter(episodeId, TimeFrame.Year));
            episodeStatisticResponse = await statisticClient.GetEpisodeStatisticResponseAsync(podcastId, new EpisodeStatisticFilter(episodeId, TimeFrame.Custom, DateTime.Now.AddDays(-3), DateTime.Now));
        }
    }
}