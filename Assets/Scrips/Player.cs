using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;

    public float jumpHeight = 1f;
    public bool isGround = true;
    private float movement;
    public float moveSpeed = 5f;
    private bool facingRight = true;

    private int comboStep = 0;
    private Coroutine comboCoroutine;
    public float comboResetTime = 1f;

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
    //background
    private PolygonCollider2D backgroundCollider;
    //collection
    public int currentTao = 0;
    public Text TextHeart;

    void Start()
    {
    }

    void Update()
    {
        TextHeart.text = currentTao.ToString();
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

        if (Input.GetKey(KeyCode.Space) && isGround)
        {
            Jump();
            isGround = false;
            animator.SetBool("Jump", true);
        }

        if (Mathf.Abs(movement) > 0f)
        {
            animator.SetFloat("Run", 1f);
        }
        else
        {
            animator.SetFloat("Run", 0f);
        }

        Attack();
        if (maxHealth <= 0)
        {
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
            if (comboCoroutine != null)
            {
                StopCoroutine(comboCoroutine);
            }
            comboStep++;
            if (comboStep > 4)
            {
                comboStep = 1;
            }

            switch (comboStep)
            {
                case 1:
                    animator.SetTrigger("Attack");
                    break;
                case 2:
                    animator.SetTrigger("Attack1");
                    break;
                case 3:
                    animator.SetTrigger("Attack2");
                    break;
                case 4:
                    animator.SetTrigger("Attack3");
                    break;
            }

            comboCoroutine = StartCoroutine(ResetCombo());
        }
    }

    private IEnumerator ResetCombo()
    {
        yield return new WaitForSeconds(comboResetTime);
        comboStep = 0;
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
        rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            animator.SetBool("Jump", false);
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
    }
    public void PlayerTakeDamage(int damage)
    {
        if (maxHealth <= 0)
        {
            return;
        }
        maxHealth -= damage;
    }
    void Die()
    {
        Debug.Log(this.transform.name + " Die");
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originlGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower * (facingRight ? 1 : -1), 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originlGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    //huong
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
        if(other.gameObject.tag == "TaoCollect")
        {
            currentTao++;
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collect");
            Destroy(other.gameObject, 1f);
            Debug.Log(other.gameObject.tag + "collected");
        }
    }
}
