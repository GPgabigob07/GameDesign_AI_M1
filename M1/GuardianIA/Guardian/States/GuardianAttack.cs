using UnityEngine;

namespace M1.GuardianIA.Guardian
{
    public sealed class GuardianAttack : GuardianState
    {
        private float attackTime = 1f;

        public override void OnUpdate()
        {
            if ((attackTime -= Time.deltaTime) > 0) return;

            var t = Machine.transform;
            var et = Machine.currentEnemy.transform;
            var dist = Vector2.Distance(et.position, t.position);
            dist = Mathf.Abs(dist);

            Machine.FindNextEnemy();
            
            if (dist <= Machine.meleeRange)
            {
                Machine.MeleeAttack();
                return;
            }
            
            if (dist <= Machine.rangedRange) Machine.RangedAttack();
        }

        public override GuardianStates GetNextState()
        {
            return Machine.currentEnemy ? GuardianStates.Attack  : GuardianStates.Patrol;
        }
    }
}