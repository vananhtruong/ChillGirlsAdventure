using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth;
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;
    private Color originalColor;
    public float flashDuration = 0.5f;

    [Header("Spawn Settings")]
    public GameObject botPrefab; // Prefab của bot
    public float respawnDelay = 2f; // Thời gian delay spawn

    private AudioSource audioSource;
    [SerializeField] private AudioClip enemyHurt;
    public int rewardMoney = 5; // Số tiền nhận được khi giết quái

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        if (spriteRend != null)
        {
            originalColor = spriteRend.color;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }


    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        currentHealth -= _damage;
        if (currentHealth > 0)
        {
            if (anim != null && anim.HasState(0, Animator.StringToHash("hurt")))
            {
                audioSource.PlayOneShot(enemyHurt);
                anim.SetTrigger("hurt");
            }
            StartCoroutine(FlashRed());
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");
                //gameObject.SetActive(false);
                Player player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
                if (player != null)
                {
                    player.coins += rewardMoney;
                    player.UpdateCoinUI();
                }
                if (GetComponent<EnemyAI>() != null) GetComponent<EnemyAI>().enabled = false;
                dead = true;
                StartCoroutine(DisappearAndRespawn());
            }
        }
    }

    //public void AddHealth(float _value)
    //{
    //    currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    //}

    private IEnumerator DisappearAfterDie()
    {
        yield return new WaitForSeconds(2.5f);

    }
    private IEnumerator FlashRed()
    {
        spriteRend.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRend.color = originalColor;
    }
    private IEnumerator DisappearAndRespawn()
    {
        Vector3 deathPosition = transform.position;
        Quaternion deathRotation = transform.rotation;

        // Tắt Collider để bot không thể bị chạm vào
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Debug.Log("Bot đã bị vô hiệu hóa, chờ " + respawnDelay + " giây để spawn bot mới...");

        yield return new WaitForSeconds(respawnDelay); // Chờ trước khi spawn bot mới

        if (botPrefab != null)
        {
            col.enabled = true;
            if (GetComponent<EnemyAI>() != null) GetComponent<EnemyAI>().enabled = true;
            GameObject newBot = Instantiate(botPrefab, deathPosition, deathRotation);
            Debug.Log("Bot mới đã spawn tại: " + deathPosition);
        }
        else
        {
            Debug.LogError("botPrefab bị NULL! Hãy kiểm tra lại.");
        }
        col.enabled = true; // Bật Collider trở lại
        Destroy(gameObject); // Xóa bot cũ sau khi spawn bot mới
    }


}