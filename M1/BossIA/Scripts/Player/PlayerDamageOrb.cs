using UnityEngine;

namespace M1.BossIA.Scripts.Player
{
    public class PlayerDamageOrb : MonoBehaviour
    {
        private Transform bossTarget;
        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            bossTarget = FindFirstObjectByType<BossFSM>()?.transform;
        }

        private void FixedUpdate()
        {
            var direction = bossTarget.position - transform.position;
            direction.Normalize();
            rb.AddForce(2 * direction, ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out DamageReceiver damage))
            {
                damage.DealDamage();
            }
            
            Destroy(gameObject);
        }
    }
}