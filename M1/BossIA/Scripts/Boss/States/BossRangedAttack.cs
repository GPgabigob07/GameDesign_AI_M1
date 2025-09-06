using System;
using Unity.VisualScripting;
using Object = UnityEngine.Object;

namespace M1.BossIA.Scripts
{
    public sealed class BossRangedAttack : BossState
    {

        private BossRangedAttackBehaviour behaviour;
        protected override void OnEnter()
        {
            if (behaviour) return;
            behaviour = Machine.AddComponent<BossRangedAttackBehaviour>();
        }

        public override void OnUpdate()
        {
            Machine.HaltAttack();
        }

        public override void OnExit()
        {
            behaviour = null;
            
            if (Machine.TryGetComponent<BossRangedAttackBehaviour>(out var special))
            {
                Object.Destroy(special);
            }
        }

        public override BossStates GetNextState()
        {
            return behaviour && behaviour.isActiveAndEnabled ? BossStates.LongRangeAttack : BossStates.Idle;
        }
        
        
    }
}