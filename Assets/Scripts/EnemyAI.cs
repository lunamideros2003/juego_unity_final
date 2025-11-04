using UnityEngine;
using UnityEngine.AI; // Para la navegación

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Referencias")]
    public Transform target;         // Jugador
    public Enemy enemyStats;         // Script Enemy del enemigo
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Ataque")]
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;

    [Header("Detección")]
    public float detectionRange = 15f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (enemyStats == null || enemyStats.enabled == false)
            return;

        if (target == null)
            return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= detectionRange)
        {
            agent.SetDestination(target.position);

            if (animator != null)
                animator.SetBool("isMoving", true);

            // Si está lo suficientemente cerca, atacar
            if (distance <= attackRange)
            {
                if (Time.time >= nextAttackTime)
                {
                    Attack();
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
        }
        else
        {
            if (animator != null)
                animator.SetBool("isMoving", false);
        }
    }

    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("Attack");

        Debug.Log("🗡️ Enemigo ataca al jugador");

        // Hacer daño al jugador
        PlayerHealth player = target.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(attackDamage);
        }
    }
}
