using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    public enum BossState { Idle, Walk, Attack1, Attack2, Cast1, Cast2, Spell2, Hurt, Death }

    public Transform player;
    private Animator animator;
    public BossHealth health;
    public float moveSpeed = 2f;
    public float attackRange = 3f;
    public float maxHP = 500;
    public float currentHP;
    private BossState currentState;
    private bool isFlipped = false;
    private int attack2Count = 0;
    private bool hasCast1 = false;
    private float cooldownTime = 5f;
    private float idleTimer = 0f;
    private bool isCoolingDown = false;
    private bool isSpellDame = false;
    private bool isInvulnerable = false;
    private BossHealthCheck healthCheck;

    public float attackRadius = 4f;
    public LayerMask playerLayer;
    public int bossDame = 2;

    private AudioSource audioSource;
    [SerializeField] private AudioClip bossTele;
    private bool isAudioTele = false;
    [SerializeField] private AudioClip bossScream;
    private bool isAudioScream = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        currentHP = maxHP;
        health.UpdateHP(currentHP, maxHP);
        ChangeState(BossState.Idle);
    }

    private void Update()
    {
        LookAtPlayer();
        HandleState();
    }
    public void ChangeState(BossState newState)
    {
        if (currentState != newState)
        {
            Debug.Log($"Chuyển từ {currentState} → {newState}");
        }
        
        currentState = newState;
        idleTimer = 0f;
        isCoolingDown = (newState == BossState.Idle);
    }

    private void HandleState()
    {
        if (isCoolingDown)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= cooldownTime)
            {
                isCoolingDown = false;
                IdleState();
            }
            return;
        }

        switch (currentState)
        {
            case BossState.Idle:
                IdleState();
                break;
            case BossState.Walk:
                WalkState();
                break;
            case BossState.Attack1:
                Attack1State();
                break;
            case BossState.Attack2:
                Attack2State();
                break;
            case BossState.Cast1:
                Cast1State();
                break;
            case BossState.Cast2:
                Cast2State();
                break;
            case BossState.Spell2:
                Spell2State();
                break;
            case BossState.Hurt:
                HurtState();
                break;
            case BossState.Death:
                DeathState();
                break;
        }
    }

    private void IdleState()
    {
        animator.SetTrigger("Idle");
        //StartCoroutine(Wait5s());
        if (currentHP <= maxHP*40/100 && !hasCast1)
        {
            hasCast1 = true;
            isAudioScream = true;
            ChangeState(BossState.Cast1);
        }
        else
        {
            ChangeState(BossState.Walk);
        }
    }

    private void WalkState()
    {
        animator.SetTrigger("Walk");
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            ChangeState(currentHP <= maxHP*40/100 ? BossState.Attack2 : BossState.Attack1);
        }
    }

    private void Attack1State()
    {
        animator.SetTrigger("Attack1");
        DealDame();
        StartCoroutine(Wait1s());
        ChangeState(BossState.Idle);
    }

    private void Attack2State()
    {
        animator.SetTrigger("Attack2");
        attack2Count++;
        DealDame();
        Debug.Log($"Attack2 Count: {attack2Count}");

        if (attack2Count >= 2)
        {
            attack2Count = 0;
            isSpellDame = true;
            isAudioTele = true;
            StartCoroutine(WaitBeforeCast2());
            ChangeState(BossState.Idle);
        }
        else
        {
            ChangeState(BossState.Idle);
        }
    }

    private IEnumerator WaitBeforeCast2()
    {
        yield return new WaitForSeconds(5f);
        ChangeState(BossState.Cast2);
    }

    private IEnumerator Wait5s()
    {
        yield return new WaitForSeconds(5f);
    }
    private IEnumerator Wait1s()
    {
        yield return new WaitForSeconds(5f);
    }
    //private void Cast1State()
    //{
    //    animator.SetTrigger("Cast1");
    //    Invoke("ReturnToIdle", 5f);
    //}
    private void Cast1State()
    {
        isInvulnerable = true; // Bật trạng thái miễn nhiễm
        animator.SetTrigger("Cast1");
        if (isAudioScream)
        {
            audioSource.PlayOneShot(bossScream);
            isAudioScream = false;
        }      
        Invoke("EndCast1", 8f);
    }
    private void EndCast1()
    {
        isInvulnerable = false; // Tắt miễn nhiễm khi Cast1 kết thúc
        ReturnToIdle();
    }

    private void Cast2State()
    {
        animator.SetTrigger("Cast2");
        StartCoroutine(WaitBeforeSpell2());
    }

    private IEnumerator WaitBeforeSpell2()
    {
        yield return new WaitForSeconds(2f);
        ChangeState(BossState.Spell2);
    }
    private void Spell2State()
    {      
        Vector3 newPosition = player.position;

        newPosition.x += 0.8f;
        newPosition.y += 2.95f;
        transform.position = newPosition;
        if (isAudioTele)
        {
            audioSource.PlayOneShot(bossTele);
            isAudioTele = false;
        }       
        animator.SetTrigger("Spell2");
        
        StartCoroutine(ReturnToIdleAfterSpell2());
    }

    private IEnumerator ReturnToIdleAfterSpell2()
    {
        
        yield return new WaitForSeconds(2f);
        
        if (isSpellDame)
        {
            Debug.Log($"issssssssssss: {isSpellDame}");
            DealDame();
            isSpellDame = false;
            Debug.Log($"issssssssssss: {isSpellDame}");
        }
        ChangeState(BossState.Idle);
    }


    private void HurtState()
    {
        animator.SetTrigger("Hurt");
        Invoke("ResetHurtTrigger", 0.5f);
        Invoke("ReturnToIdle", 1f);
    }
    private void ResetHurtTrigger()
    {
        animator.ResetTrigger("Hurt");
    }
    private void DeathState()
    {
        animator.SetTrigger("Death");
        StartCoroutine(DeathSequence());
    }
    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(1f);
        BossHealthCheck.instance.ShowWin();
        Destroy(gameObject);

    }
    private void ReturnToIdle()
    {
        ChangeState(BossState.Idle);
    }

    public void TakeDamage(int damage)
    {
        if (currentHP <= 0 || isInvulnerable) return;

        currentHP -= damage;
        health.UpdateHP(currentHP, maxHP);
        if (currentHP <= 0)
        {
            ChangeState(BossState.Death);
        }
        else
        {
            ChangeState(BossState.Hurt);
        }
    }
    public void DealDame()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, attackRadius, playerLayer);

        if (hitPlayer != null && hitPlayer.GetComponent<Player>() != null)
        {
            hitPlayer.GetComponent<Player>().PlayerTakeDamage(bossDame);
        }
    }
    private bool canLookAt = true;
    public void LookAtPlayer()
    {
        if (!canLookAt) return; 

        canLookAt = false; 

        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.position = new Vector3(transform.position.x - 5, transform.position.y, transform.position.z);
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.position = new Vector3(transform.position.x + 5, transform.position.y, transform.position.z);
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
        StartCoroutine(ResetLookAt()); 
    }
    private IEnumerator ResetLookAt()
    {
        yield return new WaitForSeconds(2f); 
        canLookAt = true; 
    }
}
