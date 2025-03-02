using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    float speed = 2.5f;
    float attackRange = 4f;
    float rangeSkill = 10f;
    bool attack = false;

    Transform player;
    Rigidbody2D rigidbody;
    Boss boss;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidbody = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();

        Vector2 target = new Vector2(player.position.x, rigidbody.position.y);
        Vector2 newPosition =  Vector2.MoveTowards(rigidbody.position, target, speed * Time.fixedDeltaTime);
        rigidbody.MovePosition(newPosition);

        if (Vector2.Distance(player.position, rigidbody.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
            attack = true;      
        }
        //if(attack)
        //{
            
        //    animator.SetTrigger("Run");
        //}
        
        //if (Vector2.Distance(player.position, rigidbody.position) > rangeSkill)
        //{
        //    animator.SetTrigger("Cast");
        //    animator.SetTrigger("Spell");
        //    animator.SetTrigger("Run");
        //}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        //animator.ResetTrigger("Cast");
        //animator.ResetTrigger("Spell");
        //animator.ResetTrigger("Run");

    }
}
