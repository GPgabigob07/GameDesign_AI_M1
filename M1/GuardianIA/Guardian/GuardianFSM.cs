using System;
using Unity.VisualScripting;
using UnityEngine;

namespace M1.GuardianIA.Guardian
{
    public class GuardianFSM : BaseFSM<GuardianStates, GuardianState, GuardianFSM>
    {
        public GuardianSpear spearPrefab;
        public Transform[] patrolPoints;
        public Transform fortressEntrance;
        public GameObject currentEnemy;
        public int currentPatrolPointIndex = 0;

        public float moveSpeed, detectionRange, meleeRange, rangedRange, maxLife, currentLife, chaseRange, rangedAttackDelay, meleeAttackDelay;
        [Range(0f, 1f)] public float reinforcementsLifeGate, retreatLifeGate;
        public ParticleSystem meleeParticles;

        private DamageReceiver damageReceiver;

        private float lastRangedAttack, lastMeleeAttack;
        protected override GuardianStates InitialState => GuardianStates.Patrol;

        protected override GuardianState GetStateInstance(GuardianStates stateId)
        {
            return stateId switch
            {
                GuardianStates.Patrol => new GuardianPatrol(),
                GuardianStates.Alert => new GuardianAlert(),
                GuardianStates.Chase => new GuardianChase(),
                GuardianStates.Attack => new GuardianAttack(),
                GuardianStates.Reinforcements => new GuardianCallReinforcements(),
                GuardianStates.Retreat => new GuardianRetreat(),
                GuardianStates.Dead => new GuardianDead(),
                _ => throw new ArgumentOutOfRangeException(nameof(stateId), stateId, null)
            };
        }

        protected override void Start()
        {
            currentLife = maxLife;
            damageReceiver = GetComponent<DamageReceiver>();
            damageReceiver.OnTakeDamage += () => currentLife--;
            base.Start();
        }

        protected override void Update()
        {
            if (currentLife <= 0) return;

            base.Update();

            if (CurrentState != GuardianStates.Retreat && currentLife < (maxLife * retreatLifeGate))
            {
                ChangeStateTo(GuardianStates.Retreat);
                return;
            }

            if (CurrentState != GuardianStates.Reinforcements && currentLife < (maxLife * reinforcementsLifeGate))
            {
                ChangeStateTo(GuardianStates.Reinforcements);
            }
        }

        public bool FindNextEnemy()
        {
            if (currentEnemy) return true;
            
            var col = EnemyAtRange(detectionRange);
            currentEnemy = col?.gameObject;

            return col && col.gameObject;
        }

        public void InstantiateReinforcements()
        {
            Debug.Log("Reinforcements instantiated");
        }

        public void MeleeAttack()
        {
            if (Time.time < lastMeleeAttack + meleeAttackDelay) return;
            lastMeleeAttack = Time.time;

            if (!currentEnemy || !currentEnemy.TryGetComponent(out DamageReceiver receiver)) return;
            
            Debug.Log("Melee attack!");
            receiver.DealDamage();
            Destroy(Instantiate(meleeParticles, transform.position, Quaternion.identity), 0.5f);
        }

        public void RangedAttack()
        {
            if (Time.time < lastRangedAttack + rangedAttackDelay) return;
            lastRangedAttack = Time.time;
            
            var spear = Instantiate(spearPrefab, transform.position, Quaternion.identity);
            spear.LookToPoint(currentEnemy.transform.position);
        }

        public bool InAttackRange() =>
            Mathf.Abs(Vector2.Distance(currentEnemy.transform.position, transform.position)) <= rangedRange;
        
        public bool InChaseRange() =>
            Mathf.Abs(Vector2.Distance(currentEnemy.transform.position, transform.position)) <= chaseRange;

        private Collider2D EnemyAtRange(float range) =>
            Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask("Enemy"));

        
    }

    public enum GuardianStates
    {
        Patrol,
        Alert,
        Chase,
        Attack,
        Reinforcements,
        Retreat,
        Dead
    }

    public abstract class GuardianState : BaseFSMState<GuardianStates, GuardianState, GuardianFSM>
    {
    }
}