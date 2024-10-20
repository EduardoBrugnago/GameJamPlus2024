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
                // Captura da posi��o do mouse (para mira com mouse)
                mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

                // Mira com controle (stick anal�gico direito)
                //float aimHorizontal = Input.GetAxisRaw("RightStickX");
                //float aimVertical = Input.GetAxisRaw("RightStickY");

                //// Se houver entrada no stick direito, define a dire��o de mira com controle
                //if (Mathf.Abs(aimHorizontal) > 0.1f || Mathf.Abs(aimVertical) > 0.1f)
                //{
                //    aimDirection = new Vector2(aimHorizontal, aimVertical).normalized;
                //}

                // Se clicar com o mouse ou pressionar o bot�o de tiro, tenta atirar

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
        Shoot();  // Fun��o que cria o proj�til e o dispara
        shotsFired++;
        yield return new WaitForSeconds(timeBetweenShots);  // Espera 2 segundos entre cada tiro

        canShoot = true; // Permite que o jogador possa atirar novamente
    }

    void Shoot()
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

        // Se est� usando controle, sobrep�e a dire��o de mira
        if (aimDirection.magnitude > 0.1f)
        {
            fireDirection = aimDirection;
        }

        Vector2 spawnPosition = firePoint.position;
 
        Collider2D hitCollider = Physics2D.OverlapCircle(spawnPosition, 0.1f); // Altere o raio conforme necess�rio
        if (hitCollider != null && hitCollider.CompareTag("Player") == false &&  hitCollider.CompareTag("Enemy") == false && hitCollider.CompareTag("Destructible") == false)
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