using Moq;
using Simplecast.Client.Clients;
using Simplecast.Client.Contracts;

namespace Simplecast.Client.Tests.Mocks
{
    public class PodcastClientMock : PodcastClient
    {
        private PodcastClientMock(Mock<IRestApiClient> restApiClientMock) 
            : base(restApiClientMock.Object)
        {
            RestApiClientMock = restApiClientMock;
        }

        public Mock<IRestApiClient> RestApiClientMock { get; }

        public static PodcastClientMock Create()
        {
            return new PodcastClientMock(new Mock<IRestApiClient>(MockBehavior.Strict));
        }
    }
}
