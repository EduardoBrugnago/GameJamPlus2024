using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;   // Prefab do projetil a ser disparado
    public Transform firePoint;           // Ponto de origem do disparo (geralmente � frente do player)
    public float projectileSpeed = 10f;   // Velocidade do proj�til
    public int maxShots = 3;              // N�mero m�ximo de tiros antes do intervalo
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
                // Captura da posi��o do mouse (para mira com mouse)
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
        Shoot();  // Fun��o que cria o proj�til e o dispara
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
        // Calcula a dire��o de mira
        Vector2 fireDirection = (mousePos - (Vector2)firePoint.position).normalized;

        // Se est� usando controle, sobrep�e a dire��o de mira
        if (aimDirection.magnitude > 0.1f)
        {
            fireDirection = aimDirection;
        }

        // Cria o proj�til na posi��o do ponto de disparo e com a rota��o correta
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Adiciona velocidade ao proj�til
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = fireDirection * projectileSpeed;

        // Ajusta a rota��o do proj�til para apontar na dire��o que est� sendo disparado
        
        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Teleport()
    {
        // Calcula a dire��o de mira
        Vector2 fireDirection = (mousePos - (Vector2)firePoint.position).normalized;

        if (aimDirection.magnitude > 0.1f)
        {
            fireDirection = aimDirection.normalized;
        }

        Vector2 spawnPosition = firePoint.position;

        Collider2D hitCollider = Physics2D.OverlapCircle(spawnPosition, 0.1f); // Ajuste o raio conforme necess�rio
        if (hitCollider != null && hitCollider.CompareTag("Player") == false && hitCollider.CompareTag("Enemy") == false && hitCollider.CompareTag("Destructible") == false)
        {
            canShoot = true;
            return;
        }

        // Cria o proj�til na posi��o do ponto de disparo e com a rota��o correta
        GameObject projectile = Instantiate(teleportPrefab, firePoint.position, Quaternion.identity);

        // Adiciona velocidade ao proj�til
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = fireDirection * teleportSpeed;

        // Ajusta a rota��o do proj�til para apontar na dire��o que est� sendo disparado
        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        Quaternion player_rot = gameObject.GetComponent<PlayerController>().circleTransform.rotation;
        replayControler.SaveFrameData(gameObject.transform.position, player_rot, fireDirection);
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void FakeTeleport(Vector2 replayAngle)
    {
        // Calcula a dire��o de mira
        Vector2 fireDirection = replayAngle;

        if (aimDirection.magnitude > 0.1f)
        {
            fireDirection = aimDirection.normalized;
        }

        Vector2 spawnPosition = firePoint.position;

        Collider2D hitCollider = Physics2D.OverlapCircle(spawnPosition, 0.1f); // Ajuste o raio conforme necess�rio
        if (hitCollider != null && hitCollider.CompareTag("Player") == false && hitCollider.CompareTag("Enemy") == false && hitCollider.CompareTag("Destructible") == false)
        {
            canShoot = true;
            return;
        }

        // Cria o proj�til na posi��o do ponto de disparo e com a rota��o correta
        GameObject projectile = Instantiate(teleportPrefab, firePoint.position, Quaternion.identity);

        // Adiciona velocidade ao proj�til
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = fireDirection * teleportSpeed;

        // Ajusta a rota��o do proj�til para apontar na dire��o que est� sendo disparado
        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}