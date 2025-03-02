using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;    // Tốc độ di chuyển trái phải
    [SerializeField] private float flySpeed = 2f;     // Tốc độ bay lên
    private Rigidbody2D rb;
    private float horizontalInput;
    private Vector3 originalScale;

    private Camera mainCamera;
    private Vector3 mousePos;
    public bool canFire;
    private float time;
    public float timeBetweenFiring = 0.5f;
    public GameObject bullet;
    public Transform bulletTransform;
    private Animator animator;                        // Tham chiếu đến Animator

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.7f;
        originalScale = transform.localScale;
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();          // Lấy Animator component
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        MoveHorizontal();

        if (Input.GetKey(KeyCode.Space))
        {
            FlyUp();
        }

        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

        if (!canFire)
        {
            time += Time.deltaTime;
            if (time >= timeBetweenFiring)
            {
                canFire = true;
                time = 0;
            }
        }
        if (Input.GetMouseButton(0) && canFire)
        {
            canFire = false;
            // Kích hoạt trigger Shoot
            animator.SetTrigger("Shoot");
            // Đảm bảo đạn xuất hiện tại vị trí bulletTransform và theo hướng của robot
            GameObject newBullet = Instantiate(bullet, bulletTransform.position, transform.rotation);
            newBullet.GetComponent<BulletScrip>().SetDirection(mousePos);
        }
    }

    void MoveHorizontal()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    void FlyUp()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, flySpeed);
    }
}