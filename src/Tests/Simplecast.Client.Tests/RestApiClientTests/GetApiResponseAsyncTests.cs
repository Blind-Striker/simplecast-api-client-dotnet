using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Simplecast.Client.Core;
using Simplecast.Client.Core.Responses;
using Simplecast.Client.Models;
using Simplecast.Client.Tests.Helpers;
using Xunit;

namespace Simplecast.Client.Tests.RestApiClientTests
{
    public class GetApiResponseAsyncTests
    {
        private const string Url = "https://api.simplecast.com/v1/";
        private const string Token = "t213oksadasdae4n";

        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public GetApiResponseAsyncTests()
        {
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
            };
        }

        [Fact]
        public async Task GetApiResponseAsync_Should_Throw_ArgumentException_If_Path_Is_Null_Or_Empty()
        {
            var restApiClient = new RestApiClient(new HttpClient(new Mock<HttpMessageHandler>(MockBehavior.Strict).Object), new ApiOptions(Token, Url));

            await Assert.ThrowsAsync<ArgumentNullException>(() => restApiClient.GetApiResponseAsync<Podcast>(null));
            await Assert.ThrowsAsync<ArgumentException>(() => restApiClient.GetApiResponseAsync<Podcast>(string.Empty));
        }

        [Theory]
        [InlineData(HttpStatusCode.OK, "header1=header1Value;header2=header2Value", "podcasts")]
        [InlineData(HttpStatusCode.Accepted, "header3=header3Value;header4=header4Value", "episodes")]
        [InlineData(HttpStatusCode.InternalServerError, "header3=header3Value;header4=header4Value", "statistics")]
        [InlineData(HttpStatusCode.Unauthorized, "header3=header3Value;header4=header4Value", "statistics")]
        public async Task GetApiResponseAsync_Should_Return_ApiResponse_With_StatusCode_And_Headers_And_UrlPath_Regardless_Of_StatusCode_Success_Or_Not(
            HttpStatusCode httpStatusCode, string headerParams, string path)
        {
            var podcast = new Podcast() {Author = "Codefiction", Description = "Inanılmaz podcast", Id = 1234};
            IDictionary<string, string> headerParameters = headerParams.ToHeaderParameters();

            string stringContent = JsonConvert.SerializeObject(podcast, _jsonSerializerSettings);

            var httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            httpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = httpStatusCode,
                    Content = new StringContent(stringContent)
                })
                .Verifiable();

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var restApiClient = new RestApiClient(httpClient, new ApiOptions(Token, Url));

            var apiResponse = await restApiClient.GetApiResponseAsync<Podcast>(path, null, headerParameters);

            httpMessageHandler.Protected()
                              .Verify("SendAsync", Times.Once(),
                                      ItExpr.Is<HttpRequestMessage>(message => message.Method == HttpMethod.Get && message.RequestUri.ToString().Contains(path)),
                                      ItExpr.IsAny<CancellationToken>());

            Assert.Equal(httpStatusCode, apiResponse.HttpStatusCode);
            Assert.Contains(headerParameters, pair => apiResponse.Headers.All(valuePair => valuePair.Key == pair.Key && valuePair.Value == pair.Value));
            Assert.Equal(path, apiResponse.UrlPath);
        }

        [Fact]
        public async Task GetApiResponseAsync_Should_Return_ApiResponse_With_Success_And_DeserializedObject_If_HttpResponseMessage_IsSuccessStatusCode_True()
        {
            var podcast = new Podcast() { Author = "Codefiction", Description = "Inanılmaz podcast", Id = 1234 };

            string stringContent = JsonConvert.SerializeObject(podcast, _jsonSerializerSettings);

            var httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            httpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(stringContent)
                })
                .Verifiable();

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var restApiClient = new RestApiClient(httpClient, new ApiOptions(Token, Url));

            var apiResponse = await restApiClient.GetApiResponseAsync<Podcast>("episodes");

            httpMessageHandler.Protected()
                              .Verify("SendAsync", Times.Once(),
                                      ItExpr.Is<HttpRequestMessage>(message => message.Method == HttpMethod.Get && message.RequestUri.ToString().Contains("episodes")),
                                      ItExpr.IsAny<CancellationToken>());

            Assert.Equal(HttpStatusCode.OK, apiResponse.HttpStatusCode);
            Assert.False(apiResponse.Error);
            Assert.NotNull(apiResponse.Model);
            Assert.Null(apiResponse.Message);
            Assert.Equal(podcast.Author, apiResponse.Model.Author);
            Assert.Equal(podcast.Description, apiResponse.Model.Description);
            Assert.Equal(podcast.Id, apiResponse.Model.Id);
        }

        [Fact]
        public async Task GetApiResponseAsync_Should_Return_ApiResponse_With_Error_True_And_With_Error_Message_If_HttpResponseMessage_IsSuccessStatusCode_True()
        {
            var errorResponse = new ErrorResponse() {Error = "An error has occured"};
            string stringContent = JsonConvert.SerializeObject(errorResponse, _jsonSerializerSettings);

            var httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            httpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(stringContent)
                })
                .Verifiable();

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var restApiClient = new RestApiClient(httpClient, new ApiOptions(Token, Url));

            var apiResponse = await restApiClient.GetApiResponseAsync<Podcast>("episodes");

            httpMessageHandler.Protected()
                              .Verify("SendAsync", Times.Once(),
                                      ItExpr.Is<HttpRequestMessage>(message => message.Method == HttpMethod.Get && message.RequestUri.ToString().Contains("episodes")),
                                      ItExpr.IsAny<CancellationToken>());

            Assert.Equal(HttpStatusCode.InternalServerError, apiResponse.HttpStatusCode);
            Assert.True(apiResponse.Error);
            Assert.Null(apiResponse.Model);
            Assert.Equal(errorResponse.Error, apiResponse.Message);
        }
    }
}
