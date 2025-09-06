using UnityEngine;

public class BossRangedAttackBehaviour: MonoBehaviour
{
    private void Start()
    {
        Instantiate(Resources.Load("BossOrb"), GetRandomPoint(), Quaternion.identity);
        Destroy(this, 2f);
    }
    
    private Vector2 GetRandomPoint()
    {
        var angle = Random.Range(0f, Mathf.PI * 2f);
        return transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 3;
    }
}