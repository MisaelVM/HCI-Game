using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { IDLE, CHASING, ATTACKING, DEAD };

public class EnemyBehaviourScript : MonoBehaviour
{
    [SerializeField] private float attackRange = 15.0f;
    //[SerializeField] private float speed = 0.1f;
    [SerializeField] private GameObject player;

    private Animator animator;
    private static readonly int GotHit = Animator.StringToHash("GotHit");
    private static readonly int HitSourceRight = Animator.StringToHash("HitSourceRight");
    private static readonly int DeathTrigger = Animator.StringToHash("DeathTrigger");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int Attack = Animator.StringToHash("Attack");

    private NavMeshAgent navMeshAgent;

    [SerializeField] private EnemyScript enemyScript;
    private EnemyState state = EnemyState.IDLE;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.DEAD || state == EnemyState.ATTACKING)
            return;

        if (state == EnemyState.IDLE)
            navMeshAgent.isStopped = true;

        if (state == EnemyState.CHASING)
            ChasePlayer();
        else
            DetectPlayer();

        /*if (state == EnemyState.ATTACKING)
            navMeshAgent.isStopped = true;*/
        //StartCoroutine(AttackPlayer());

    }

    private void OnTriggerEnter(Collider other) {
        if (state == EnemyState.DEAD)
            return;

        var damager = other.gameObject.GetComponent<Damager>();
        if (damager == null || damager.damages == Entities.PLAYER)
            return;

        Debug.Log("Call");
        animator.SetFloat(HitSourceRight, FromRight(other.ClosestPoint(transform.position)));
        bool isStillAlive = enemyScript.TakeDamage(damager.damage);
        if (isStillAlive)
            animator.SetTrigger(GotHit);
        else {
            //navMeshAgent.isStopped = true;
            navMeshAgent.enabled = false;
            StopAllCoroutines();
            state = EnemyState.DEAD;
            animator.SetTrigger(DeathTrigger);
            StartCoroutine(EnemyDeath());
        }
    }

    private void OnTriggerExit(Collider other) {
        animator.ResetTrigger(GotHit);
    }

    private float FromRight(Vector3 other) {
        var right = transform.right;
        var dot = Vector3.Dot(right.normalized, other.normalized);
        dot = (dot / 2.0f) + 0.5f; // Clamp
        Debug.Log($"from right: {dot}");
        return dot;
    }

    private void DetectPlayer() {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= attackRange) {
            state = EnemyState.CHASING;
            animator.SetBool(IsMoving, true);
        }
        else
            state = EnemyState.IDLE;
    }

    private void ChasePlayer() {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= 2.0f) {
            //navMeshAgent.isStopped = true;
            state = EnemyState.ATTACKING;
            StartCoroutine(AttackPlayer());
            //Debug.Log("Attack");
            return;
        }
        else if (distance > attackRange) {
            animator.SetBool(IsMoving, false);
            state = EnemyState.IDLE;
            return;
        }

        navMeshAgent.isStopped = false;
        navMeshAgent.destination = player.transform.position;
        Vector3 direction = transform.position - player.transform.position;
        Quaternion orientation = Quaternion.LookRotation(direction.normalized);
        orientation.Normalize();
        transform.rotation = Quaternion.Euler(0, orientation.eulerAngles.y, orientation.eulerAngles.z);
    }

    IEnumerator AttackPlayer() {
        navMeshAgent.isStopped = true;
        const float animDuration = 1.5f;
        float elapsedTime = 0.0f;
        animator.SetTrigger(Attack);
        animator.SetBool(IsMoving, false);
        while (elapsedTime < animDuration) {
            elapsedTime += Time.deltaTime;
            Vector3 direction = transform.position - player.transform.position;
            Quaternion orientation = Quaternion.LookRotation(direction.normalized);
            orientation.Normalize();
            transform.rotation = Quaternion.Euler(0, orientation.eulerAngles.y, orientation.eulerAngles.z);
            yield return null;
        }
        state = EnemyState.IDLE;
    }

    IEnumerator EnemyDeath() {
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(4.0f);
        enemyScript.DisableHealthBar();
        yield return new WaitForSeconds(1.0f);
        const float animDuration = 2.0f;
        float elapsedTime = 0.0f;
        Vector3 from = transform.position;
        Vector3 to = from + Vector3.down;
        while (elapsedTime < animDuration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animDuration;
            transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
        Destroy(gameObject);
    }
}
