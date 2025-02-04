using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public Animator animator;
    public Rigidbody2D rb;
    public float jumpHeight = 1f;
    public bool isGround = true;
    private float movement;
    public float moveSpeed = 5f;
    private bool facingRight = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Horizontal");
        if (movement < 0f && facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && facingRight == false)
        {
            transform.eulerAngles = new Vector3(0f, -0f, 0f);
            facingRight = true;
        }


        if (Input.GetKey(KeyCode.Space) && isGround == true)
        {
            Jump();
            isGround = false;
            //animator.SetBool("Jump", true);
        }
        if (Mathf.Abs(movement) > 0f)
        {
            //animator.SetFloat("Run", 1f);
        }
        else if (movement < 0.1f)
        {
            //animator.SetFloat("Run", 0);
        }
        Attack();

    }
    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //animator.SetTrigger("Attack");
        }
    }
    private void FixedUpdate()
    {
        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * moveSpeed;
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
            //animator.SetBool("Jump", false);
        }
    }
}
