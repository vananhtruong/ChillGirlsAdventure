using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            smoothedPosition.z = offset.z; // Giữ Z cố định cho game 2D
            transform.position = smoothedPosition;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}