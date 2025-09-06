using System;
using System.Collections;
using UnityEngine;

public class BossSpecialAttackBehaviour : MonoBehaviour
{

    private GameObject melee, ranged;
    
    private IEnumerator DisplayMeleeEffect()
    {
        yield return IteratePoints(v => Instantiate(melee, v, Quaternion.identity));
    }

    private IEnumerator SpawnProjectile()
    {
        yield return IteratePoints(v => Instantiate(ranged, v, Quaternion.identity));
    }

    IEnumerator IteratePoints(Action<Vector2> callback)
    {
        var radius = 5;
        Vector2 center = transform.position;
        var count = 10;

        var step = 2f * Mathf.PI / count;

        for (var i = 0; i < count; i++)
        {
            var angle = i * step;
            var point = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            callback(point);
            yield return new WaitForSeconds(0.125f);
        }
    }
    
    private IEnumerator Start()
    {
        melee = Resources.Load<GameObject>("BossMeleeAttackEffect");
        ranged = Resources.Load<GameObject>("BossOrb");
        
        StartCoroutine(DisplayMeleeEffect());
        yield return new WaitForSeconds(.3f);
        StartCoroutine(SpawnProjectile());
        yield return new WaitForSeconds(2);
        Destroy(this);
    }
}
