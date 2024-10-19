using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;   // Prefab do projetil a ser disparado
    public Transform firePoint;           // Ponto de origem do disparo (geralmente � frente do player)
    public float projectileSpeed = 10f;   // Velocidade do proj�til
    public int maxShots = 3;              // N�mero m�ximo de tiros antes do intervalo
    public float timeBetweenShots = 2f;   // Tempo entre cada tiro

    private int shotsFired = 0;           // Contador de tiros disparados
    private bool canShoot = true;         // Controle de tempo de disparo

    private Camera cam;
    private Vector2 mousePos;
    private Vector2 aimDirection;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
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
        if (Input.GetButtonDown("Fire1") && canShoot && shotsFired < maxShots)
        {
            canShoot = false;
            StartCoroutine(ShootCoroutine());
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
}