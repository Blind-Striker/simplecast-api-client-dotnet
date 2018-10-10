namespace Simplecast.Client
{
    public static class UrlPathBuilder
    {
        public const string PodcastPath = "podcasts";
        public const string EpisodePath = "episodes";
        public const string EmbedPath = "embed";
        public const string StatisticPath = "statistics";
        public const string StatisticOverallPath = "overall";

        public static readonly string PodcastUrl = $"{PodcastPath}.json";

        public static readonly string PodcastByIdUrlTemplate = $"{PodcastPath}/{{0}}.json";
        public static readonly string EpisodesUrlTemplate = $"{PodcastPath}/{{0}}/{EpisodePath}.json";
        public static readonly string EpisodesByIdUrlTemplate = $"{PodcastPath}/{{0}}/{EpisodePath}/{{1}}.json";

        public static readonly string PlayerEmbedUrlTemplate = $"{PodcastPath}/{{0}}/{EpisodePath}/{{1}}/{EmbedPath}.json";

        public static readonly string PodcastStatisticUrlTemplate = $"{PodcastPath}/{{0}}/{StatisticPath}.json";

        public static readonly string PodcastStatisticOverallUrlTemplate = $"{PodcastPath}/{{0}}/{StatisticPath}/{StatisticOverallPath}.json";

        public static readonly string PodcastStatisticEpisodeUrlTemplate = $"{PodcastPath}/{{0}}/{StatisticPath}/episode.json";

        public static string GetPodcastByIdUrl(int podcastId)
        {
            return string.Format(PodcastByIdUrlTemplate, podcastId);
        }

        public static string GetEpisodesUrl(int podcastId)
        {
            return string.Format(EpisodesUrlTemplate, podcastId);
        }

        public static string GetEpisodeByIdUrl(int podcastId, int episodeId)
        {
            return string.Format(EpisodesByIdUrlTemplate, podcastId, episodeId);
        }

        public static string GetPlayerEmbedUrl(int podcastId, int episodeId)
        {
            return string.Format(PlayerEmbedUrlTemplate, podcastId, episodeId);
        }

        public static string GetPodcastStatisticUrl(int podcastId)
        {
            return string.Format(PodcastStatisticUrlTemplate, podcastId);
        }

        public static string GetPodcastStatisticOverallUrl(int podcastId)
        {
            return string.Format(PodcastStatisticOverallUrlTemplate, podcastId);
        }

        public static string GetPodcastStatisticEpisodeUrl(int podcastId)
        {
            return string.Format(PodcastStatisticEpisodeUrlTemplate, podcastId);
        }
    }
}