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

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        if (spriteRend != null)
        {
            originalColor = spriteRend.color;
        }
        botPrefab=gameObject;
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
        Vector3 deathPosition = transform.position; // Lưu lại vị trí bot chết
        Quaternion deathRotation = transform.rotation; // Lưu lại hướng quay của bot

        Debug.Log("Bot Prefab day ne: " + (botPrefab != null ? botPrefab.name : "NULL"));
        Debug.Log("Bot deathPosition day ne: " + deathPosition);
        Debug.Log("Bot deathRotation day ne: " + deathRotation);

        // Spawn bot mới ở vị trí cũ
        if (botPrefab != null)
        {

            Debug.Log("Spawn bot mới sau " + respawnDelay + " giây...");
            yield return new WaitForSeconds(respawnDelay);
            try
            {
                GameObject newBot = Instantiate(botPrefab, deathPosition, deathRotation);
                Debug.Log("Bot mới đã spawn thành công tại: " + deathPosition);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Lỗi khi Instantiate: " + e.Message);
            }

            Destroy(gameObject);
        }
    }
    
}