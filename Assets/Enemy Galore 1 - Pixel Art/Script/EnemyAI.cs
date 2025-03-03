using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    public float moveDistance = 5f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;

    private Animator animator;
    private Vector3 startPosition;
    private int direction = 1;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private Transform player;

    void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                isAttacking = false;
                animator.SetBool("isAttacking", false);
            }
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        animator.SetBool("isMoving", true);

        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - startPosition.x) >= moveDistance)
        {
            direction *= -1;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    void Attack()
    {
        isAttacking = true;
        attackTimer = attackCooldown;
        animator.SetBool("isMoving", false);
        animator.SetTrigger("isAttacking");
    }
}
