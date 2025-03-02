﻿using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;

    public float jumpHeight = 1f;
    public bool isGround = false;
    private float movement;
    public float moveSpeed = 5f;
    private bool facingRight = true;

    public Transform attackPoint;
    public float attackRadius = 1.5f;
    public LayerMask targetLayer;

    public int maxHealth = 10;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 5f;
    private float dashingTime = 0.3f;
    private float dashingCooldown = 1f;

    [SerializeField] private TrailRenderer tr;

    // Thêm biến để đếm số lần nhảy
    private int jumpCount = 0;

    // Thêm các biến âm thanh
    private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;    // Âm thanh khi nhảy
    [SerializeField] private AudioClip attackSound;  // Âm thanh khi tấn công
    [SerializeField] private AudioClip dashSound;    // Âm thanh khi dash
    [SerializeField] private AudioClip hurtSound;    // Âm thanh khi bị thương

    void Start()
    {
        // Lấy component AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Nếu chưa có AudioSource, thêm mới
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        movement = Input.GetAxis("Horizontal");
        if (movement < 0f && facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && !facingRight)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }

        animator.SetBool("Grounded", isGround);
        animator.SetBool("Jump", !isGround && rb.linearVelocity.y > 0);

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            Jump();
        }

        // Kiểm tra nút Q để nhận damage (tạm thời)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerTakeDamage(1);
        }

        // Kiểm tra giữ chuột phải để kích hoạt Shield
        if (Input.GetMouseButtonDown(1)) // Chuột phải được nhấn
        {
            animator.SetTrigger("Shield"); // Kích hoạt trigger Shield
            animator.SetBool("IdeShield", true);
        }

        // Kiểm tra thả chuột phải để set IdleShield = false
        if (Input.GetMouseButtonUp(1)) // Chuột phải được thả
        {
            animator.SetBool("IdeShield", false); // Đặt IdleShield thành false
        }

        animator.SetFloat("Run", Mathf.Abs(movement));

        Attack();
        if (maxHealth <= 0)
        {
            Die();
        }
        if (isDashing)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
            if (attackSound != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
        }
    }

    private void FixedUpdate()
    {
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
    {
        Collider2D hitInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, targetLayer);
        if (hitInfo)
        {
            if (hitInfo.GetComponent<Health>() != null)
            {
                hitInfo.GetComponent<Health>().TakeDamage(1);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }

    public void PlayerTakeDamage(int damage)
    {
        if (maxHealth <= 0)
        {
            return;
        }
        maxHealth -= damage;
        animator.SetTrigger("Hurt");
        if (hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }
    }

    void Die()
    {
        Debug.Log(this.transform.name + " Die");
        animator.SetTrigger("Die");
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        tr.emitting = true;
        animator.SetTrigger("Dash");

        if (dashSound != null)
        {
            audioSource.PlayOneShot(dashSound);
        }

        Vector2 dashVelocity = new Vector2(transform.localScale.x * dashingPower * (facingRight ? 1 : -1), 0f);
        rb.linearVelocity = dashVelocity;

        yield return new WaitForSeconds(dashingTime);

        rb.linearVelocity = Vector2.zero;
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}