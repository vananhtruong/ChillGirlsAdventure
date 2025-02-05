using UnityEngine;

public class EnemiesMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector3 position;
    float step = 0.01f;
    float direction = 1f;
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;
        if (position.x > 6 || position.x < -6) {
            direction *= -1f;
            transform.Rotate(0f, 180f, 0f);
        }
        position.x += direction * step;
        transform.position = position;
    }
}
