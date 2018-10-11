using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Simplecast.Client.Core.Responses;
using Simplecast.Client.Models;
using Simplecast.Client.Tests.Mocks;
using Xunit;

namespace Simplecast.Client.Tests.ClientTests.StatisticClientTests
{
    public class GetPodcastStatisticTests
    {
        private const int PodcastId = 8354;
        private static readonly string PodcastStatisticUrl = UrlPathBuilder.GetPodcastStatisticUrl(PodcastId);

        [Fact]
        public async Task GetPodcastStatisticResponseAsync_Should_Throw_ArgumentException_If_PodcastId_Not_Greater_Than_Zero()
        {
            StatisticClientMock statisticClientMock = StatisticClientMock.Create();

            await Assert.ThrowsAsync<ArgumentException>(() => statisticClientMock.GetPodcastStatisticResponseAsync(-1));
        }

        [Fact]
        public async Task GetPodcastStatisticResponseAsync_Should_Return_ApiResponse_With_Statistic_By_Given_PodcastId()
        {
            StatisticClientMock statisticClientMock = StatisticClientMock.Create();

            statisticClientMock.RestApiClientMock
                             .Setup(client => client.GetApiResponseAsync<Statistic>(
                                        It.IsAny<string>(),
                                        It.IsAny<IList<KeyValuePair<string, string>>>(),
                                        It.IsAny<IDictionary<string, string>>()))
                             .ReturnsAsync(() => new ApiResponse<Statistic>() { Model = new Statistic() });

            ApiResponse<Statistic> apiResponse = await statisticClientMock.GetPodcastStatisticResponseAsync(PodcastId);

            statisticClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<Statistic>(
                                                           It.Is<string>(url => url == PodcastStatisticUrl),
                                                           It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues == null),
                                                           It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);

            Assert.NotNull(apiResponse);
            Assert.NotNull(apiResponse.Model);
        }

        [Fact]
        public async Task GetPodcastStatisticAsync_Should_Return_Statistic_By_Given_PodcastId()
        {
            StatisticClientMock statisticClientMock = StatisticClientMock.Create();

            statisticClientMock.RestApiClientMock
                               .Setup(client => client.GetApiResponseAsync<Statistic>(
                                          It.IsAny<string>(),
                                          It.IsAny<IList<KeyValuePair<string, string>>>(),
                                          It.IsAny<IDictionary<string, string>>()))
                               .ReturnsAsync(() => new ApiResponse<Statistic>() { Model = new Statistic() });

            Statistic statistic = await statisticClientMock.GetPodcastStatisticAsync(PodcastId);

            statisticClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<Statistic>(
                                                             It.Is<string>(url => url == PodcastStatisticUrl),
                                                             It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues == null),
                                                             It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);

            Assert.NotNull(statistic);
        }
    }
}
