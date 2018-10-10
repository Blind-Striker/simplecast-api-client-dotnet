using Moq;
using Simplecast.Client.Clients;
using Simplecast.Client.Contracts;

namespace Simplecast.Client.Tests.Mocks
{
    public class PlayerEmbedClientMock : PlayerEmbedClient
    {
        private PlayerEmbedClientMock(Mock<IRestApiClient> restApiClientMock)
            : base(restApiClientMock.Object)
        {
            RestApiClientMock = restApiClientMock;
        }

        public Mock<IRestApiClient> RestApiClientMock { get; }

        public static PlayerEmbedClientMock Create()
        {
            return new PlayerEmbedClientMock(new Mock<IRestApiClient>(MockBehavior.Strict));
        }
    }
}