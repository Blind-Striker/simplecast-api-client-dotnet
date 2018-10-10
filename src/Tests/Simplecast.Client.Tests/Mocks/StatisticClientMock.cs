using Moq;
using Simplecast.Client.Clients;
using Simplecast.Client.Contracts;

namespace Simplecast.Client.Tests.Mocks
{
    public class StatisticClientMock : StatisticClient
    {
        private StatisticClientMock(Mock<IRestApiClient> restApiClientMock)
            : base(restApiClientMock.Object)
        {
            RestApiClientMock = restApiClientMock;
        }

        public Mock<IRestApiClient> RestApiClientMock { get; }

        public static StatisticClientMock Create()
        {
            return new StatisticClientMock(new Mock<IRestApiClient>(MockBehavior.Strict));
        }
    }
}