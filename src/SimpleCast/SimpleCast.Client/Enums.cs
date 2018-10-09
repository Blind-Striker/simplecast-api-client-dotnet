namespace Simplecast.Client
{
    public sealed class TimeFrame
    {
        internal const string RecentStr = "recent";

        private TimeFrame()
        {
        }

        private TimeFrame(string option)
        {
            Option = option;
        }

        public string Option { get; }

        public static readonly TimeFrame Recent = new TimeFrame(RecentStr);
        public static readonly TimeFrame Year = new TimeFrame("year");
        public static readonly TimeFrame All = new TimeFrame("all");
        public static readonly TimeFrame Custom = new TimeFrame("custom");

        public override string ToString()
        {
            return Option;
        }
    }
}
