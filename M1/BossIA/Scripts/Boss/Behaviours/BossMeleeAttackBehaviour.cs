using System.Collections;
using UnityEngine;

namespace M1.BossIA.Scripts.Behaviours
{
    public class BossMeleeAttackBehaviour : MonoBehaviour
    {
        IEnumerator Start()
        {
            var particle = Instantiate(Resources.Load("BossMeleeAttackEffect"), transform.position, Quaternion.identity);
            yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(.15f);
            var collision = Physics2D.OverlapCircle(transform.position, 3f, LayerMask.GetMask("Player"));

            if (collision)
            {
                Debug.Log("Boss melee hit!");
                collision.gameObject.TryGetComponent(out DamageReceiver damageReceiver);
                damageReceiver?.DealDamage();
            }
            
            yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(.15f);
            Destroy(this);
            Destroy(particle);
        }
    }
}