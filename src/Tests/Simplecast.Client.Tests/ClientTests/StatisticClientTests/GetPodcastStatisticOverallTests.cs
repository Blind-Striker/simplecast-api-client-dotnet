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
    public class GetPodcastStatisticOverallTests
    {
        private const int PodcastId = 8354;
        private static readonly OverallStatisticFilter OverallStatisticFilter = new OverallStatisticFilter(TimeFrame.Custom, DateTime.Now.AddDays(-3), DateTime.Now);
        private static readonly string PodcastStatisticOverallUrl = UrlPathBuilder.GetPodcastStatisticOverallUrl(PodcastId);        

        [Fact]
        public async Task GetPodcastStatisticOverallResponseAsync_Should_Throw_ArgumentException_If_PodcastId_Not_Greater_Than_Zero()
        {
            StatisticClientMock statisticClientMock = StatisticClientMock.Create();

            await Assert.ThrowsAsync<ArgumentException>(() => statisticClientMock.GetPodcastStatisticOverallResponseAsync(-1));
        }

        [Fact]
        public async Task GetPodcastStatisticOverallResponseAsync_Should_Return_ApiResponse_With_Statistic_By_Given_PodcastId()
        {
            StatisticClientMock statisticClientMock = StatisticClientMock.Create();

            statisticClientMock.RestApiClientMock
                               .Setup(client => client.GetApiResponseAsync<Statistic>(
                                          It.IsAny<string>(),
                                          It.IsAny<IList<KeyValuePair<string, string>>>(),
                                          It.IsAny<IDictionary<string, string>>()))
                               .ReturnsAsync(() => new ApiResponse<Statistic>() { Model = new Statistic() });

            ApiResponse<Statistic> apiResponse = await statisticClientMock.GetPodcastStatisticOverallResponseAsync(PodcastId);

            statisticClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<Statistic>(
                                                             It.Is<string>(url => url == PodcastStatisticOverallUrl),
                                                             It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues == null),
                                                             It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);

            Assert.NotNull(apiResponse);
            Assert.NotNull(apiResponse.Model);
        }

        [Fact]
        public async Task GetPodcastStatisticOverallResponseAsync_Should_Return_ApiResponse_With_Statistic_By_Given_OverallStatisticFilter()
        {
            StatisticClientMock statisticClientMock = StatisticClientMock.Create();

            statisticClientMock.RestApiClientMock
                               .Setup(client => client.GetApiResponseAsync<Statistic>(
                                          It.IsAny<string>(),
                                          It.IsAny<IList<KeyValuePair<string, string>>>(),
                                          It.IsAny<IDictionary<string, string>>()))
                               .ReturnsAsync(() => new ApiResponse<Statistic>() { Model = new Statistic() });

            var queryParams = OverallStatisticFilter.ToQueryParams();

            ApiResponse<Statistic> apiResponse = await statisticClientMock.GetPodcastStatisticOverallResponseAsync(PodcastId, OverallStatisticFilter);

            statisticClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<Statistic>(
                                                             It.Is<string>(url => url == PodcastStatisticOverallUrl),
                                                             It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues.Any(pair => queryParams.Contains(pair))),
                                                             It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);

            Assert.NotNull(apiResponse);
            Assert.NotNull(apiResponse.Model);
        }

        [Fact]
        public async Task GetPodcastStatisticOverallAsync_Should_Return_Statistic_By_Given_PodcastId()
        {
            StatisticClientMock statisticClientMock = StatisticClientMock.Create();

            statisticClientMock.RestApiClientMock
                               .Setup(client => client.GetApiResponseAsync<Statistic>(
                                          It.IsAny<string>(),
                                          It.IsAny<IList<KeyValuePair<string, string>>>(),
                                          It.IsAny<IDictionary<string, string>>()))
                               .ReturnsAsync(() => new ApiResponse<Statistic>() { Model = new Statistic() });

            Statistic statistic = await statisticClientMock.GetPodcastStatisticOverallAsync(PodcastId);

            statisticClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<Statistic>(
                                                             It.Is<string>(url => url == PodcastStatisticOverallUrl),
                                                             It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues == null),
                                                             It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);

            Assert.NotNull(statistic);
        }

        [Fact]
        public async Task GetPodcastStatisticOverallAsync_Should_Return_Statistic_By_Given_OverallStatisticFilter()
        {
            StatisticClientMock statisticClientMock = StatisticClientMock.Create();

            statisticClientMock.RestApiClientMock
                               .Setup(client => client.GetApiResponseAsync<Statistic>(
                                          It.IsAny<string>(),
                                          It.IsAny<IList<KeyValuePair<string, string>>>(),
                                          It.IsAny<IDictionary<string, string>>()))
                               .ReturnsAsync(() => new ApiResponse<Statistic>() { Model = new Statistic() });

            var queryParams = OverallStatisticFilter.ToQueryParams();

            Statistic statistic = await statisticClientMock.GetPodcastStatisticOverallAsync(PodcastId, OverallStatisticFilter);

            statisticClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<Statistic>(
                                                             It.Is<string>(url => url == PodcastStatisticOverallUrl),
                                                             It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues.Any(pair => queryParams.Contains(pair))),
                                                             It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);

            Assert.NotNull(statistic);
        }
    }
}
