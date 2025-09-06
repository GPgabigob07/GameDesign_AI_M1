using UnityEngine;

namespace M1.GuardianIA.Guardian
{
    public class GuardianSpear : MonoBehaviour
    {
        private Rigidbody2D rb;
        public float force = 15f;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.AddForce(transform.up * force, ForceMode2D.Impulse);
            Destroy(gameObject, 5f);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Destroy(gameObject);
            if (other.gameObject.TryGetComponent(out DamageReceiver receiver)) receiver.DealDamage();
        }
    }
}