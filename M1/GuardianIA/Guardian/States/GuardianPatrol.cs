using UnityEngine;

namespace M1.GuardianIA.Guardian
{
    public sealed class GuardianPatrol : GuardianState
    {
        public override void OnFixedUpdate()
        {
            var nextIndex = Machine.currentPatrolPointIndex;
            if (Mathf.Abs(Vector2.Distance(Machine.transform.position,
                    Machine.patrolPoints[nextIndex].position)) < 0.25f)
            {
                nextIndex++;
                nextIndex = nextIndex >= Machine.patrolPoints.Length ? 0 : nextIndex;
            }

            var t = Machine.transform;
            var point = Machine.patrolPoints[nextIndex];
            Machine.currentPatrolPointIndex = nextIndex;
            Machine.LookToPoint(point.position);
            
            t.Translate(Vector2.up * (Time.fixedDeltaTime * Machine.moveSpeed));
        }

        public override GuardianStates GetNextState()
        {
            if (Machine.FindNextEnemy()) return GuardianStates.Alert;

            return GuardianStates.Patrol;
        }
    }
}