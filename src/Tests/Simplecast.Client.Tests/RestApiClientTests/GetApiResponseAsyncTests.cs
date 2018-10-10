using System;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Simplecast.Client.Core;
using Simplecast.Client.Models;
using Xunit;

namespace Simplecast.Client.Tests.RestApiClientTests
{
    public class GetApiResponseAsyncTests
    {
        private const string Url = "https://api.simplecast.com/v1/";
        private const string Token = "t213oksadasdae4n";

        [Fact]
        public async Task CallAsync_Should_Throw_ArgumentException_If_Path_Is_Null_Or_Empty()
        {
            var restApiClient = new RestApiClient(new HttpClient(new Mock<HttpMessageHandler>(MockBehavior.Strict).Object), new ApiOptions(Token, Url));

            await Assert.ThrowsAsync<ArgumentNullException>(() => restApiClient.GetApiResponseAsync<Podcast>(null));
            await Assert.ThrowsAsync<ArgumentException>(() => restApiClient.GetApiResponseAsync<Podcast>(string.Empty));
        }
    }
}
