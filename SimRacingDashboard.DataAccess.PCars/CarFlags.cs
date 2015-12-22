using System;

namespace SimRacingDashboard.DataAccess.PCars
{
    [Flags]
    enum CarFlags : byte
    {
        Headlight = 1 << 0,
        EngineActive = 1 << 1,
        EngineWarning = 1 << 2,
        SpeedLimiter = 1 << 3,
        ABS = 1 << 4,
        Handbrake = 1 << 5,
        StabilityControl = 1 << 6,
        TractionControl = 1 << 7
    }
}
