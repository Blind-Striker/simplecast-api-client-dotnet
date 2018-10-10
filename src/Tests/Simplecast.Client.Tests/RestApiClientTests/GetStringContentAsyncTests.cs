using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Simplecast.Client.Core;
using Xunit;

namespace Simplecast.Client.Tests.RestApiClientTests
{
    public class GetStringContentAsyncTests
    {
        private const string Url = "https://api.simplecast.com/v1/";
        private const string Token = "t213oksadasdae4n";

        [Fact]
        public async Task GetStringContentAsync_Should_Throw_ArgumentException_If_Path_Is_Null_Or_Empty()
        {
            var restApiClient = new RestApiClient(new HttpClient(new Mock<HttpMessageHandler>(MockBehavior.Strict).Object),
                                                  new ApiOptions(Token, Url));

            await Assert.ThrowsAsync<ArgumentNullException>(() => restApiClient.GetStringContentAsync(null));
            await Assert.ThrowsAsync<ArgumentException>(() => restApiClient.GetStringContentAsync(string.Empty));
        }

        [Fact]
        public async Task GetStringContentAsync_Should_Return_Content_As_String()
        {
            var stringContent = "Hello World!";

            var httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            httpMessageHandler
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(stringContent),
                })
                .Verifiable();

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var restApiClient = new RestApiClient(httpClient, new ApiOptions(Token, Url));

            string content = await restApiClient.GetStringContentAsync("episodes");

            httpMessageHandler.Protected()
                              .Verify("SendAsync", Times.Once(),
                                      ItExpr.Is<HttpRequestMessage>(message => message.Method == HttpMethod.Get && message.RequestUri.ToString().Contains("episodes")),
                                      ItExpr.IsAny<CancellationToken>());

            Assert.Equal(stringContent, content);
        }
    }
}
