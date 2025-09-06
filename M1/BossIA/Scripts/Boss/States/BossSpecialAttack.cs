using Unity.VisualScripting;
using UnityEngine;

namespace M1.BossIA.Scripts
{
    public sealed class BossSpecialAttack : BossState
    {
        private BossSpecialAttackBehaviour behaviour;
        protected override void OnEnter()
        {
            if (behaviour) return;
            behaviour = Machine.AddComponent<BossSpecialAttackBehaviour>();
        }

        public override BossStates GetNextState()
        {
            return behaviour && behaviour.isActiveAndEnabled ? BossStates.SpecialAttack : BossStates.Idle;
        }

        public override void OnExit()
        {
            if (Machine.TryGetComponent<BossSpecialAttackBehaviour>(out var special))
            {
                Object.Destroy(special);
            }
        }
    }
}