using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Simplecast.Client.Core;
using Simplecast.Client.Tests.Helpers;
using Xunit;

namespace Simplecast.Client.Tests.RestApiClientTests
{
    public class CallAsyncTests
    {
        private const string Url = "https://api.simplecast.com/v1/";
        private const string Token = "t213oksadasdae4n";

        [Fact]
        public async Task CallAsync_Should_Throw_ArgumentException_If_Path_Is_Null_Or_Empty()
        {
            var restApiClient = new RestApiClient(new HttpClient(new Mock<HttpMessageHandler>(MockBehavior.Strict).Object),
                                                  new ApiOptions(Token, Url));

            await Assert.ThrowsAsync<ArgumentNullException>(() => restApiClient.CallAsync(HttpMethod.Get, null));
            await Assert.ThrowsAsync<ArgumentException>(() => restApiClient.CallAsync(HttpMethod.Get, string.Empty));
        }

        [Theory]
        [InlineData("GET", "podcasts", null, null)]
        [InlineData("POST", "podcasts", null, null)]
        [InlineData("PUT", "podcasts", null, null)]
        [InlineData("DELETE", "podcasts", null, null)]
        [InlineData("GET", "podcasts", "time_frame=all;start_date=2018-01-01;end_date=2018-01-02", "header1=header1Value;header2=header2Value")]
        [InlineData("POST", "podcasts", "time_frame=all;end_date=2018-01-02", "header1=header1Value")]
        [InlineData("PUT", "podcasts", "start_date=2018-01-01;end_date=2018-01-02", "header1=header1Value;header2=header2Value")]
        [InlineData("DELETE", "podcasts", "time_frame=all;start_date=2018-01-01", "header1=header1Value")]
        public async Task CallAsync_Should_Call_HttpClient_SendAsync_With_HttpRequestMessage_By_Given_Parameters(string httpMethodStr, string path, string queryParams, string headerParams)
        {
            var httpMethod = new HttpMethod(httpMethodStr);
            IList<KeyValuePair<string, string>> queryStingParameters = queryParams.ToQueryStingParameters();
            IDictionary<string, string> headerParameters = headerParams.ToHeaderParameters();


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
                    Content = new StringContent("[{'id':1,'value':'1'}]"),
                })
                .Verifiable();

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var restApiClient = new RestApiClient(httpClient, new ApiOptions(Token, Url));

            await restApiClient.CallAsync(httpMethod, path, queryStingParameters, headerParameters);

            httpMessageHandler.Protected()
                              .Verify("SendAsync", Times.Once(),
                                      ItExpr.Is<HttpRequestMessage>(message => IsValidHttpRequestMessage(message, httpMethod, path, queryStingParameters, headerParameters)),
                                      ItExpr.IsAny<CancellationToken>());
        }

        private static bool IsValidHttpRequestMessage(HttpRequestMessage httpRequestMessage,
                                               HttpMethod httpMethod, 
                                               string path, 
                                               IList<KeyValuePair<string, string>> queryStingParameters, 
                                               IDictionary<string, string> headerParameters)
        {
            if (httpRequestMessage.Method != httpMethod)
            {
                return false;
            }

            string requestUriAbsoluteUri = httpRequestMessage.RequestUri.ToString();

            if (!requestUriAbsoluteUri.Contains(path))  
            {
                return false;
            }

            if (!requestUriAbsoluteUri.Contains($"api_key={Token}"))
            {
                return false;
            }

            if (queryStingParameters != null)
            {
                if (queryStingParameters.Any(valuePair => !requestUriAbsoluteUri.Contains($"{valuePair.Key}={valuePair.Value}")))
                {
                    return false;
                }
            }

            return headerParameters == null || headerParameters.All(valuePair => httpRequestMessage.Headers.GetValues(valuePair.Key).Contains(valuePair.Value));
        }
    }
}
