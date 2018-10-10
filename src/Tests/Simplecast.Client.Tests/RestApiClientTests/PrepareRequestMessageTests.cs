using System;
using System.Collections.Generic;
using System.Net.Http;
using Moq;
using Simplecast.Client.Core;
using Xunit;

namespace Simplecast.Client.Tests.RestApiClientTests
{
    public class PrepareRequestMessageTests
    {
        private const string Url = "https://api.simplecast.com/v1/";
        private const string Token = "t213oksadasdae4n";

        [Fact]
        public void PrepareRequestMessage_Should_Throw_ArgumentException_If_Path_Is_Null_Or_Empty()
        {
            var restApiClient = new RestApiClient(new HttpClient(new Mock<HttpMessageHandler>(MockBehavior.Strict).Object),
                                                  new ApiOptions(Token, Url));

            Assert.Throws<ArgumentNullException>(() => restApiClient.PrepareRequestMessage(null, HttpMethod.Get));
            Assert.Throws<ArgumentException>(() => restApiClient.PrepareRequestMessage(string.Empty, HttpMethod.Get));
        }

        [Fact]
        public void PrepareRequestMessage_Should_Add_ApiKey_As_Default_Query_String_Parameter()
        {
            var restApiClient = new RestApiClient(new HttpClient(new Mock<HttpMessageHandler>(MockBehavior.Strict).Object),
                                                  new ApiOptions(Token, Url));

            HttpRequestMessage httpRequestMessage = restApiClient.PrepareRequestMessage("podcasts", HttpMethod.Get, null);

            string requestUriAbsoluteUri = httpRequestMessage.RequestUri.ToString();

            Assert.Contains("podcasts", requestUriAbsoluteUri);
            Assert.Contains($"api_key={Token}", requestUriAbsoluteUri);
        }

        [Fact]
        public void PrepareRequestMessage_Should_Add_Given_KeyValuePair_As_Query_String_Parameters()
        {
            var restApiClient = new RestApiClient(new HttpClient(new Mock<HttpMessageHandler>(MockBehavior.Strict).Object),
                                                  new ApiOptions(Token, Url));

            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("time_frame", "all"),
                new KeyValuePair<string, string>("start_date", "2018-01-01"),
                new KeyValuePair<string, string>("end_date", "2018-01-02")
            };

            HttpRequestMessage httpRequestMessage = restApiClient.PrepareRequestMessage("podcasts", HttpMethod.Get, parameters);

            string requestUriAbsoluteUri = httpRequestMessage.RequestUri.ToString();

            Assert.Contains("podcasts", requestUriAbsoluteUri);
            Assert.Contains($"api_key={Token}", requestUriAbsoluteUri);

            foreach (KeyValuePair<string, string> valuePair in parameters)
            {
                Assert.Contains($"{valuePair.Key}={valuePair.Value}", requestUriAbsoluteUri);
            }
        }

        [Fact]
        public void PrepareRequestMessage_Should_Add_Given_Dictionary_As_Header_Parameters()
        {
            var restApiClient = new RestApiClient(new HttpClient(new Mock<HttpMessageHandler>(MockBehavior.Strict).Object),
                                                  new ApiOptions(Token, Url));

            var headers = new Dictionary<string, string>() {{"header1", "header1Value"}, { "header2", "header2Value" } };   

            HttpRequestMessage httpRequestMessage = restApiClient.PrepareRequestMessage("podcasts", HttpMethod.Get, null, headers);

            string requestUriAbsoluteUri = httpRequestMessage.RequestUri.ToString();

            Assert.Contains("podcasts", requestUriAbsoluteUri);
            Assert.Contains($"api_key={Token}", requestUriAbsoluteUri);

            foreach (KeyValuePair<string, string> keyValuePair in headers)
            {
                Assert.Contains(httpRequestMessage.Headers.GetValues(keyValuePair.Key), s => keyValuePair.Value == s);
            }
        }
    }
}
