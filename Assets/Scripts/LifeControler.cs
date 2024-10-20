using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeControler : MonoBehaviour
{
    public int health = 2;
    public GerenciadorFase lvlControler;
    private GameObject player;

    public Sprite deadSprite; // O sprite que será trocado ao morrer
    private Collider2D col;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animationRenderer;
    private Vector2 forceDirection;
    private bool shouldMove = false;
    private GameObject objectToMove;
    private Vector2 startPosition;
    private float pushDistance = 4f; 
    private float pushSpeed = 6f;
    public GameObject Explosion;
    void Update()
    {
        if (shouldMove && objectToMove != null)
        {
            // Calcula a nova posição
            Vector2 newPosition = Vector2.Lerp(objectToMove.transform.position,
                (Vector2)this.transform.position + forceDirection, pushSpeed * Time.deltaTime);

            // Verifica a distância percorrida
            float distanceMoved = Vector2.Distance(this.transform.position, newPosition);

            // Se a distância percorrida for menor que o limite, continue movendo
            if (distanceMoved < pushDistance)
            {
                objectToMove.transform.position = newPosition;
            }
            else
            {
                // Para o movimento após atingir a distância
                shouldMove = false;
            }
        }
    }

    private void Start()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animationRenderer = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");

        lvlControler = FindFirstObjectByType<GerenciadorFase>();
    }

    public void HandleDamege(int dmg)
    {
        
        health -= dmg;
        if (health <= 0)
        {
            health = 0;
            HandleDeath();
        }
    }

    public void HandleDeath()
    {
        if (col != null)
            col.enabled = false;
        if (rb != null)
            rb.simulated = false;

        if (animationRenderer != null)
        {
            EnemyAI iaEnemy = GetComponent<EnemyAI>();
            if (iaEnemy != null)
            {
                animationRenderer.enabled = false;
                iaEnemy.currentState = EnemyAI.EnemyState.Dead;
                spriteRenderer.color = Color.white;
            }
        }

        if (Explosion != null)
        {
            GameObject GO = Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(GO, 1f);
            
        }
        // Trocar o sprite
        if (deadSprite != null && spriteRenderer != null)
            shouldMove = true;
            spriteRenderer.sprite = deadSprite;
            spriteRenderer.sortingOrder = 1;
        
        
    }


    
    public void ValidateDmgTypeByTarget(bool isTeleport, int dmg, Vector2? dir)
    {

        
        if (!isTeleport)
        {
            HandleDamege(dmg);
        } else
        {
            EnemyControler enemy = GetComponent<EnemyControler>();
            if (enemy != null)
            {
               
                bool isValid = lvlControler.ValidateTarget(enemy);

                if (isValid)
                {
                    lvlControler.DoSlowmotion();
                    
                    bool isLastEnemy = lvlControler.ValidateLastEnemy(enemy);
                    if (!isLastEnemy)
                    {
                        FindFirstObjectByType<PlayerSounds>().PlaySfx(PlayerSounds.SfxState.Teleport);
                        HandleDamege(999);
                    } else
                    {
                        HandleDamege(999);
                        lvlControler.HandleWin(enemy);
                    }
                    
                }
                else
                {
                    PlayerController playerControl = player.GetComponent<PlayerController>();
                    if (playerControl != null)
                    {
                        playerControl.HandleDeath("You failed the choreography");
                    }
                }
            }
            else
            {
                FindFirstObjectByType<PlayerSounds>().PlaySfx(PlayerSounds.SfxState.Glass);
                HandleDamege(999);
            }
        }

        if (dir is Vector2)
        {
            forceDirection = (Vector2)dir;
        }
    }

}
