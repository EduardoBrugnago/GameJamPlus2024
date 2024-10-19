using System.Collections;
using UnityEngine;

public class SniperAI : EnemyAI
{
    public GameObject projectilePrefab;  // O projetil que o sniper dispara
    public float projectileSpeed = 5f;

    protected override IEnumerator Attack()
    {
        if (Vector2.Distance(player.position, transform.position) > attackRange)
        {
            // Atira no jogador
            Debug.Log(1);
            ShootProjectile();
        }
        else
        {
            // Se o jogador estiver perto, faz um ataque curto
            Debug.Log("Sniper atacando corpo a corpo!");
        }

        yield return new WaitForSeconds(1f);

        // Volta para a perseguição
        currentState = EnemyState.Perseguicao;
    }

    void ShootProjectile()
    {
        Vector2 fireDirection = (player.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = fireDirection * projectileSpeed;
    }
}
