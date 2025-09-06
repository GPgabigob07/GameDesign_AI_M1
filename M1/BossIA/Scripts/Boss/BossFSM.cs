using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace M1.BossIA.Scripts
{
    public class BossFSM : BaseFSM<BossStates, BossState, BossFSM>
    {
        [SerializeField] internal float playerDetectionRange = 15,
            plaerFollowRange = 15,
            meleeAttackRange = 3,
            longRangeAttackRange = 10,
            maxLife = 100,
            currentLife,
            specialAttackInterval = 5,
            moveSpeed = 5,
            fleeSpeed = 10,
            recoverInterval,
            attackDelay = 2f;

        [SerializeField, Range(0f, 1f)] private float specialAttackLifeGate = .5f, fleeAttemptLifeGate = .2f;
        [SerializeField] internal Transform safeSpot;

        private DamageReceiver receiver;
        internal GameObject player;
        private float attackDebounce = 0f;

        private bool superArmor;

        protected override void Start()
        {
            receiver = GetComponent<DamageReceiver>();
            receiver.OnTakeDamage += () => currentLife--;
            
            base.Start();
            attackDebounce = 2f;
        }

        protected override void FixedUpdate()
        {
            TryFindPlayer();
            base.FixedUpdate();
        }

        protected override void Update()
        {
            if (IsDead()) return;
            
            if (currentLife <= 0)
            {
                ChangeStateTo(BossStates.Dead);
                return;
            }

            attackDebounce += Time.deltaTime;
            base.Update();

            if (currentLife <= (maxLife * fleeAttemptLifeGate))
            {
                ChangeStateTo(BossStates.Flee);
            }
        }

        private bool IsDead()
        {
            return CurrentState == BossStates.Dead;
        }

        internal bool ShouldPerformSpecialAttack() => currentLife <= (maxLife * specialAttackLifeGate);

        private void TryFindPlayer()
        {
            if (player) return;

            var foundPlayer = Physics2D.OverlapCircle(transform.position, playerDetectionRange, LayerMask.GetMask("Player"));
            if (foundPlayer)
            {
                player = foundPlayer.gameObject;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, playerDetectionRange);
        }

        public bool InMeleeRange()
        {
            return DistanceToPlayer() <= meleeAttackRange && CanAttack();
        }

        public bool InAttackRange()
        {
            return DistanceToPlayer() <= longRangeAttackRange && CanAttack();
        }

        public bool CanAttack()
        {
            return attackDebounce > attackDelay;
        }

        private float DistanceToPlayer() => player ? Mathf.Abs(Vector2.Distance(player.transform.position, transform.position)) : int.MaxValue;

        protected override BossStates InitialState => BossStates.Idle;

        protected override BossState GetStateInstance(BossStates stateId)
        {
            return stateId switch
            {
                BossStates.Chase => new BossChase(),
                BossStates.MeleeAttack => new BossMeleeAttack(),
                BossStates.LongRangeAttack => new BossRangedAttack(),
                BossStates.SpecialAttack => new BossSpecialAttack(),
                BossStates.Flee => new BossFlee(),
                BossStates.Recover => new BossRecover(),
                BossStates.Dead => new BossDead(),
                _ => new BossIdle()
            };
        }

        internal bool InSafeSpot()
        {
            return Mathf.Abs(Vector2.Distance(transform.position, safeSpot.position)) < 0.2;
        }

        public void HaltAttack()
        {
            attackDebounce = 0;
        }
    }

    public abstract class BossState : BaseFSMState<BossStates, BossState, BossFSM>
    {
    }

    public enum BossStates
    {
        Idle,
        Chase,
        MeleeAttack,
        LongRangeAttack,
        SpecialAttack,
        Flee,
        Recover,
        Dead
    }
}