using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityAction { ATTACK, DEFEND, SPECIAL, IDLE }

public class EntityAttributes : MonoBehaviour
{
    public string entityName = "Mob";

    public int maxHealth = 100;
    public int health = 100;

    public int atk = 20;
    public int def = 15;

    public EntityAction action = EntityAction.IDLE;

    private Animator animator;

    // Hashings for animation triggers
    private int animationHitTriggerHash = Animator.StringToHash("HitTrigger");
    private int animationDeathTriggerHash = Animator.StringToHash("DeathTrigger");
    private int animationApproachingEnemyHash = Animator.StringToHash("ApproachingEnemy");
    private int animationRetreatingHash = Animator.StringToHash("Retreating");

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Rebind();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            animator.SetTrigger(animationHitTriggerHash);
        else if (Input.GetKeyDown(KeyCode.X))
            animator.SetTrigger(animationDeathTriggerHash);
        else if (Input.GetKeyDown(KeyCode.C))
            animator.SetBool(animationApproachingEnemyHash, !animator.GetBool(animationApproachingEnemyHash));
    }

    public int TakeDamage(EntityAttributes attacker) {
        int damage = Mathf.Max((attacker.atk - this.def) / ((action == EntityAction.DEFEND) ? 2 : 1), 1);
        health = Mathf.Max(health - damage, 0);

        if (health > 0)
            animator.SetTrigger(animationHitTriggerHash);
        else
            animator.SetTrigger(animationDeathTriggerHash);

        return damage;
    }

    public void Die()
    {
        animator.SetTrigger(animationDeathTriggerHash);
    }

    public int AttackEnemy(EntityAttributes target) {
        animator.SetTrigger(animationHitTriggerHash);

        return 0;
        /*Vector3 initialPosition = transform.position;
        animator.SetBool(animationApproachingEnemyHash, true);
        MoveTowards(target.transform.position);
        int damage = target.TakeDamage(this);
        animator.SetBool(animationApproachingEnemyHash, false);
        animator.SetBool(animationRetreatingHash, true);
        //MoveTowards(initialPosition);
        animator.SetBool(animationRetreatingHash, false);
        return damage;*/
    }

    private void MoveTowards(Vector3 destination) {
        const float speed = 20.0f;
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }
}
