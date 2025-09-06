using UnityEngine;

namespace M1.GuardianIA.Guardian
{
    public sealed class GuardianChase : GuardianState
    {
        public override void OnFixedUpdate()
        {
            if (!Machine.currentEnemy) return;

            var t = Machine.transform;
            var et = Machine.currentEnemy.transform;
            var direction = t.position - et.position;
            direction.Normalize();
            
            Machine.LookToPoint(et.position);
            
            Machine.transform.Translate(Vector2.up * (Time.fixedDeltaTime * Machine.moveSpeed));
        }

        public override GuardianStates GetNextState()
        {
            if (Machine.InAttackRange()) return GuardianStates.Attack;
            if (!Machine.currentEnemy) return GuardianStates.Patrol;
            return GuardianStates.Chase;
        }
    }
}