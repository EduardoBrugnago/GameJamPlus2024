using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;    // Velocidade de movimento
    public Rigidbody2D rb;          // Referência ao Rigidbody2D do player
    public Camera cam;              // Referência à câmera principal

    Vector2 movement;               // Vetor de movimentação
    Vector2 mousePos;               // Posição do mouse
    Vector2 aimDirection;           // Direção da mira usando controle

    void Awake()
    {
        // Pegando o Rigidbody2D automaticamente do próprio player
        rb = GetComponent<Rigidbody2D>();

        // Definir Rigidbody2D para Kinematic se necessário (para evitar qualquer física indesejada)
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Pegando a câmera principal
        cam = Camera.main;

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D não encontrado no Player.");
        }

        if (cam == null)
        {
            Debug.LogError("Câmera principal não encontrada.");
        }
    }

    void Update()
    {
        // Movimentação usando teclado ou controle
        movement.x = Input.GetAxisRaw("Horizontal");  // Eixo Horizontal (WASD, setas, ou stick analógico esquerdo)
        movement.y = Input.GetAxisRaw("Vertical");    // Eixo Vertical

        // Captura da posição do mouse (para mira com mouse)
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        //// Mira com controle (stick analógico direito)
        //float aimHorizontal = Input.GetAxis("RightStickX");
        //float aimVertical = Input.GetAxis("RightStickY");

        //// Se houver entrada no stick direito, define a direção de mira com controle
        //if (Mathf.Abs(aimHorizontal) > 0.1f || Mathf.Abs(aimVertical) > 0.1f)
        //{
        //    aimDirection = new Vector2(aimHorizontal, aimVertical).normalized;
        //}
    }
    void FixedUpdate()
    {
        // Normaliza o vetor de movimento para garantir a mesma velocidade em todas as direções
        if (movement.magnitude > 0)
        {
            movement = movement.normalized;
        }

        // Se não houver movimento, zerar a velocidade
        if (movement.magnitude == 0)
        {
            rb.velocity = Vector2.zero;  // Zera a velocidade do Rigidbody2D para parar o movimento imediatamente
        }
        else
        {
            // Aplicar movimentação diretamente à posição
            rb.velocity = movement * moveSpeed;
        }

        // Mira com o mouse (teclado + mouse)
        Vector2 lookDir = mousePos - rb.position;

        // Se está usando controle, sobrepõe a direção de mira
        if (aimDirection.magnitude > 0.1f)
        {
            lookDir = aimDirection;
        }

        // Calcular o ângulo e aplicar rotação
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
}