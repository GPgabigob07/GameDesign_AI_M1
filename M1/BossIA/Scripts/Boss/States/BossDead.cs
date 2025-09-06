namespace M1.BossIA.Scripts
{
    public sealed class BossDead : BossState
    {
        public override BossStates GetNextState()
        {
            return BossStates.Dead;
        }
    }
}