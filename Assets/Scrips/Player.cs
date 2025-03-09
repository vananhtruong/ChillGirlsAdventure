<<<<<<< HEAD
﻿using UnityEngine;
=======
﻿using Unity.VisualScripting;
using UnityEngine;
>>>>>>> a9670cd417c94c4ef4d0a3849f7267fbaac79b8a
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public CharacterControllerManager controllerManager; // Thêm tham chiếu đến manager

<<<<<<< HEAD
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

=======
    public float jumpHeight = 1f;
    public bool isGround = false;
    private float movement;
    public float moveSpeed = 5f;
    private bool facingRight = true;

>>>>>>> a9670cd417c94c4ef4d0a3849f7267fbaac79b8a
    public Transform attackPoint;
    public float attackRadius = 1.5f;
    public LayerMask targetLayer;

    public int maxHealth = 10;

    [SerializeField] private TrailRenderer tr;
    private float dashingTime = 0.3f;
    private float dashingCooldown = 1.0f;

    private int jumpCount = 0;
    private int maxJumps = 2;

    private int jumpCount = 0;

    //background
    private PolygonCollider2D backgroundCollider;
    //collection
    public int currentTao = 0;
    public Text TextHeart;

    private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip hurtSound;

    private bool isShieldActive = false;

    void Start()
    {
<<<<<<< HEAD
        if (animator == null)
            animator = GetComponent<Animator>();
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
=======
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        currentTao = maxHealth;
>>>>>>> a9670cd417c94c4ef4d0a3849f7267fbaac79b8a
    }

    void Update()
    {
<<<<<<< HEAD
        // Check if grounded
        grounded = CheckGrounded();

        // Reset jump count when grounded
        if (grounded)
=======


        if (transform.position.y < -12f)
        {
            PlayerTakeDamage(maxHealth); // Giảm toàn bộ máu
        }
        TextHeart.text = currentTao.ToString();
        if (!enabled) return; // Chỉ chạy khi component được bật

        movement = Input.GetAxis("Horizontal");
        if (movement < 0f && facingRight)
>>>>>>> a9670cd417c94c4ef4d0a3849f7267fbaac79b8a
        {
            jumpCount = 0;
        }

<<<<<<< HEAD
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
=======
        animator.SetBool("Grounded", isGround);
        animator.SetBool("Jump", !isGround && rb.linearVelocity.y > 0);

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerTakeDamage(1);
        }

        if (Input.GetMouseButtonDown(1))
        {
            isShieldActive = true;
            animator.SetTrigger("Shield");
            animator.SetBool("IdeShield", true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            isShieldActive = false;
            animator.SetBool("IdeShield", false);
        }

        animator.SetFloat("Run", Mathf.Abs(movement));

        Attack();
        if (maxHealth <= 0)
        {
            Die();
>>>>>>> a9670cd417c94c4ef4d0a3849f7267fbaac79b8a
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && grounded)
        {
            StartCoroutine(Dash());
        }

        // Jump with double jump limit
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
<<<<<<< HEAD
            if (grounded || jumpCount < maxJumps - 1)
            {
                jumpCount++;
                animator.SetTrigger("Jump");
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
=======
            animator.SetTrigger("Attack"); 
            PlayerAttack();
            PlayerAttackBoss();
            if (attackSound != null)
            {
                audioSource.PlayOneShot(attackSound);
>>>>>>> a9670cd417c94c4ef4d0a3849f7267fbaac79b8a
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

<<<<<<< HEAD
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
=======
    private void FixedUpdate()
    {
        if (!enabled) return; // Chỉ chạy khi component được bật

        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * moveSpeed;
        if (isDashing)
        {
            return;
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
        isGround = false;
        jumpCount++;

        if (jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Va chạm với: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            jumpCount = 0;
        }
        if (collision.gameObject.tag == "TaoCollect")
        {
            currentTao++;
            maxHealth++;
            collision.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collect");
            Destroy(collision.gameObject, 1f);
            Debug.Log(collision.gameObject.tag + "collected");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f, LayerMask.GetMask("Ground"));
            isGround = hits.Length > 0;
            animator.SetBool("Grounded", isGround);
        }
    }

    public void PlayerAttack()
>>>>>>> a9670cd417c94c4ef4d0a3849f7267fbaac79b8a
    {
        Collider2D hitInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, targetLayer);
        if (hitInfo && hitInfo.GetComponent<Health>() != null)
        {
<<<<<<< HEAD
            hitInfo.GetComponent<Health>().TakeDamage(1);
        }
    }
=======
            if (hitInfo.GetComponent<Health>() != null)
            {
                Debug.Log("We hit " + hitInfo.name);
                hitInfo.GetComponent<Health>().TakeDamage(1);
            }
        }
    }
    public void PlayerAttackBoss()
    {
        Collider2D hitInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, targetLayer);
        if (hitInfo)
        {
            if (hitInfo.GetComponent<BossAI>() != null)
            {
                Debug.Log("We hit " + hitInfo.name);
                hitInfo.GetComponent<BossAI>().TakeDamage(50);
            }
            
        }
    }
>>>>>>> a9670cd417c94c4ef4d0a3849f7267fbaac79b8a

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
<<<<<<< HEAD

        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
=======
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
>>>>>>> a9670cd417c94c4ef4d0a3849f7267fbaac79b8a
    }

    public void PlayerTakeDamage(int damage)
    {
        if (isShieldActive)
        {
            Debug.Log("Shield blocked damage!");
            return;
        }
        if (maxHealth <= 0)
            return;
        maxHealth -= damage;
<<<<<<< HEAD
        if (maxHealth <= 0)
        {
            Die();
=======
        currentTao -= damage;
        animator.SetTrigger("Hurt");
        if (hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
>>>>>>> a9670cd417c94c4ef4d0a3849f7267fbaac79b8a
        }
    }

    void Die()
    {
        Debug.Log(this.transform.name + " Die");
        animator.SetTrigger("Die");
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        enabled = false;
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        tr.emitting = true;
        animator.SetTrigger("Dash");

        if (dashSound != null)
        {
            audioSource.PlayOneShot(dashSound);
        }

        Vector2 dashVelocity = new Vector2(facingDirection * dashForce, 0f);
        rb.linearVelocity = dashVelocity;

        yield return new WaitForSeconds(dashingTime);

        rb.linearVelocity = Vector2.zero;
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
    }
    public static float Clamp(float value, float min, float max)
    {
        if (value < min)
        {
            value = min;
        }
        else if (value > max)
        {
            value = max;
        }

<<<<<<< HEAD
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
=======
        return value;
    }
    private void ClampOnBackground()
    {
        if (backgroundCollider == null)
        {
            backgroundCollider = GameObject.FindWithTag("Background")?.GetComponent<PolygonCollider2D>();
            if (backgroundCollider == null) return;
        }
        // get limit
        Bounds bounds = backgroundCollider.bounds;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, bounds.min.x, bounds.max.x);
        pos.y = Mathf.Clamp(pos.y, bounds.min.y, bounds.max.y);

        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "TaoCollect")
        {
            currentTao++;
            maxHealth++;
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collect");
            Destroy(other.gameObject, 1f);
            Debug.Log(other.gameObject.tag + "collected");
        }
    }
    private IEnumerator DestroyAfterAnimation()
    {
        // Chờ 1.5 giây - điều chỉnh thời gian theo animation Die của bạn
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
>>>>>>> a9670cd417c94c4ef4d0a3849f7267fbaac79b8a
    }
}