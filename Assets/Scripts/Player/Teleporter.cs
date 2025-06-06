using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class Teleporter : MonoBehaviour
{
    public float teleportDistance = 2.0f;
    public float maxDistance = 2f;
    private Vector2 lastPosition;
    private bool hasCollided = false;
    

    private float collisionTimeLimit = 2f;
    private GameObject player;
    public GerenciadorFase gerenciadorFase;
    private void Start()
    {
        // Armazena a posi��o inicial do Teleporter
        player = GameObject.FindGameObjectWithTag("Player");
        FindFirstObjectByType<PlayerSounds>().PlaySfx(PlayerSounds.SfxState.Trow);
        PlayerShooting playerShooting = player.GetComponent<PlayerShooting>();
        gerenciadorFase = FindFirstObjectByType<GerenciadorFase>();
        if (playerShooting != null)
        {
            collisionTimeLimit = playerShooting.teleportMaxDistance / playerShooting.teleportSpeed;
        } 

        lastPosition = transform.position;
        StartCoroutine(CollisionTimer());
    }

    private void Update()
    {
        // Atualiza a �ltima posi��o a cada frame
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
                // Teleporta o jogador para a �ltima posi��o do Teleporter
               

                // Obt�m o componente PlayerShooting do jogador e ativa a vari�vel canShoot
                PlayerShooting playerShooting = player.GetComponent<PlayerShooting>();
                if (playerShooting != null)
                {

                    playerShooting.canShoot = true;
                    if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Destructible"))
                    {
                        PlayerController playerController = player.GetComponent<PlayerController>();

                        if (playerController.health <= 0)
                        {
                            return;
                        }
                        else
                        {
                            player.transform.position = collision.gameObject.transform.position;

                            playerShooting.shotsFired = 0;

                            LifeControler life = collision.gameObject.GetComponent<LifeControler>();
                            Vector2 forceDirection = (collision.gameObject.transform.position - transform.position).normalized;

                            if (life != null)
                            {
                                life.ValidateDmgTypeByTarget(true, 9999, forceDirection);
                            }


                            if (playerController != null)
                            {
                                playerController.deathTimer += 2f;
                                playerController.timerBar.transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.1f)
                                   .OnComplete(() =>
                                   {
                                       // Volta ao tamanho original em 0.5 segundos
                                       playerController.timerBar.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
                                   });
                            }
                        }
                      
                    } 
                    else
                    {
                        if(gerenciadorFase != null)
                        {
                            gerenciadorFase.LostCombo();
                        }
                        
                        FindFirstObjectByType<PlayerSounds>().PlaySfx(PlayerSounds.SfxState.Teleport2);
                        player.transform.position = lastPosition;
                    }

                   
                }    

            }

            Destroy(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator CollisionTimer()
    {
        yield return new WaitForSeconds(collisionTimeLimit);

        if (!hasCollided)
        {
            Destroy(gameObject);
            if (gerenciadorFase != null)
            {
                gerenciadorFase.LostCombo();
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerShooting playerShooting = player.GetComponent<PlayerShooting>();
                if (playerShooting != null)
                {
                    playerShooting.ResetShoot();
                }
            }
        }
    }
}


