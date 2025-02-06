using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        startingHealth--;
        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");
                //GetComponent<PlayerMovement>().enabled = false;
                dead = true;
                StartCoroutine(DisappearAfterDie());
            }
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator DisappearAfterDie()
    {
        // Wait for the "die" animation to finish (assume it takes 1 second here)
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        // Alternatively, you could specify a fixed time if the animation length is known
        // yield return new WaitForSeconds(1f);

        // Deactivate or destroy the GameObject
        gameObject.SetActive(false);
        // Or: Destroy(gameObject);
    }
}
