using Moq;
using Simplecast.Client.Clients;
using Simplecast.Client.Contracts;

namespace Simplecast.Client.Tests.Mocks
{
    public class EpisodeClientMock : EpisodeClient
    {
        private EpisodeClientMock(Mock<IRestApiClient> restApiClientMock)  
            : base(restApiClientMock.Object)
        {
            RestApiClientMock = restApiClientMock;
        }

        public Mock<IRestApiClient> RestApiClientMock { get; }

        public static EpisodeClientMock Create()
        {
            return new EpisodeClientMock(new Mock<IRestApiClient>(MockBehavior.Strict));
        }
    }
}