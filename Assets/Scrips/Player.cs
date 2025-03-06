using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;

    public float moveSpeed = 4.0f;
    public float jumpForce = 7.5f;
    public float dashForce = 6.0f;
    private bool grounded = false;
    private bool isDashing = false;
    private int facingDirection = 1; // 1 for right, -1 for left
    private float timeSinceAttack = 0.0f;
    private int currentAttack = 0;
    private float delayToIdle = 0.0f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    public Transform attackPoint;
    public float attackRadius = 1.5f;
    public LayerMask targetLayer;

    public int maxHealth = 10;

    [SerializeField] private TrailRenderer tr;
    private float dashingTime = 0.3f;
    private float dashingCooldown = 1.0f;

    private int jumpCount = 0;
    private int maxJumps = 2;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if grounded
        grounded = CheckGrounded();

        // Reset jump count when grounded
        if (grounded)
        {
            jumpCount = 0;
        }

        // Update timer for attack combo
        timeSinceAttack += Time.deltaTime;

        float inputX = Input.GetAxis("Horizontal");

        // Flip sprite
        if (inputX > 0 && facingDirection == -1)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            facingDirection = 1;
        }
        else if (inputX < 0 && facingDirection == 1)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            facingDirection = -1;
        }

        // Move (only if not dashing)
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);
        }

        // Set Animator parameters
        animator.SetFloat("AirSpeedY", rb.linearVelocity.y);
        animator.SetBool("Grounded", grounded);

        // Attack combo system
        if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f && !isDashing)
        {
            if (currentAttack >= 3 || timeSinceAttack > 1.0f)
            {
                currentAttack = 0;
            }

            currentAttack = Mathf.Clamp(currentAttack + 1, 1, 3);
            animator.SetTrigger("Attack" + currentAttack);
            timeSinceAttack = 0.0f;
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && grounded)
        {
            StartCoroutine(Dash());
        }

        // Jump with double jump limit
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            if (grounded || jumpCount < maxJumps - 1)
            {
                jumpCount++;
                animator.SetTrigger("Jump");
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }

        // Run/Idle animations
        if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            delayToIdle = 0.05f;
            animator.SetInteger("AnimState", 1);
        }
        else
        {
            delayToIdle -= Time.deltaTime;
            if (delayToIdle < 0)
                animator.SetInteger("AnimState", 0);
        }

        // Debug info
        // Debug.Log($"Grounded: {grounded}, JumpCount: {jumpCount}, AirSpeedY: {rb.linearVelocity.y}");
    }

    private bool CheckGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius);
        bool isGrounded = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Ground"))
            {
                isGrounded = true;
                break;
            }
        }
        return isGrounded;
    }

    private void FixedUpdate()
    {
        // Movement is handled in Update for smoothness
    }

    void Attack()
    {
        Collider2D hitInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, targetLayer);
        if (hitInfo && hitInfo.GetComponent<Health>() != null)
        {
            hitInfo.GetComponent<Health>().TakeDamage(1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);

        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    public void PlayerTakeDamage(int damage)
    {
        if (maxHealth <= 0)
            return;
        maxHealth -= damage;
        if (maxHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(this.transform.name + " Die");
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        tr.emitting = true;

        Vector2 dashVelocity = new Vector2(facingDirection * dashForce, 0f);
        rb.linearVelocity = dashVelocity;

        yield return new WaitForSeconds(dashingTime);

        rb.linearVelocity = Vector2.zero;
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}