using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Simplecast.Client.Core.Extensions;
using Simplecast.Client.Core.Responses;
using Simplecast.Client.FilterModels;
using Simplecast.Client.Models;
using Simplecast.Client.Tests.Mocks;
using Xunit;

namespace Simplecast.Client.Tests.ClientTests.StatisticClientTests
{
    public class GetEpisodeStatisticTests
    {
        private const int PodcastId = 5354;
        private const int EpisodeId = 15354;

        private static readonly EpisodeStatisticFilter EpisodeStatisticFilter = new EpisodeStatisticFilter(EpisodeId, TimeFrame.Custom, DateTime.Now.AddDays(-3), DateTime.Now);
        private static readonly string PodcastStatisticEpisodeUrl = UrlPathBuilder.GetPodcastStatisticEpisodeUrl(PodcastId);

        [Fact]
        public async Task GetEpisodeStatisticResponseAsync_Should_Throw_ArgumentException_If_PodcastId_Not_Greater_Than_Zero()
        {
            StatisticClientMock statisticClientMock = StatisticClientMock.Create();

            await Assert.ThrowsAsync<ArgumentException>(() => statisticClientMock.GetEpisodeStatisticResponseAsync(-1, EpisodeStatisticFilter));
        }

        [Fact]
        public async Task GetEpisodeStatisticResponseAsync_Should_Throw_ArgumentNullException_If_EpisodeStatisticFilter_Is_Null()
        {
            StatisticClientMock statisticClientMock = StatisticClientMock.Create();

            await Assert.ThrowsAsync<ArgumentNullException>(() => statisticClientMock.GetEpisodeStatisticResponseAsync(PodcastId, null));
        }

        [Fact]
        public async Task GetEpisodeStatisticResponseAsync_Should_Throw_ArgumentException_If_EpisodeStatisticFilter_EpisodeId_Not_Greater_Than_Zero()
        {
            StatisticClientMock statisticClientMock = StatisticClientMock.Create();

            await Assert.ThrowsAsync<ArgumentException>(() => statisticClientMock.GetEpisodeStatisticResponseAsync(PodcastId, new EpisodeStatisticFilter(-1)));
        }

        [Fact]
        public async Task GetEpisodeStatisticResponseAsync_Should_Return_ApiResponse_With_Statistic_By_Given_PodcastId_And_EpisodeStatisticFilter()
        {
            StatisticClientMock statisticClientMock = StatisticClientMock.Create();

            statisticClientMock.RestApiClientMock
                               .Setup(client => client.GetApiResponseAsync<Statistic>(
                                          It.IsAny<string>(),
                                          It.IsAny<IList<KeyValuePair<string, string>>>(),
                                          It.IsAny<IDictionary<string, string>>()))
                               .ReturnsAsync(() => new ApiResponse<Statistic>() { Model = new Statistic() });

            var queryParams = EpisodeStatisticFilter.ToQueryParams();

            ApiResponse<Statistic> apiResponse = await statisticClientMock.GetEpisodeStatisticResponseAsync(PodcastId, EpisodeStatisticFilter);

            statisticClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<Statistic>(
                                                             It.Is<string>(url => url == PodcastStatisticEpisodeUrl),
                                                             It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues.Any(pair => queryParams.Contains(pair))),
                                                             It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);

            Assert.NotNull(apiResponse);
            Assert.NotNull(apiResponse.Model);
        }

        [Fact]
        public async Task GetEpisodeStatisticResponseAsync_Should_Return_Statistic_By_Given_PodcastId_And_EpisodeStatisticFilter()
        {
            StatisticClientMock statisticClientMock = StatisticClientMock.Create();

            statisticClientMock.RestApiClientMock
                               .Setup(client => client.GetApiResponseAsync<Statistic>(
                                          It.IsAny<string>(),
                                          It.IsAny<IList<KeyValuePair<string, string>>>(),
                                          It.IsAny<IDictionary<string, string>>()))
                               .ReturnsAsync(() => new ApiResponse<Statistic>() { Model = new Statistic() });

            var queryParams = EpisodeStatisticFilter.ToQueryParams();

            Statistic statistic = await statisticClientMock.GetEpisodeStatisticAsync(PodcastId, EpisodeStatisticFilter);

            statisticClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<Statistic>(
                                                             It.Is<string>(url => url == PodcastStatisticEpisodeUrl),
                                                             It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues.Any(pair => queryParams.Contains(pair))),
                                                             It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);

            Assert.NotNull(statistic);
        }
    }
}
