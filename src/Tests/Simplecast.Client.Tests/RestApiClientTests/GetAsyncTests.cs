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
using Simplecast.Client.Models;
using Xunit;

namespace Simplecast.Client.Tests.RestApiClientTests
{
    public class GetAsyncTests
    {
        private const string Url = "https://api.simplecast.com/v1/";
        private const string Token = "t213oksadasdae4n";

        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public GetAsyncTests()
        {
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
            };
        }

        [Fact]
        public async Task GetStringContentAsync_Should_Throw_ArgumentException_If_Path_Is_Null_Or_Empty()
        {
            var restApiClient = new RestApiClient(new HttpClient(new Mock<HttpMessageHandler>(MockBehavior.Strict).Object),
                                                  new ApiOptions(Token, Url));

            await Assert.ThrowsAsync<ArgumentNullException>(() => restApiClient.GetAsync<Podcast>(null));
            await Assert.ThrowsAsync<ArgumentException>(() => restApiClient.GetAsync<Podcast>(string.Empty));
        }

        [Fact]
        public async Task GetAsync_Should_Return_DeserializedObject()
        {
            var statistic = new Statistic()
            {
                TotalListens = 1231,
                Data = new List<Datum>()
                {
                    new Datum() {Date = "2018-10-10", Listens = 222},
                    new Datum() {Date = "2018-11-11", Listens = 453}
                }
            };

            string stringContent = JsonConvert.SerializeObject(statistic, _jsonSerializerSettings);

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
                    Content = new StringContent(stringContent)
                })
                .Verifiable();

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var restApiClient = new RestApiClient(httpClient, new ApiOptions(Token, Url));

            Statistic statisticResult = await restApiClient.GetAsync<Statistic>("statistic");

            httpMessageHandler.Protected()
                              .Verify("SendAsync", Times.Once(),
                                      ItExpr.Is<HttpRequestMessage>(message => message.Method == HttpMethod.Get && message.RequestUri.ToString().Contains("statistic")),
                                      ItExpr.IsAny<CancellationToken>());

            Assert.NotNull(statisticResult);
            Assert.Equal(statistic.TotalListens, statisticResult.TotalListens);
            Assert.Equal(statistic.Data.Count, statisticResult.Data.Count);
            Assert.Contains(statistic.Data, datum => statisticResult.Data.Any(statisticDatum =>
                                                                  statisticDatum.Listens == datum.Listens &&
                                                                  statisticDatum.Date == datum.Date));
        }
    }
}
