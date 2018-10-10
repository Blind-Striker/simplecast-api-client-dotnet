using System;
using Simplecast.Client.Contracts;
using Simplecast.Client.Core;

namespace Simplecast.Client.FilterModels
{
    public class EpisodeStatisticFilter : IFilter
    {
        private readonly DateTime? _endDate;
        private readonly DateTime? _startDate;
        private readonly TimeFrame _timeFrame;

        public EpisodeStatisticFilter(int episodeId, DateTime? startDate = null, DateTime? endDate = null) 
            : this(episodeId, Client.TimeFrame.Recent, startDate, endDate)
        {
        }

        public EpisodeStatisticFilter(int episodeId, TimeFrame timeFrame, DateTime? startDate = null, DateTime? endDate = null)
        {
            EpisodeId = episodeId;
            _timeFrame = timeFrame;
            _startDate = startDate;
            _endDate = endDate;
        }

        [Query("episode_id")]
        public int EpisodeId { get; }

        [Query("timeframe")]
        public string TimeFrame => _timeFrame.Option;

        [Query("start_date")]
        public string StartDate => _startDate?.ToString("YYYY-MM-DD");

        [Query("end_date")]
        public string EndDate => _endDate?.ToString("YYYY-MM-DD");
    }
}