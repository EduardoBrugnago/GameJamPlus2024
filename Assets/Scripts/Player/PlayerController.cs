using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;    // Velocidade de movimento
    public Rigidbody2D rb;          // Refer�ncia ao Rigidbody2D do player
    public Camera cam;              // Refer�ncia � c�mera principal

    Vector2 movement;               // Vetor de movimenta��o
    Vector2 mousePos;               // Posi��o do mouse
    Vector2 aimDirection;           // Dire��o da mira usando controle

    void Awake()
    {
        // Pegando o Rigidbody2D automaticamente do pr�prio player
        rb = GetComponent<Rigidbody2D>();

        // Definir Rigidbody2D para Kinematic se necess�rio (para evitar qualquer f�sica indesejada)
        rb.bodyType = RigidbodyType2D.Kinematic;

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
    }

    void Update()
    {
        // Movimenta��o usando teclado ou controle
        movement.x = Input.GetAxisRaw("Horizontal");  // Eixo Horizontal (WASD, setas, ou stick anal�gico esquerdo)
        movement.y = Input.GetAxisRaw("Vertical");    // Eixo Vertical

        // Captura da posi��o do mouse (para mira com mouse)
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        //// Mira com controle (stick anal�gico direito)
        //float aimHorizontal = Input.GetAxis("RightStickX");
        //float aimVertical = Input.GetAxis("RightStickY");

        //// Se houver entrada no stick direito, define a dire��o de mira com controle
        //if (Mathf.Abs(aimHorizontal) > 0.1f || Mathf.Abs(aimVertical) > 0.1f)
        //{
        //    aimDirection = new Vector2(aimHorizontal, aimVertical).normalized;
        //}
    }
    void FixedUpdate()
    {
        // Normaliza o vetor de movimento para garantir a mesma velocidade em todas as dire��es
        if (movement.magnitude > 0)
        {
            movement = movement.normalized;
        }

        // Se n�o houver movimento, zerar a velocidade
        if (movement.magnitude == 0)
        {
            rb.velocity = Vector2.zero;  // Zera a velocidade do Rigidbody2D para parar o movimento imediatamente
        }
        else
        {
            // Aplicar movimenta��o diretamente � posi��o
            rb.velocity = movement * moveSpeed;
        }

        // Mira com o mouse (teclado + mouse)
        Vector2 lookDir = mousePos - rb.position;

        // Se est� usando controle, sobrep�e a dire��o de mira
        if (aimDirection.magnitude > 0.1f)
        {
            lookDir = aimDirection;
        }

        // Calcular o �ngulo e aplicar rota��o
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
}