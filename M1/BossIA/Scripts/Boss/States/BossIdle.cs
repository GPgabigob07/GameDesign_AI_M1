namespace M1.BossIA.Scripts
{
    public sealed class BossIdle : BossState
    {
        public override BossStates GetNextState()
        {
            if (Machine.player) return BossStates.Chase;
            
            return BossStates.Idle;
        }
    }
}