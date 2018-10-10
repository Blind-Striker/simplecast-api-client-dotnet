namespace Simplecast.Client
{
    public sealed class TimeFrame
    {
        internal const string RecentStr = "recent";

        public static readonly TimeFrame Recent = new TimeFrame(RecentStr);
        public static readonly TimeFrame Year = new TimeFrame("year");
        public static readonly TimeFrame All = new TimeFrame("all");
        public static readonly TimeFrame Custom = new TimeFrame("custom");

        private TimeFrame()
        {
        }

        private TimeFrame(string option)
        {
            Option = option;
        }

        public string Option { get; }

        public override string ToString()
        {
            return Option;
        }
    }
}