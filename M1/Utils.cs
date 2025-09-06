using UnityEngine;

namespace M1
{
    public static class Utils
    {
        public static void LookToPoint(this MonoBehaviour behavior, Vector3 point)
        {
            behavior.transform.LookToPoin(point);
        }

        public static void LookToPoin(this Transform transform, Vector3 point)
        {
            Vector2 direction = (point - transform.position).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}