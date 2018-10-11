using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Simplecast.Client.Core.Responses;
using Simplecast.Client.Models;
using Simplecast.Client.Tests.Mocks;
using Xunit;

namespace Simplecast.Client.Tests.ClientTests.PodcastClientTests
{
    public class GetPodcastsTests
    {
        private static readonly string PodcastUrl = UrlPathBuilder.PodcastUrl;

        [Fact]
        public async Task GetPodcastsResponseAsync_Should_Return_ApiResponse_With_List_Of_Podcasts()
        {
            PodcastClientMock podcastClientMock = PodcastClientMock.Create();

            podcastClientMock.RestApiClientMock
                             .Setup(client => client.GetApiResponseAsync<List<Podcast>>(
                                        It.IsAny<string>(),
                                        It.IsAny<IList<KeyValuePair<string, string>>>(),
                                        It.IsAny<IDictionary<string, string>>()))
                             .ReturnsAsync(() => new ApiResponse<List<Podcast>>() { Model = new List<Podcast>() });

            ApiResponse<List<Podcast>> apiResponse = await podcastClientMock.GetPodcastsResponseAsync();

            podcastClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<List<Podcast>>(
                                                           It.Is<string>(url => url == PodcastUrl),
                                                           It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues == null),
                                                           It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);

            Assert.NotNull(apiResponse);
            Assert.NotNull(apiResponse.Model);
        }

        [Fact]
        public async Task GetPodcastsAsync_Should_Return_ApiResponse_With_List_Of_Podcasts()
        {
            PodcastClientMock podcastClientMock = PodcastClientMock.Create();

            podcastClientMock.RestApiClientMock
                             .Setup(client => client.GetApiResponseAsync<List<Podcast>>(
                                        It.IsAny<string>(),
                                        It.IsAny<IList<KeyValuePair<string, string>>>(),
                                        It.IsAny<IDictionary<string, string>>()))
                             .ReturnsAsync(() => new ApiResponse<List<Podcast>>() { Model = new List<Podcast>() });

            List<Podcast> podcasts = await podcastClientMock.GetPodcastsAsync();

            podcastClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<List<Podcast>>(
                                                           It.Is<string>(url => url == PodcastUrl),
                                                           It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues == null),
                                                           It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);

            Assert.NotNull(podcasts);
        }
    }
}
