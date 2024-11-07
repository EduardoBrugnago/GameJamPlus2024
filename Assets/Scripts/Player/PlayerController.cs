using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;    // Velocidade de movimento
    public Rigidbody2D rb;          // Refer�ncia ao Rigidbody2D do player
    public Camera cam;              // Refer�ncia � c�mera principal
    public GameObject body;
    public Animator animator;

    public bool onAction = false;
    Vector2 movement;               // Vetor de movimenta��o
    Vector2 mousePos;               // Posi��o do mouse
    Vector2 aimDirection;           // Dire��o da mira usando controle

    public int health = 2;
    public int maxHealth = 2;

    public float deathTimer = 5f;
    public float maxDeathTimer = 5f;
    public Slider timerBar;
    public bool startTimer = false;
    public Transform circleTransform;
    public SpriteRenderer feedbackTarget;
    public Image killColor;
    public ResetInterface resetInterface;
    public PlayerShooting shooterControler;
    void Awake()
    {
        // Pegando o Rigidbody2D automaticamente do pr�prio player
        rb = GetComponent<Rigidbody2D>();

        // Definir Rigidbody2D para Kinematic se necess�rio (para evitar qualquer f�sica indesejada)
        //rb.bodyType = RigidbodyType2D.Kinematic;

        // Pegando a c�mera principal
        cam = Camera.main;

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D n�o encontrado no Player.");
        }

        if (cam == null)
        {
            Debug.LogError("C�mera principal n�o encontrada.");
        }

        circleTransform = transform.Find("Circle");
        shooterControler = GetComponent<PlayerShooting>();
        CinemachineVirtualCamera virtualCamera = FindFirstObjectByType<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            virtualCamera.Follow = transform;
        }

        //GameObject resetCanvas = GameObject.FindGameObjectWithTag("Reset");
        //if (resetCanvas != null)
        //{
        //    resetInterface = resetCanvas.GetComponent<ResetInterface>();
        //}
    }

    void Update()
    {
        // Movimentação usando teclado ou controle
        if (!onAction)
        {
            // Captura a posição do mouse em coordenadas de mundo
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            // Captura a direção de mira com o controle, caso esteja em uso
            aimDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (startTimer)
            {
                HandleTimer();
            }
        }
    }

    void FixedUpdate()
    {
        if (!onAction)
        {
            Vector2 lookDir;
            
            if (aimDirection.magnitude > 0.1f)
            {
                // Normaliza a direção do analógico esquerdo para garantir precisão
                lookDir = aimDirection.normalized;
            }
            else
            {
                // Alternativamente, use a posição do mouse se o analógico não estiver em uso
                lookDir = mousePos - rb.position;
            }

            // Calcula o ângulo para rotação completa em 360 graus
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

            // Aplica a rotação no transform
            circleTransform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public void HandleTimer()
    {
        if (deathTimer > maxDeathTimer)
        {
            deathTimer = maxDeathTimer;
        }

        deathTimer -= Time.deltaTime;

        if (timerBar != null)
        {
            timerBar.value = deathTimer / maxDeathTimer;
        }

        if (deathTimer <= 0)
        {
            HandleDeath("Your time is up!");
            deathTimer = maxDeathTimer;
        }
    }
    public void HandleDeath(string msg)
    {
        if (resetInterface != null)
        {
            onAction = true;
            resetInterface.Reset(msg);
        }
        else
        {
            ResetScene();
        }

    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void HandleDamege(int dmg)
    {
        if (!onAction)
        {
            FindFirstObjectByType<PlayerSounds>().PlaySfx(PlayerSounds.SfxState.DmgPlayer);
            feedbackTarget.material.DOFloat(1f, "_HitEffectBlend", 0.25f).OnComplete(() =>
            {
                feedbackTarget.material.DOFloat(0f, "_HitEffectBlend", 0.25f);
            });
            health -= dmg;
            if (health <= 0)
            {
                health = 0;
                HandleDeath("You Died!");
            }
        }

    }

    public void TargetHandler(TargetType target)
    {
        feedbackTarget.material.EnableKeyword("CHANGECOLOR_ON");
        feedbackTarget.material.EnableKeyword("CHANGECOLOR2_ON");
        feedbackTarget.material.EnableKeyword("CHANGECOLOR3_ON");
        feedbackTarget.material.SetColor("_ColorChangeTarget", new Color(71f / 255f, 169f / 255f, 16f / 255f));
        //feedbackTarget.material.SetColor("_ColorChangeTarget2", new Color(109f / 255f, 9f / 255f, 116f / 255f));
        feedbackTarget.material.SetColor("_ColorChangeTarget3", new Color(188f / 255f, 188f / 255f, 188f / 255f));
        feedbackTarget.material.SetFloat("_ColorChangeTolerance", 0.68f);
        feedbackTarget.material.SetFloat("_ColorChangeTolerance2", 0.766f);
        feedbackTarget.material.SetFloat("_ColorChangeTolerance3", 0.25f);
        if (feedbackTarget != null)
        {
            switch (target)
            {
                case TargetType.Do:
                    feedbackTarget.material.SetColor("_ColorChangeNewCol", Color.red);
                    //feedbackTarget.material.SetColor("_ColorChangeNewCol2", Color.red);
                    feedbackTarget.material.SetColor("_ColorChangeNewCol3", Color.red);
                    killColor.color = Color.red; // Cor para Nota 1
                    break;
                case TargetType.Re:

                    feedbackTarget.material.SetColor("_ColorChangeNewCol", Color.green);
                    //feedbackTarget.material.SetColor("_ColorChangeNewCol2", Color.green);
                    feedbackTarget.material.SetColor("_ColorChangeNewCol3", Color.green);
                    killColor.color = Color.green; // Cor para Nota 1
                    break;
                case TargetType.Mi:
                    feedbackTarget.material.SetColor("_ColorChangeNewCol", Color.blue);
                    //feedbackTarget.material.SetColor("_ColorChangeNewCol2", Color.blue);
                    feedbackTarget.material.SetColor("_ColorChangeNewCol3", Color.blue);
                    killColor.color = Color.blue; // Cor para Nota 1
                    break;
                case TargetType.Fa:
                    feedbackTarget.material.SetColor("_ColorChangeNewCol", Color.yellow);
                    //feedbackTarget.material.SetColor("_ColorChangeNewCol2", Color.green);
                    feedbackTarget.material.SetColor("_ColorChangeNewCol3", Color.yellow);
                    killColor.color = Color.yellow; // Cor para Nota 1
                    break;
            }
        }
    }
    
    public void DisablePlayerControl(bool canDoThing)
    {
        onAction = !canDoThing;
        
        if(shooterControler != null)
        {
            shooterControler.canShoot = canDoThing;
        }
    }
}