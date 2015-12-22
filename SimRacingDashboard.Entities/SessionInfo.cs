using System;

namespace SimRacingDashboard.Entities
{
    [Serializable]
    public struct SessionInfo
    {
        public SessionState State { get; set; }

        public sbyte Participants { get; set; }
    }
}
