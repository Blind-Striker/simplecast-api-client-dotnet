using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Simplecast.Client.Core.Responses;
using Simplecast.Client.Models;
using Simplecast.Client.Tests.Mocks;
using Xunit;

namespace Simplecast.Client.Tests.ClientTests.EpisodeClientTests
{
    public class GetEpisodesTests
    {
        [Fact]
        public async Task GetEpisodesResponseAsync_Should_ArgumentException_If_PodcastId_Not_Greater_Than_Zero()
        {
            EpisodeClientMock episodeClientMock = EpisodeClientMock.Create();

            await Assert.ThrowsAsync<ArgumentException>(() => episodeClientMock.GetEpisodesResponseAsync(-1));
        }

        [Fact]
        public async Task GetEpisodesResponseAsync_Should_Return_ApiResponse_With_List_Of_Episodes_By_Given_PodcastId()
        {
            EpisodeClientMock episodeClientMock = EpisodeClientMock.Create();

            const int podcastId = 5354;
            string episodesUrl = UrlPathBuilder.GetEpisodesUrl(podcastId);

            episodeClientMock.RestApiClientMock
                             .Setup(client => client.GetApiResponseAsync<List<Episode>>(
                                        It.IsAny<string>(), 
                                        It.IsAny<IList<KeyValuePair<string, string>>>(),
                                        It.IsAny<IDictionary<string, string>>()))
                             .ReturnsAsync(() => new ApiResponse<List<Episode>>() {Model = new List<Episode>()});

            ApiResponse<List<Episode>> apiResponse = await episodeClientMock.GetEpisodesResponseAsync(podcastId);

            episodeClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<List<Episode>>(
                                                           It.Is<string>(url => url == episodesUrl),
                                                           It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues == null),
                                                           It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);

            Assert.NotNull(apiResponse);
            Assert.NotNull(apiResponse.Model);
        }
    }
}
