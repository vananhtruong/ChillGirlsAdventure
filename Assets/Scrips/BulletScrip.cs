using UnityEngine;

public class BulletScrip : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float force = 50f; // Có thể chỉnh trong Inspector
    private Vector3 mousePos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // Đảm bảo không bị trọng lực
        rb.linearDamping = 0f;         // Đảm bảo không bị lực cản

        Vector3 direction = (mousePos - transform.position).normalized;
        rb.linearVelocity = direction * force;

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

        Debug.Log($"Bullet Velocity: {rb.linearVelocity.magnitude}, Direction: {direction}, Force: {force}");
    }

    public void SetDirection(Vector3 mousePosition)
    {
        mousePos = mousePosition;
    }
}