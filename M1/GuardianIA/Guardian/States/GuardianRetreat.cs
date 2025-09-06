using UnityEngine;

namespace M1.GuardianIA.Guardian
{
    public sealed class GuardianRetreat : GuardianState
    {
        private Vector2 verticalPoint;

        protected override void OnEnter()
        {
            var t = Machine.transform;
            var entrance = Machine.fortressEntrance;
            
            Machine.LookToPoint(entrance.position);
        }

        public override void OnFixedUpdate()
        {
            var t = Machine.transform;
            
            //in safe zone
            if (Mathf.Abs(Vector2.Distance(Machine.fortressEntrance.position, Machine.transform.position)) <= .2f)
            {
                Machine.currentLife += 1 * Time.fixedDeltaTime;
                Machine.currentLife = Mathf.Min(Machine.currentLife, Machine.maxLife);
                return;
            }
            
            t.Translate(t.up * (Time.fixedDeltaTime * Machine.moveSpeed));
        }


        public override GuardianStates GetNextState()
        {
            return Machine.currentLife == Machine.maxLife ? GuardianStates.Patrol : GuardianStates.Retreat;
        }
    }
}