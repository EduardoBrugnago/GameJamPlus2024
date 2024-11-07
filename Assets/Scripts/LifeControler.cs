using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class LifeControler : MonoBehaviour
{
    public int health = 2;
    public GerenciadorFase lvlControler;
    private GameObject player;

    public int primaryLayer;
    public Sprite primarySprite;

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
    public ShadowCaster2D shadowCaster;
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
        shadowCaster = GetComponent<ShadowCaster2D>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animationRenderer = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");

        lvlControler = FindFirstObjectByType<GerenciadorFase>();
        primarySprite = spriteRenderer.sprite;
        primaryLayer = spriteRenderer.sortingOrder;

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

    public void ResetState()
    {
        if (col != null)
            col.enabled = true;
        if (rb != null)
            rb.simulated = true;
        if (shadowCaster != null)
        {
            shadowCaster.enabled = true;
        }

        spriteRenderer.sprite = primarySprite;
        spriteRenderer.sortingOrder = primaryLayer;
        health = 1;
    }

    public void HandleDeath()
    {
        if (col != null)
            col.enabled = false;
        if (rb != null)
            rb.simulated = false;

        if(shadowCaster != null)
        {
            shadowCaster.enabled = false;
        }

        if (animationRenderer != null)
        {
            EnemyAI iaEnemy = GetComponent<EnemyAI>();
            if (iaEnemy != null)
            {
                animationRenderer.enabled = false;
                iaEnemy.currentState = EnemyAI.EnemyState.Dead;
                spriteRenderer.color = Color.white;

                spriteRenderer.material.DisableKeyword("CHANGECOLOR_ON");
                spriteRenderer.material.DisableKeyword("CHANGECOLOR2_ON");
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
        }
        else
        {
            EnemyControler enemy = GetComponent<EnemyControler>();
            if (enemy != null)
            {
                bool isValid = lvlControler.ValidateTarget(enemy);
                PlayerController playerControl = player.GetComponent<PlayerController>();

                if (isValid)
                {
                    lvlControler.DoSlowmotion();

                    bool isLastEnemy = lvlControler.ValidateLastEnemy(enemy);
                    lvlControler.HandleCombo(enemy.deathPoints);

                    if (!isLastEnemy)
                    {
                        if (player != null)
                        {
                            CinemachineImpulseSource impPlayer = player.GetComponent<CinemachineImpulseSource>();
                           
                            if (impPlayer != null)
                            {
                                CameraShakeBh.instance.CameraShake(impPlayer);
                            }
                        }
                        FindFirstObjectByType<PlayerSounds>().PlaySfx(PlayerSounds.SfxState.Teleport);
                        HandleDamege(999);
                    }
                    else
                    {

                        if (playerControl != null)
                        {
                            playerControl.DisablePlayerControl(false);
                        }
                        
                        HandleDamege(999);
                        FindFirstObjectByType<PlayerSounds>().PlaySfx(PlayerSounds.SfxState.Teleport);
                        lvlControler.HandleWin(enemy);
                    }

                }
                else
                {
                  
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
