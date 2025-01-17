﻿using System;

namespace SimRacingDashboard.Entities
{
    [Serializable]
    public struct EventState
    {
        public byte LapsInEvent { get; set; }

        public float TrackLength { get; set; }

        public TimeSpan TimeRemaining { get; set; }

        public Flag Flag { get; set; }
    }
}
