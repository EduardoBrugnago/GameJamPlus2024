using System.Collections;
using UnityEngine;

public class SniperAI : EnemyAI
{
    public GameObject projectilePrefab;  // O projetil que o sniper dispara
    public float projectileSpeed = 5f;
    public float followDistance = 5;
    public Transform firePoint;

    protected override IEnumerator Attack()
    {

        if (Vector2.Distance(player.position, transform.position) > attackRange)
        {

            // Atira no jogador
            ShootProjectile();
        }
        else
        {
            Debug.Log(3);
            // Se o jogador estiver perto, faz um ataque curto
            Debug.Log("Sniper atacando corpo a corpo!");
        }

        yield return new WaitForSeconds(atkDelay);

        // Volta para a perseguição
        currentState = EnemyState.Perseguicao;
    }

    protected override IEnumerator ChasePlayer()
    {
        aiPath.canMove = true;
        aiPath.maxSpeed = chaseSpeed;

        while (Vector2.Distance(player.position, transform.position) < followDistance)
        {
            // Calcula o caminho até o jogador usando o A* e segue
            seeker.StartPath(transform.position, player.position);

            yield return null;

            if (Vector2.Distance(player.position, transform.position) > retreatRange)
            {
                currentState = EnemyState.Recuo;
                yield break;
            }
        }

        LifeControler controlerE = GetComponent<LifeControler>();
        if(controlerE != null)
        {
            if(controlerE.health > 0)
            {
                currentState = EnemyState.Ataque;
            }
        } else
        {
            currentState = EnemyState.Ataque;
        }
        
    }

    void ShootProjectile()
    {
        RotateTo(player.position);
        animator.Play("Enemy_range");
        Vector2 fireDirection = (player.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().parent = gameObject;
        projectile.GetComponent<Rigidbody2D>().velocity = fireDirection * projectileSpeed;
    }
    public void RotateTo(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = rotation;
    }
}
