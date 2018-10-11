using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Simplecast.Client.Core.Responses;
using Simplecast.Client.Models;
using Simplecast.Client.Tests.Mocks;
using Xunit;

namespace Simplecast.Client.Tests.ClientTests.PodcastClientTests
{
    public class GetPodcastByIdTests
    {
        private const int PodcastId = 5354;
        private static readonly string PodcastByIdUrl = UrlPathBuilder.GetPodcastByIdUrl(PodcastId);

        [Fact]
        public async Task GetPodcastByIdResponseAsync_Should_Throw_ArgumentException_If_PodcastId_Not_Greater_Than_Zero()
        {
            PodcastClientMock podcastClientMock = PodcastClientMock.Create();

            await Assert.ThrowsAsync<ArgumentException>(() => podcastClientMock.GetPodcastByIdResponseAsync(-1));
        }

        [Fact]
        public async Task GetPodcastByIdResponseAsync_Should_Return_ApiResponse_With_Podcast_By_Given_PodcastId()
        {
            PodcastClientMock podcastClientMock = PodcastClientMock.Create();

            podcastClientMock.RestApiClientMock
                             .Setup(client => client.GetApiResponseAsync<Podcast>(
                                        It.IsAny<string>(),
                                        It.IsAny<IList<KeyValuePair<string, string>>>(),
                                        It.IsAny<IDictionary<string, string>>()))
                             .ReturnsAsync(() => new ApiResponse<Podcast>() { Model = new Podcast() });

            ApiResponse<Podcast> apiResponse = await podcastClientMock.GetPodcastByIdResponseAsync(PodcastId);

            podcastClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<Podcast>(
                                                           It.Is<string>(url => url == PodcastByIdUrl),
                                                           It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues == null),
                                                           It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);

            Assert.NotNull(apiResponse);
            Assert.NotNull(apiResponse.Model);
        }

        [Fact]
        public async Task GetPodcastByIdAsync_Should_Return_Podcast_By_Given_PodcastId()
        {
            PodcastClientMock podcastClientMock = PodcastClientMock.Create();

            podcastClientMock.RestApiClientMock
                             .Setup(client => client.GetApiResponseAsync<Podcast>(
                                        It.IsAny<string>(),
                                        It.IsAny<IList<KeyValuePair<string, string>>>(),
                                        It.IsAny<IDictionary<string, string>>()))
                             .ReturnsAsync(() => new ApiResponse<Podcast>() { Model = new Podcast() });

            Podcast podcast = await podcastClientMock.GetPodcastByIdAsync(PodcastId);

            podcastClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<Podcast>(
                                                           It.Is<string>(url => url == PodcastByIdUrl),
                                                           It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues == null),
                                                           It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);

            Assert.NotNull(podcast);
        }
    }
}
