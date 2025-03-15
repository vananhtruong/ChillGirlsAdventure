using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    public float attackRange = 5f;
    public float attackCooldown = 2f;
    public int attackDamage = 2;

    private Animator animator;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    public Transform player;

    public float attackRadius = 4f;
    public LayerMask playerLayer;
    private AudioSource audioSource;
    [SerializeField] private AudioClip enemyAtk;
    private bool isFlipped = false;

    // Nhập 2 điểm từ Inspector
    public Vector3 startPoint;
    public Vector3 endPoint;

    private Vector3 currentTarget;

    void Start()
    {
        animator = GetComponent<Animator>();
        transform.position = startPoint; // Đặt vị trí ban đầu
        currentTarget = endPoint; // Bắt đầu di chuyển về EndPoint

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Không tìm thấy Player! Hãy kiểm tra tag của Player.");
        }

        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        // **Đảm bảo bot quay đúng hướng khi spawn**
        if (currentTarget.x < transform.position.x && !isFlipped)
        {
            Flip();
        }
        else if (currentTarget.x > transform.position.x && isFlipped)
        {
            Flip();
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
            audioSource.PlayOneShot(enemyAtk);
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
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        // Nếu bot đến vị trí mục tiêu, đổi hướng
        if (Vector3.Distance(transform.position, currentTarget) < 0.1f)
        {
            currentTarget = (currentTarget == startPoint) ? endPoint : startPoint;
            Flip();
        }
    }

    void Attack()
    {
        isAttacking = true;
        attackTimer = attackCooldown;
        animator.SetBool("isMoving", false);
        LookAtPlayer();
        animator.SetTrigger("isAttacking");
        StartCoroutine(DealDamage());
        Invoke(nameof(ResetAttack), 0.5f);
    }

    void ResetAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    private IEnumerator DealDamage()
    {
        yield return new WaitForSeconds(0.5f);
        if (!isAttacking) yield break;

        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, attackRadius, playerLayer);
        if (hitPlayer != null && hitPlayer.GetComponent<Player>() != null)
        {
            hitPlayer.GetComponent<Player>().PlayerTakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    private void Flip()
    {
        isFlipped = !isFlipped;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1; // Lật bot
        transform.localScale = newScale;
    }

}
