using UnityEngine;

public class EnemiesMove : MonoBehaviour
{
    public float speed = 2f;  // Tốc độ di chuyển
    public float moveRange = 6f; // Khoảng cách di chuyển tối đa

    private Vector3 startPosition;
    private int direction = 1; // Hướng di chuyển (1 = phải, -1 = trái)

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Di chuyển bot theo hướng hiện tại
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        // Kiểm tra xem bot đã đi đến giới hạn chưa
        if (Mathf.Abs(transform.position.x - startPosition.x) >= moveRange)
        {
            direction *= -1; // Đảo chiều di chuyển
            Flip(); // Lật bot
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Lật hướng sprite
        transform.localScale = scale;
    }
}
