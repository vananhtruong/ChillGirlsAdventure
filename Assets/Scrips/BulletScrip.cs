using System.Collections;
using UnityEngine;

public class BulletScrip : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float force = 50f;
    private Vector3 mousePos;
    public LayerMask playerLayer;
    public LayerMask bossLayer;
    private bool isHit = true ;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; 
        //rb.linearDamping = 0f;         // Đảm bảo không bị lực cản

        Vector3 direction = (mousePos - transform.position).normalized;
        rb.linearVelocity = direction * force;

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

        Debug.Log($"Bullet Velocity: {rb.linearVelocity.magnitude}, Direction: {direction}, Force: {force}");
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true; 
        }
    }
    private void Update()
    {
        DealDamage();
        DealDamage1();
    }

    public void SetDirection(Vector3 mousePosition)
    {
        mousePos = mousePosition;
    }
    private void DealDamage()
    { 


        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, 1, playerLayer);

        if (hitPlayer != null && hitPlayer.GetComponent<Health>() != null && isHit)
        {
            isHit = false;
            hitPlayer.GetComponent<Health>().TakeDamage(1);
        }
    }
    private void DealDamage1()
    { 


        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, 1, bossLayer);

        if (hitPlayer != null && hitPlayer.GetComponent<BossAI>() != null && isHit)
        {
            isHit = false;
            hitPlayer.GetComponent<BossAI>().TakeDamage(100);
        }
    }



}