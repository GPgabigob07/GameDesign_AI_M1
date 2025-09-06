using UnityEngine;

namespace M1.GuardianIA.Guardian
{
    public sealed class GuardianCallReinforcements : GuardianState
    {

        private Vector2 reinforcementPosition, entryPosition;
        private float timeToReinforcement = 1f;
        private bool spawned = false;
        
        protected override void OnEnter()
        {
            var t = Machine.transform;
            entryPosition = t.position;
            var entrance = Machine.fortressEntrance;
            
            var direction = t.position - entrance.position;
            direction /= 2;
            
            reinforcementPosition = entrance.position + direction;
        }

        public override void OnUpdate()
        {
            timeToReinforcement -= Time.deltaTime;
        }

        public override void OnFixedUpdate()
        {
            if (spawned) return;
            
            if (timeToReinforcement <= 0)
            {
                Machine.InstantiateReinforcements();
                spawned = true;
                return;
            }
            
            Machine.transform.position = Vector2.Lerp(entryPosition, reinforcementPosition, 1-timeToReinforcement);
        }

        public override GuardianStates GetNextState()
        {
            return spawned ? GuardianStates.Alert  : GuardianStates.Reinforcements;
        }
    }
}