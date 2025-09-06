using UnityEngine;

namespace M1.BossIA.Scripts
{
    public sealed class BossFlee : BossState
    {
        
        public override void OnFixedUpdate()
        {
            var direction = Machine.safeSpot.position - Machine.transform.position;
            direction.Normalize();
            Machine.transform.Translate(Time.fixedDeltaTime * Machine.fleeSpeed * direction);
        }

        public override BossStates GetNextState()
        {
            if (Machine.InMeleeRange()) return BossStates.Chase;
            if (Machine.InSafeSpot()) return BossStates.Recover;

            return BossStates.Flee;
        }
    }
}