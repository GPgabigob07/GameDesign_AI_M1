using UnityEngine;

namespace M1.BossIA.Scripts
{
    public sealed class BossChase : BossState
    {
        public override BossStates GetNextState()
        {
            if (Machine.ShouldPerformSpecialAttack()) return BossStates.SpecialAttack;
            if (Machine.InMeleeRange()) return BossStates.MeleeAttack;
            if (Machine.InAttackRange()) return BossStates.LongRangeAttack;
            return BossStates.Chase;
        }

        public override void OnFixedUpdate()
        {
            var direction = Machine.player.transform.position - Machine.transform.position;
            direction.Normalize();
            
            Machine.transform.Translate(Time.fixedDeltaTime * Machine.moveSpeed * direction);
        }
    }
}