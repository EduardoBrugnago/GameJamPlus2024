using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class AimIndicator : MonoBehaviour
{
    public GameObject arrowPivot; // A ponta da seta
    public GameObject arrowHead;
    public Transform origin; // O ponto de origem da seta (Objeto pai que se move)
    public float maxDistance = 10f; // O alcance m�ximo da seta
    public LayerMask collisionMask; // Camadas que a seta deve colidir
    private LineRenderer lineRenderer;
    private SpriteRenderer arrowHeadSpriteRenderer;
    public PlayerShooting playerShooting;
    public PlayerController playerController;
    void Start()
    {
        // Inicializa o LineRenderer
        lineRenderer = arrowHead.GetComponent<LineRenderer>();
        // Define a quantidade de pontos da linha (in�cio e fim)
        lineRenderer.positionCount = 2;
        arrowHeadSpriteRenderer = arrowHead.GetComponent<SpriteRenderer>();
        maxDistance = playerShooting.teleportMaxDistance;
    }

    void Update()
    {
        if (playerController != null)
        {
            Color modifiedColor = new Color(playerController.killColor.color.r, playerController.killColor.color.g, playerController.killColor.color.b, 0.5f);
            arrowHeadSpriteRenderer.color = modifiedColor;
            lineRenderer.startColor = modifiedColor;
            lineRenderer.endColor = modifiedColor;
            if (playerController.onAction || !playerShooting.canShoot)
            {
                lineRenderer.enabled = false;
                arrowHeadSpriteRenderer.enabled = false;
            }
            else
            {
                lineRenderer.enabled = true;
                arrowHeadSpriteRenderer.enabled = true;
            }
        }

        // Captura a dire��o do anal�gico esquerdo
        Vector3 aimDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        Vector3 targetPosition;
        if (aimDirection.magnitude > 0.1f) // Se o joystick estiver em uso
        {
            // Define a posi��o alvo em rela��o ao joystick, limitada pela dist�ncia m�xima
            targetPosition = origin.position + aimDirection.normalized * maxDistance;
        }
        else
        {
            // Usa a posi��o do mouse caso o joystick n�o esteja em uso
            targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            targetPosition.z = origin.position.z;
        }

        // Calcula a dire��o e dist�ncia para o ponto alvo
        Vector3 direction = (targetPosition - origin.position).normalized;
        float distanceToTarget = Vector3.Distance(origin.position, targetPosition);
        float distance = Mathf.Min(distanceToTarget, maxDistance);

        // Realiza o Raycast para verificar colis�es
        float arrowHeadWidth = (arrowHeadSpriteRenderer.bounds.size.x - 0.2f) / 2f;
        RaycastHit2D hit = Physics2D.CircleCast(origin.position, arrowHeadWidth, direction, distance, collisionMask);

        // Se houver colis�o, ajusta a dist�ncia at� o ponto de colis�o
        if (hit.collider != null && distanceToTarget >= hit.distance)
        {
            distance = hit.distance;
        }

        // Define a posi��o da ponta da seta
        arrowPivot.transform.position = origin.position + direction * distance;

        // Calcula o �ngulo e aplica a rota��o da seta
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        arrowPivot.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Define o ponto inicial e final da linha
        lineRenderer.SetPosition(0, gameObject.transform.position);
        lineRenderer.SetPosition(1, arrowPivot.transform.position);
    }

}
