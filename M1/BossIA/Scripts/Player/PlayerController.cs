using UnityEditor;
using UnityEngine;

namespace M1.BossIA.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        
        [Header("Movement")]
        public float moveSpeed = 5f;
        public float rotationSpeed = 200f;
        public float linearDrag = 0.5f, angluarDrag = 0.5f;

        [Header("Attack")] public float attackInterval;
        
        private DamageReceiver damageReceiver;
        private Rigidbody2D rb;
        private float moveInput;
        private float rotateInput;

        private Vector2 spawnPosition;

        private bool breaks, dash, attack;
        private float attackDebounce = 0;

        private void Awake()
        {
            damageReceiver = GetComponent<DamageReceiver>();
            damageReceiver.OnTakeDamage += Respawn;
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.linearDamping = 0.5f;
            rb.angularDamping = 0.5f;
            spawnPosition = transform.position;
        }

        private void Respawn()
        {
            transform.position = spawnPosition;
            transform.rotation = Quaternion.identity;
            rb.linearVelocity = Vector2.zero;
        }

        private void Update()
        {
            moveInput = Input.GetAxisRaw("Vertical");
            rotateInput = Input.GetAxisRaw("Horizontal");
            breaks = Input.GetKey(KeyCode.LeftShift);
            dash = Input.GetKeyDown(KeyCode.Space);
            attack  = Input.GetKey(KeyCode.Mouse0);

            if (attack && (attackDebounce += Time.deltaTime) > attackInterval)
            {
                attackDebounce = 0;
                FireOrb();
            }
        }

        private void FireOrb()
        {
            Instantiate(Resources.Load("PlayerOrb"), transform.position, Quaternion.identity);
        }

        private void FixedUpdate()
        {
            rb.MoveRotation(rb.rotation - rotateInput * rotationSpeed * Time.fixedDeltaTime);

            rb.linearDamping = breaks ? 5f : linearDrag;
            rb.angularDamping = breaks ? 5f : angluarDrag;

            Vector2 forward = transform.up;
            if (dash)
            {
                dash = false;
                rb.AddForce(forward * (moveSpeed * 5), ForceMode2D.Impulse);
            }

            if (moveInput == 0) return;
            
            rb.AddForce(forward * moveSpeed, ForceMode2D.Force);
        }
    }
}