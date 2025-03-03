using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    public float moveDistance = 5f;
    public float attackRange = 5;
    public float attackCooldown = 2f;
    public int attackDamge = 2;

    private Animator animator;
    private Vector3 startPosition;
    private int direction = 1;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private Transform player;

    public float attackRadius = 4f;
    public LayerMask playerLayer;
    void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Không tìm thấy Player! Hãy kiểm tra tag của Player.");
        }

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
            //Debug.Log("Khoảng cách đến Player: " + distanceToPlayer);
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

        StartCoroutine(DealDamage());
        Invoke(nameof(ResetAttack), 0.5f); // Reset lại attack sau animation
    }

    void ResetAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    private IEnumerator DealDamage()
    {
        yield return new WaitForSeconds(0.5f); // Chờ theo animation

        if (!isAttacking) yield break; // Nếu đang không attack, thoát luôn

        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, attackRadius, playerLayer);

        if (hitPlayer != null && hitPlayer.GetComponent<Player>() != null)
        {
            hitPlayer.GetComponent<Player>().PlayerTakeDamage(attackDamge);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius); // Dùng transform.position thay vì attackPoint
    }

}
