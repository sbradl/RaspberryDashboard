namespace SimRacingDashboard.DataAccess.PCars
{
    enum PCarsSessionState : byte
    {
        Invalid = 0,
        Practice,
        Test,
        Qualifying,
        FormationLap,
        Race,
        TimeAttack,
    }
}
