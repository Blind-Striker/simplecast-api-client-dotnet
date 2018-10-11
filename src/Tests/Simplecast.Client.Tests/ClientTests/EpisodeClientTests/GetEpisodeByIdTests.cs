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
    public class GetEpisodeByIdTests
    {
        private const int PodcastId = 5354;
        private const int EpisodeId = 15354;

        private static readonly string EpisodesByIdUrl = UrlPathBuilder.GetEpisodeByIdUrl(PodcastId, EpisodeId);

        [Fact]
        public async Task GetEpisodeByIdResponseAsync_Should_Throw_ArgumentException_If_PodcastId_Not_Greater_Than_Zero()
        {
            EpisodeClientMock episodeClientMock = EpisodeClientMock.Create();

            await Assert.ThrowsAsync<ArgumentException>(() => episodeClientMock.GetEpisodeByIdResponseAsync(-1, EpisodeId));
        }

        [Fact]
        public async Task GetEpisodeByIdResponseAsync_Should_Throw_ArgumentException_If_EpisodeId_Not_Greater_Than_Zero()
        {
            EpisodeClientMock episodeClientMock = EpisodeClientMock.Create();

            await Assert.ThrowsAsync<ArgumentException>(() => episodeClientMock.GetEpisodeByIdResponseAsync(PodcastId, -1));
        }

        [Fact]
        public async Task GetEpisodeByIdResponseAsync_Should_Return_ApiResponse_With_Episode_By_Given_PodcastId_And_EpisodeId()
        {
            EpisodeClientMock episodeClientMock = EpisodeClientMock.Create();

            episodeClientMock.RestApiClientMock
                             .Setup(client => client.GetApiResponseAsync<Episode>(
                                        It.IsAny<string>(),
                                        It.IsAny<IList<KeyValuePair<string, string>>>(),
                                        It.IsAny<IDictionary<string, string>>()))
                             .ReturnsAsync(() => new ApiResponse<Episode>() { Model = new Episode() });

            ApiResponse<Episode> apiResponse = await episodeClientMock.GetEpisodeByIdResponseAsync(PodcastId, EpisodeId);

            episodeClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<Episode>(
                                                           It.Is<string>(url => url == EpisodesByIdUrl),
                                                           It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues == null),
                                                           It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);

            Assert.NotNull(apiResponse);
            Assert.NotNull(apiResponse.Model);
        }

        [Fact]
        public async Task GetEpisodesAsync_Should_Return_Episode_By_Given_PodcastId_And_EpisodeId()
        {
            EpisodeClientMock episodeClientMock = EpisodeClientMock.Create();

            episodeClientMock.RestApiClientMock
                             .Setup(client => client.GetApiResponseAsync<Episode>(
                                        It.IsAny<string>(),
                                        It.IsAny<IList<KeyValuePair<string, string>>>(),
                                        It.IsAny<IDictionary<string, string>>()))
                             .ReturnsAsync(() => new ApiResponse<Episode>() { Model = new Episode() });

            Episode episode = await episodeClientMock.GetEpisodeByIdAsync(PodcastId, EpisodeId);

            episodeClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<Episode>(
                                                           It.Is<string>(url => url == EpisodesByIdUrl),
                                                           It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues == null),
                                                           It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);


            Assert.NotNull(episode);
        }
    }
}
