using System.Collections;
using UnityEngine;

public class SniperAI : EnemyAI
{
    public GameObject projectilePrefab;  // O projetil que o sniper dispara
    public float projectileSpeed = 5f;

    protected override IEnumerator Attack()
    {
        Debug.Log(Vector2.Distance(player.position, transform.position) + " - " + attackRange);
        if (Vector2.Distance(player.position, transform.position) > attackRange)
        {
            // Atira no jogador
            ShootProjectile();
        }
        else
        {
            // Se o jogador estiver perto, faz um ataque curto
            Debug.Log("Sniper atacando corpo a corpo!");
        }

        yield return new WaitForSeconds(2f);

        // Volta para a perseguição
        currentState = EnemyState.Perseguicao;
    }

    void ShootProjectile()
    {
        Debug.Log("KRL");
        Vector2 fireDirection = (player.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = fireDirection * projectileSpeed;
    }
}
