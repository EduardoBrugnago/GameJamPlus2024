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
    public float maxDistance = 10f; // O alcance máximo da seta
    public LayerMask collisionMask; // Camadas que a seta deve colidir
    private LineRenderer lineRenderer;
    private SpriteRenderer arrowHeadSpriteRenderer;
    public PlayerShooting playerShooting;
    public PlayerController playerController;
    void Start()
    {
        // Inicializa o LineRenderer
        lineRenderer = arrowHead.GetComponent<LineRenderer>();
        // Define a quantidade de pontos da linha (início e fim)
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

        // Captura a direção do analógico esquerdo
        Vector3 aimDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        Vector3 targetPosition;
        if (aimDirection.magnitude > 0.1f) // Se o joystick estiver em uso
        {
            // Define a posição alvo em relação ao joystick, limitada pela distância máxima
            targetPosition = origin.position + aimDirection.normalized * maxDistance;
        }
        else
        {
            // Usa a posição do mouse caso o joystick não esteja em uso
            targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            targetPosition.z = origin.position.z;
        }

        // Calcula a direção e distância para o ponto alvo
        Vector3 direction = (targetPosition - origin.position).normalized;
        float distanceToTarget = Vector3.Distance(origin.position, targetPosition);
        float distance = Mathf.Min(distanceToTarget, maxDistance);

        // Realiza o Raycast para verificar colisões
        float arrowHeadWidth = (arrowHeadSpriteRenderer.bounds.size.x - 0.2f) / 2f;
        RaycastHit2D hit = Physics2D.CircleCast(origin.position, arrowHeadWidth, direction, distance, collisionMask);

        // Se houver colisão, ajusta a distância até o ponto de colisão
        if (hit.collider != null && distanceToTarget >= hit.distance)
        {
            distance = hit.distance;
        }

        // Define a posição da ponta da seta
        arrowPivot.transform.position = origin.position + direction * distance;

        // Calcula o ângulo e aplica a rotação da seta
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        arrowPivot.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Define o ponto inicial e final da linha
        lineRenderer.SetPosition(0, gameObject.transform.position);
        lineRenderer.SetPosition(1, arrowPivot.transform.position);
    }

}
