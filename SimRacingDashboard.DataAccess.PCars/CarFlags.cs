using System;

namespace SimRacingDashboard.DataAccess.PCars
{
    [Flags]
    enum CarFlags : byte
    {
        Headlight,
        EngineActive,
        EngineWarning,
        SpeedLimiter,
        ABS,
        Handbrake,
        StabilityControl,
        TractionControl,
    }
}
