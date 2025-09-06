using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace M1.GuardianIA.Enemies
{
    public class GuardianEnemy: MonoBehaviour
    {
        public float moveSpeed, hp;
        public Transform target;

        private DamageReceiver damage;
        void Start()
        {
            if (!target) return;
            LookToPoint(target.position);
            damage = GetComponent<DamageReceiver>();
            damage.OnTakeDamage += () =>
            {
                hp--;
                transform.Translate(transform.up * (-1 * moveSpeed));
                if (hp <= 0)
                {
                    Destroy(gameObject);
                }
            };

            StartCoroutine(DamageArea());
        }

        private void FixedUpdate()
        {
            LookToPoint(target.position);
            transform.Translate(moveSpeed * Time.deltaTime * Vector2.up);
        }

        public void LookToPoint(Vector3 point)
        {
            Vector2 direction = (point - transform.position).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        public void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.TryGetComponent(out DamageReceiver damageReceiver)) return;
            damageReceiver.DealDamage();
        }

        private IEnumerator DamageArea()
        {
            while (isActiveAndEnabled)
            {
                yield return new WaitForFixedUpdate();
                var player = Physics2D.OverlapCircle(transform.position, 1.5f, LayerMask.GetMask("Player"));

                if (player && player.gameObject.TryGetComponent(out DamageReceiver damageReceiver))
                {
                    damageReceiver.DealDamage();
                }

                yield return new WaitForSeconds(1f);
            }
        }
    }
}