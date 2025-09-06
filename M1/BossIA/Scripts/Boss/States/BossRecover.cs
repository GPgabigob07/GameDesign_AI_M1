using UnityEngine;

namespace M1.BossIA.Scripts
{
    public sealed class BossRecover : BossState
    {
        private float currentTime = 0f;
        protected override void OnEnter()
        { 
            Machine.currentLife += Machine.maxLife * .15f;
        }

        public override void OnUpdate()
        {
            currentTime += Time.deltaTime;
            if (currentTime > Machine.recoverInterval)
            {
                Machine.currentLife += .05f * Machine.maxLife;
                currentTime = 0f;
            }

            Machine.currentLife = Mathf.Min(Machine.currentLife, Machine.maxLife);
        }

        public override BossStates GetNextState()
        {
            if (Machine.InMeleeRange()) return BossStates.Chase;
            if (Machine.currentLife == Machine.maxLife) return BossStates.Idle;

            return BossStates.Recover;
        }
    }
}