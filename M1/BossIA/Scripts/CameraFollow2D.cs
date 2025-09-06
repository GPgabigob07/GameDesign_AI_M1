using UnityEngine;

namespace M1.BossIA.Scripts
{
    public class CameraFollow2D : MonoBehaviour
    {
        public Transform target;
        public float smoothSpeed = 0.125f;
        public Vector3 offset;

        private void LateUpdate()
        {
            if (!target) return;
            var desiredPosition = target.position + offset;
            var lerpPosition = Vector2.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = new Vector3(lerpPosition.x, lerpPosition.y, transform.position.z);
        }
    }
}