using System;
using Simplecast.Client.Contracts;
using Simplecast.Client.Core;

namespace Simplecast.Client.FilterModels
{
    public class OverallStatisticFilter : IFilter
    {
        private readonly DateTime? _endDate;
        private readonly DateTime? _startDate;
        private readonly TimeFrame _timeFrame;

        public OverallStatisticFilter(DateTime? startDate = null, DateTime? endDate = null) 
            : this(Client.TimeFrame.Recent, startDate, endDate)
        {
        }

        public OverallStatisticFilter(TimeFrame timeFrame, DateTime? startDate = null, DateTime? endDate = null)
        {
            _timeFrame = timeFrame;
            _startDate = startDate;
            _endDate = endDate;
        }

        [Query("timeframe")]
        public string TimeFrame => _timeFrame.Option;

        [Query("start_date")]
        public string StartDate => _startDate?.ToString("YYYY-MM-DD");

        [Query("end_date")]
        public string EndDate => _endDate?.ToString("YYYY-MM-DD");
    }
}