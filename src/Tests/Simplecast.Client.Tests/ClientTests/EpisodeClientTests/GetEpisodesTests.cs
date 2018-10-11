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
        private const int PodcastId = 5354;
        private static readonly string EpisodesUrl = UrlPathBuilder.GetEpisodesUrl(PodcastId);

        [Fact]
        public async Task GetEpisodesResponseAsync_Should_Throw_ArgumentException_If_PodcastId_Not_Greater_Than_Zero()
        {
            EpisodeClientMock episodeClientMock = EpisodeClientMock.Create();

            await Assert.ThrowsAsync<ArgumentException>(() => episodeClientMock.GetEpisodesResponseAsync(-1));
        }

        [Fact]
        public async Task GetEpisodesResponseAsync_Should_Return_ApiResponse_With_List_Of_Episodes_By_Given_PodcastId()
        {
            EpisodeClientMock episodeClientMock = EpisodeClientMock.Create();

            episodeClientMock.RestApiClientMock
                             .Setup(client => client.GetApiResponseAsync<List<Episode>>(
                                        It.IsAny<string>(), 
                                        It.IsAny<IList<KeyValuePair<string, string>>>(),
                                        It.IsAny<IDictionary<string, string>>()))
                             .ReturnsAsync(() => new ApiResponse<List<Episode>>() {Model = new List<Episode>()});

            ApiResponse<List<Episode>> apiResponse = await episodeClientMock.GetEpisodesResponseAsync(PodcastId);

            episodeClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<List<Episode>>(
                                                           It.Is<string>(url => url == EpisodesUrl),
                                                           It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues == null),
                                                           It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);

            Assert.NotNull(apiResponse);
            Assert.NotNull(apiResponse.Model);
        }

        [Fact]
        public async Task GetEpisodesAsync_Should_Return_List_Of_Episodes_By_Given_PodcastId()
        {
            EpisodeClientMock episodeClientMock = EpisodeClientMock.Create();

            episodeClientMock.RestApiClientMock
                             .Setup(client => client.GetApiResponseAsync<List<Episode>>(
                                        It.IsAny<string>(),
                                        It.IsAny<IList<KeyValuePair<string, string>>>(),
                                        It.IsAny<IDictionary<string, string>>()))
                             .ReturnsAsync(() => new ApiResponse<List<Episode>>() { Model = new List<Episode>() });

            List<Episode> episodes = await episodeClientMock.GetEpisodesAsync(PodcastId);

            episodeClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<List<Episode>>(
                                                           It.Is<string>(url => url == EpisodesUrl),
                                                           It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues == null),
                                                           It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);


            Assert.NotNull(episodes);
        }
    }
}
