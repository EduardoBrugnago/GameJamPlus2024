using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;   // Prefab do projetil a ser disparado
    public Transform firePoint;           // Ponto de origem do disparo (geralmente à frente do player)
    public float projectileSpeed = 10f;   // Velocidade do projétil
    public int maxShots = 3;              // Número máximo de tiros antes do intervalo
    public float timeBetweenShots = 2f;   // Tempo entre cada tiro

    public int shotsFired = 0;           // Contador de tiros disparados
    public bool canShoot = true;         // Controle de tempo de disparo

    private Camera cam;
    private Vector2 mousePos;
    private Vector2 aimDirection;

    public GameObject teleportPrefab;
    public float teleportMaxDistance = 10f;
    public float teleportSpeed = 10f;
    PlayerController controlerPlayer;
    public GerenciadorFase gerenciadorFase;
    ReplayControler replayControler;

    void Start()
    {
        replayControler = FindFirstObjectByType<ReplayControler>();
        controlerPlayer = GetComponent<PlayerController>();
        cam = Camera.main;
        gerenciadorFase  = FindFirstObjectByType<GerenciadorFase>();
    }

    void Update()
    {
        if (controlerPlayer != null)
        {
            if (!controlerPlayer.onAction)
            {
                // Captura da posição do mouse (para mira com mouse)
                mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                aimDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

                if (Input.GetButtonDown("Fire1") && canShoot)
                {
                    if(gerenciadorFase != null)
                    {
                        gerenciadorFase.CancelSlowmotion();
                    }
                    GetComponent<PlayerController>().animator.Play("PShoot");
                    canShoot = false;
                    Teleport();
                }
            }
        }
 
    }

    private Coroutine shootCoroutine;

    public void ResetShoot()
    {
        canShoot = true;
        shotsFired = 0;
    }
    public void StartShooting()
    {
     
        shootCoroutine = StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine()
    {
        Shoot();  // Função que cria o projétil e o dispara
        shotsFired++;
        yield return new WaitForSeconds(timeBetweenShots);  // Espera X segundos entre cada tiro

        canShoot = true; // Permite que o jogador possa atirar novamente
    }

    public void CancelShoot()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
        }
    }


    public void Shoot()
    {
        // Calcula a direção de mira
        Vector2 fireDirection = (mousePos - (Vector2)firePoint.position).normalized;

        // Se está usando controle, sobrepõe a direção de mira
        if (aimDirection.magnitude > 0.1f)
        {
            fireDirection = aimDirection;
        }

        // Cria o projétil na posição do ponto de disparo e com a rotação correta
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Adiciona velocidade ao projétil
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = fireDirection * projectileSpeed;

        // Ajusta a rotação do projétil para apontar na direção que está sendo disparado
        
        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Teleport()
    {
        // Calcula a direção de mira
        Vector2 fireDirection = (mousePos - (Vector2)firePoint.position).normalized;

        if (aimDirection.magnitude > 0.1f)
        {
            fireDirection = aimDirection.normalized;
        }

        Vector2 spawnPosition = firePoint.position;

        Collider2D hitCollider = Physics2D.OverlapCircle(spawnPosition, 0.1f); // Ajuste o raio conforme necessário
        if (hitCollider != null && hitCollider.CompareTag("Player") == false && hitCollider.CompareTag("Enemy") == false && hitCollider.CompareTag("Destructible") == false)
        {
            canShoot = true;
            return;
        }

        // Cria o projétil na posição do ponto de disparo e com a rotação correta
        GameObject projectile = Instantiate(teleportPrefab, firePoint.position, Quaternion.identity);

        // Adiciona velocidade ao projétil
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = fireDirection * teleportSpeed;

        // Ajusta a rotação do projétil para apontar na direção que está sendo disparado
        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        Quaternion player_rot = gameObject.GetComponent<PlayerController>().circleTransform.rotation;
        replayControler.SaveFrameData(gameObject.transform.position, player_rot, fireDirection);
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void FakeTeleport(Vector2 replayAngle)
    {
        // Calcula a direção de mira
        Vector2 fireDirection = replayAngle;

        if (aimDirection.magnitude > 0.1f)
        {
            fireDirection = aimDirection.normalized;
        }

        Vector2 spawnPosition = firePoint.position;

        Collider2D hitCollider = Physics2D.OverlapCircle(spawnPosition, 0.1f); // Ajuste o raio conforme necessário
        if (hitCollider != null && hitCollider.CompareTag("Player") == false && hitCollider.CompareTag("Enemy") == false && hitCollider.CompareTag("Destructible") == false)
        {
            canShoot = true;
            return;
        }

        // Cria o projétil na posição do ponto de disparo e com a rotação correta
        GameObject projectile = Instantiate(teleportPrefab, firePoint.position, Quaternion.identity);

        // Adiciona velocidade ao projétil
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = fireDirection * teleportSpeed;

        // Ajusta a rotação do projétil para apontar na direção que está sendo disparado
        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}