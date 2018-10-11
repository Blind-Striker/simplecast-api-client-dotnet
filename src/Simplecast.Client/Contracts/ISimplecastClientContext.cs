namespace Simplecast.Client.Contracts
{
    public interface ISimplecastClientContext
    {
        IEpisodeClient EpisodeClient { get; }

        IPlayerEmbedClient PlayerEmbedClient { get; }

        IPodcastClient PodcastClient { get; }

        IStatisticClient StatisticClient { get; }
    }
}