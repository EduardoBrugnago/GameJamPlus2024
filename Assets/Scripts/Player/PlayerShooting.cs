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
    void Start()
    {
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

                // Mira com controle (stick analógico direito)
                //float aimHorizontal = Input.GetAxisRaw("RightStickX");
                //float aimVertical = Input.GetAxisRaw("RightStickY");

                //// Se houver entrada no stick direito, define a direção de mira com controle
                //if (Mathf.Abs(aimHorizontal) > 0.1f || Mathf.Abs(aimVertical) > 0.1f)
                //{
                //    aimDirection = new Vector2(aimHorizontal, aimVertical).normalized;
                //}

                // Se clicar com o mouse ou pressionar o botão de tiro, tenta atirar

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

    IEnumerator ShootCoroutine()
    {
        Shoot();  // Função que cria o projétil e o dispara
        shotsFired++;
        yield return new WaitForSeconds(timeBetweenShots);  // Espera 2 segundos entre cada tiro

        canShoot = true; // Permite que o jogador possa atirar novamente
    }

    void Shoot()
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

        // Se está usando controle, sobrepõe a direção de mira
        if (aimDirection.magnitude > 0.1f)
        {
            fireDirection = aimDirection;
        }

        Vector2 spawnPosition = firePoint.position;
 
        Collider2D hitCollider = Physics2D.OverlapCircle(spawnPosition, 0.1f); // Altere o raio conforme necessário
        if (hitCollider != null && hitCollider.CompareTag("Player") == false &&  hitCollider.CompareTag("Enemy") == false && hitCollider.CompareTag("Destructible") == false)
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