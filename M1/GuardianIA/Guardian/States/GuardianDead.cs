using UnityEditor;
using UnityEngine;

namespace M1.GuardianIA.Guardian
{
    public class GuardianDead : GuardianState
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            Object.Destroy(Machine);
        }

        public override GuardianStates GetNextState()
        {
            return GuardianStates.Dead;
        }
    }
}