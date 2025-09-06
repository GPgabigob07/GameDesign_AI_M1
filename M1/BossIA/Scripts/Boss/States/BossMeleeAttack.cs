using M1.BossIA.Scripts.Behaviours;
using Unity.VisualScripting;
using UnityEngine;

namespace M1.BossIA.Scripts
{
    public sealed class BossMeleeAttack : BossState
    {
        private BossMeleeAttackBehaviour behaviour;

        protected override void OnEnter()
        {
            if (behaviour) return;

            behaviour = Machine.AddComponent<BossMeleeAttackBehaviour>();
        }

        public override void OnUpdate()
        {
            Machine.HaltAttack();
        }

        public override BossStates GetNextState()
        {
            return behaviour && behaviour.isActiveAndEnabled ? BossStates.MeleeAttack : BossStates.Idle;
        }

        public override void OnExit()
        {
            if (Machine.TryGetComponent<BossMeleeAttackBehaviour>(out var special))
            {
                Object.Destroy(special);
            }
        }
    }
}