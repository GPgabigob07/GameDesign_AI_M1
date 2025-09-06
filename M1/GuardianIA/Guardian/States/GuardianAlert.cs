namespace M1.GuardianIA.Guardian
{
    public sealed class GuardianAlert : GuardianState
    {
        public override void OnFixedUpdate()
        {
            if (!Machine.currentEnemy) return;

            Machine.LookToPoint(Machine.currentEnemy.transform.position);
        }

        public override GuardianStates GetNextState()
        {
            if (Machine.InChaseRange()) return GuardianStates.Chase;

            return GuardianStates.Alert;
        }
    }
}