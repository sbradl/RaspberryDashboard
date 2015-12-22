using System;

namespace SimRacingDashboard.Entities
{
    public struct TimingInfo
    {
        public TimeSpan BestLapTime { get; set; }
        public TimeSpan LastLapTime { get; set; }
        public TimeSpan CurrentLapTime { get; set; }

        public TimeSpan SplitTime { get; set; }
        public TimeSpan SplitTimeDifference { get; set; }

        public TimeSpan[] CurrentSectorTimes { get; set; }
        public TimeSpan[] FastestSectorTimes { get; set; }
    }
}
