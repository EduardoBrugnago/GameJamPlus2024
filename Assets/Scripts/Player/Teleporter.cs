using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Teleporter : MonoBehaviour
{
    public float teleportDistance = 2.0f;
    private Vector2 lastPosition;
    private bool hasCollided = false;
    public float maxDistance = 2f;

    private float collisionTimeLimit = 2f;
    private GameObject player;
    public GerenciadorFase gerenciadorFase;
    private void Start()
    {
        // Armazena a posição inicial do Teleporter
        player = GameObject.FindGameObjectWithTag("Player");
        FindFirstObjectByType<PlayerSounds>().PlaySfx(PlayerSounds.SfxState.Trow);
        PlayerShooting playerShooting = player.GetComponent<PlayerShooting>();
        if(playerShooting != null)
        {
            collisionTimeLimit = playerShooting.teleportMaxDistance / playerShooting.teleportSpeed;
        } 

        lastPosition = transform.position;
        StartCoroutine(CollisionTimer());
    }

    private void Update()
    {
        // Atualiza a última posição a cada frame
        lastPosition = transform.position;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            hasCollided = true;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // Teleporta o jogador para a última posição do Teleporter
               

                // Obtém o componente PlayerShooting do jogador e ativa a variável canShoot
                PlayerShooting playerShooting = player.GetComponent<PlayerShooting>();
                if (playerShooting != null)
                {
                    playerShooting.canShoot = true;

                    if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Destructible"))
                    {
                        player.transform.position = collision.gameObject.transform.position;

                        playerShooting.shotsFired = 0;

                        LifeControler life = collision.gameObject.GetComponent<LifeControler>();
                        Vector2 forceDirection = (collision.gameObject.transform.position - transform.position).normalized;

                        if (life != null)
                        {
                            life.ValidateDmgTypeByTarget(true, 9999, forceDirection);
                        }

                        PlayerController playerController = player.GetComponent<PlayerController>();
                        if (playerController != null)
                        {
                            playerController.deathTimer += 2f;
                        }
                    } else
                    {
                        FindFirstObjectByType<PlayerSounds>().PlaySfx(PlayerSounds.SfxState.Teleport2);
                        player.transform.position = lastPosition;
                    }
                }

               

            }

            Destroy(gameObject);
        }
    }

    private IEnumerator CollisionTimer()
    {
        yield return new WaitForSeconds(collisionTimeLimit);

        if (!hasCollided)
        {
            Destroy(gameObject);
            PlayerController playerControl = player.GetComponent<PlayerController>();
            if (playerControl != null)
            {
                playerControl.HandleDeath();
            }
        }
    }


}


