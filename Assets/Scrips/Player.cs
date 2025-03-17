using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public CharacterControllerManager controllerManager; // Thêm tham chiếu đến manager

    public float jumpHeight = 1f;
    public bool isGround = false;
    private float movement;
    public float moveSpeed = 5f;
    private bool facingRight = true;

    public Transform attackPoint;
    public float attackRadius = 1.5f;
    public LayerMask targetLayer;

   // public int maxHealth = 10;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 5f;
    private float dashingTime = 0.3f;
    private float dashingCooldown = 1f;

    [SerializeField] private TrailRenderer tr;

    private int jumpCount = 0;

    //background
    private PolygonCollider2D backgroundCollider;
    //collection
   // public int currentTao = 0;
  //  public Text TextHeart;
    private SceneController sceneController;

    private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip hurtSound;

    private bool isShieldActive = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        //   currentTao = maxHealth;
        sceneController = SceneController.instance;
        if(sceneController == null)
        {
            Debug.LogError("sceneController not found!");
        }
    }

    void Update()
    {


        if (transform.position.y < -12f)
        {
            PlayerTakeDamage(sceneController.currentTao); // Giảm toàn bộ máu
        }
        // sceneController.UpdateUI();
        //  TextHeart.text = currentTao.ToString();
        if (!enabled) return; // Chỉ chạy khi component được bật

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
        //if (Input.GetMouseButtonDown(0))
        //{
        //    // Kiểm tra xem chuột có đang bấm vào UI không
        //    if (EventSystem.current.IsPointerOverGameObject())
        //    {
        //        return; // Nếu đang bấm UI, không thực hiện chém
        //    }

        //    Attack(); // Nếu không, thực hiện chém
        //}
        Attack();
        //if (maxHealth <= 0)
        //{
        //    Die();
        //}
        if (sceneController.currentTao <= 0)
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
            PlayerAttack();
            PlayerAttackBoss();
            if (attackSound != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
        }
    }

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
        //if (collision.gameObject.tag == "TaoCollect")
        //{
        //    currentTao++;
        //    maxHealth++;
        //    collision.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collect");
        //    Destroy(collision.gameObject, 1f);
        //    Debug.Log(collision.gameObject.tag + "collected");
        //}
        if (collision.gameObject.CompareTag("TaoCollect"))
        {
            sceneController.AddTao();
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
    {
        Collider2D hitInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, targetLayer);
        if (hitInfo)
        {
            if (hitInfo.GetComponent<Health>() != null)
            {
                Debug.Log("We hit " + hitInfo.name);
                hitInfo.GetComponent<Health>().TakeDamage(sceneController.playerDame);
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
                hitInfo.GetComponent<BossAI>().TakeDamage(sceneController.playerDame);
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
        if (isShieldActive)
        {
            Debug.Log("Shield blocked damage!");
            return;
        }
        //if (maxHealth <= 0)
        //{
        //    return;
        //}
        //maxHealth -= damage;
        // currentTao -= damage;
        //SceneController.instance.maxHealth -= damage;
        //SceneController.instance.currentTao -= damage;
        sceneController.TakeDamage(damage);
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
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        enabled = false;
        StartCoroutine(DestroyAfterAnimation());
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
        if (other.gameObject.tag == "TaoCollect")
        {
            //currentTao++;
            //maxHealth++;
            //other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collect");
            //Destroy(other.gameObject, 1f);
            //Debug.Log(other.gameObject.tag + "collected");

            sceneController.AddTao();
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
    }
    

}