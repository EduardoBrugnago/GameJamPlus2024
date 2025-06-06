using Pathfinding;
using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Idle, Alerta, Perseguicao, Ataque, Recuo, Dead }
    public EnemyState currentState = EnemyState.Idle;

    public Transform player;
    public float visionRange = 10f;
    public float attackRange = 2f;
    public float chaseSpeed = 3f;
    public float rotationSpeed = 5f;
    public float alertTime = 2f;
    public float retreatRange = 15f;
    public float visionRadius = 5f;
    public float fieldOfViewAngle = 90f;
    public LayerMask obstacleMask;
    public GameObject melee;
    public float atkDuration = 0.4f;
    public float atkDelay = 1.0f;
    float atkTimer = 0f;
    bool isAttacking = false;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Vector2 initialPosition;
    [HideInInspector]
    public Seeker seeker;
    [HideInInspector]
    public AIPath aiPath;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        initialPosition = transform.position;

        // Inicializar componentes do A* Pathfinding
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();

        StartCoroutine(StateMachine());
    }

    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRadius);


        Vector3 leftBoundary = Quaternion.Euler(0, 0, fieldOfViewAngle / 2) * transform.up;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, -fieldOfViewAngle / 2) * transform.up;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * visionRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * visionRadius);
    }

    public IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    Idle();
                    break;

                case EnemyState.Alerta:
                    yield return StartCoroutine(Alert());
                    break;

                case EnemyState.Perseguicao:
                    yield return StartCoroutine(ChasePlayer());
                    break;

                case EnemyState.Ataque:
                    yield return StartCoroutine(Attack());
                    break;

                case EnemyState.Recuo:
                    yield return StartCoroutine(Retreat());
                    break;
                case EnemyState.Dead:
                    animator.enabled = false;
                    yield return null;
                    break;
            }
            yield return null;
        }
    }
    
    void Idle()
    {
        if (PlayerInSight())
        {
            currentState = EnemyState.Alerta;
        }
    }

    bool PlayerInSight()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= visionRadius)
        {
            float angleToPlayer = Vector2.Angle(transform.up, directionToPlayer);

            if (angleToPlayer <= fieldOfViewAngle / 2)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, distanceToPlayer, obstacleMask);

                if (hit.collider == null)
                {
                    return true;
                }
            }
        }

        return false;
    }

    IEnumerator Alert()
    {
        RotateTowards(player.position);

        yield return new WaitForSeconds(alertTime);

        currentState = EnemyState.Perseguicao;
    }

    protected virtual IEnumerator ChasePlayer()
    {
        aiPath.canMove = true;
        aiPath.maxSpeed = chaseSpeed;

        while (Vector2.Distance(player.position, transform.position) > attackRange)
        {
            // Calcula o caminho at� o jogador usando o A* e segue
            seeker.StartPath(transform.position, player.position);

            yield return null;

            if (Vector2.Distance(player.position, transform.position) > retreatRange)
            {
                currentState = EnemyState.Recuo;
                yield break;
            }
        }

        currentState = EnemyState.Ataque;
    }

    protected virtual IEnumerator Attack()
    {
        yield return new WaitForSeconds(atkDelay);

        if (Vector2.Distance(player.position, transform.position) > attackRange)
        {
            currentState = EnemyState.Perseguicao;
        }
        else if(animator.enabled)
        {
            StartCoroutine(DoAttack());
        }
    }

    IEnumerator DoAttack()
    {
        if (!isAttacking)
        {
            if (melee != null)
            {
                if (animator != null && animator.enabled)
                {
                    animator.Play("Enemy_melee");
                }
                melee.SetActive(true);

            }

            isAttacking = true;
            yield return new WaitForSeconds(atkDuration);
            isAttacking = false;
            if (melee != null)
            {
                melee.SetActive(false);
            }
        }
    }

    IEnumerator Retreat()
    {
        aiPath.canMove = true;

        while (Vector2.Distance(initialPosition, transform.position) > 0.1f)
        {
            // Caminho de volta para a posi��o inicial
            seeker.StartPath(transform.position, initialPosition);
            yield return null;
        }

        currentState = EnemyState.Idle;
    }

    public void RotateTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}