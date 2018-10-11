using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Simplecast.Client.Core.Responses;
using Simplecast.Client.Models;
using Simplecast.Client.Tests.Mocks;
using Xunit;

namespace Simplecast.Client.Tests.ClientTests.PlayerEmbedClientTests
{
    public class GetPlayerEmbedTests
    {
        private const int PodcastId = 52354;
        private const int EpisodeId = 25354;

        private static readonly string PlayerEmbedUrl = UrlPathBuilder.GetPlayerEmbedUrl(PodcastId, EpisodeId);

        [Fact]
        public async Task GetPlayerEmbedResponseAsync_Should_Throw_ArgumentException_If_PodcastId_Not_Greater_Than_Zero()
        {
            PlayerEmbedClientMock playerEmbedClientMock = PlayerEmbedClientMock.Create();

            await Assert.ThrowsAsync<ArgumentException>(() => playerEmbedClientMock.GetPlayerEmbedResponseAsync(-1, EpisodeId));
        }

        [Fact]
        public async Task GetPlayerEmbedResponseAsync_Should_Throw_ArgumentException_If_EpisodeId_Not_Greater_Than_Zero()
        {
            PlayerEmbedClientMock playerEmbedClientMock = PlayerEmbedClientMock.Create();

            await Assert.ThrowsAsync<ArgumentException>(() => playerEmbedClientMock.GetPlayerEmbedResponseAsync(PodcastId, -1));
        }

        [Fact]
        public async Task GetPlayerEmbedResponseAsync_Should_Return_ApiResponse_With_Embed_By_Given_PodcastId_And_EpisodeId()
        {
            PlayerEmbedClientMock playerEmbedClientMock = PlayerEmbedClientMock.Create();

            playerEmbedClientMock.RestApiClientMock
                             .Setup(client => client.GetApiResponseAsync<Embed>(
                                        It.IsAny<string>(),
                                        It.IsAny<IList<KeyValuePair<string, string>>>(),
                                        It.IsAny<IDictionary<string, string>>()))
                             .ReturnsAsync(() => new ApiResponse<Embed>() { Model = new Embed() });

            ApiResponse<Embed> apiResponse = await playerEmbedClientMock.GetPlayerEmbedResponseAsync(PodcastId, EpisodeId);

            playerEmbedClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<Embed>(
                                                           It.Is<string>(url => url == PlayerEmbedUrl),
                                                           It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues == null),
                                                           It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);

            Assert.NotNull(apiResponse);
            Assert.NotNull(apiResponse.Model);
        }

        [Fact]
        public async Task GetPlayerEmbedAsync_Should_Return_Embed_By_Given_PodcastId_And_EpisodeId()
        {
            PlayerEmbedClientMock playerEmbedClientMock = PlayerEmbedClientMock.Create();

            playerEmbedClientMock.RestApiClientMock
                             .Setup(client => client.GetApiResponseAsync<Embed>(
                                        It.IsAny<string>(),
                                        It.IsAny<IList<KeyValuePair<string, string>>>(),
                                        It.IsAny<IDictionary<string, string>>()))
                             .ReturnsAsync(() => new ApiResponse<Embed>() { Model = new Embed() });

            Embed embed = await playerEmbedClientMock.GetPlayerEmbedAsync(PodcastId, EpisodeId);

            playerEmbedClientMock.RestApiClientMock.Verify(client => client.GetApiResponseAsync<Embed>(
                                                           It.Is<string>(url => url == PlayerEmbedUrl),
                                                           It.Is<IList<KeyValuePair<string, string>>>(keyValues => keyValues == null),
                                                           It.Is<IDictionary<string, string>>(keyValues => keyValues == null)), Times.Once);


            Assert.NotNull(embed);
        }
    }
}
