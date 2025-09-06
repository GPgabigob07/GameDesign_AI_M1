using M1.BossIA.Scripts.Player;
using UnityEngine;

namespace M1.BossIA.Scripts.Behaviours
{
    public class BossProjectile: MonoBehaviour
    {
        private Transform playerTarget;
        private Rigidbody2D rb;
        public float trackTime = 2f;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            playerTarget = FindFirstObjectByType<PlayerController>()?.transform;
        }

        private void FixedUpdate()
        {
            if ((trackTime -= Time.fixedDeltaTime) < 0) return;
            
            var direction = playerTarget.position - transform.position;
            direction.Normalize();
            rb.AddForce(1.15f * direction, ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out DamageReceiver damage))
            {
                Debug.Log($"Dealing damage against {other.gameObject.name}");
                damage.DealDamage();
            }
            
            Destroy(gameObject);
        }
    }
}