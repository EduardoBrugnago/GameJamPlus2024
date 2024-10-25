using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

        // Define a posição Z como a mesma do objeto de origem
        mousePosition.z = origin.position.z;

        // Calcula a direção da seta
        Vector3 direction = (mousePosition - origin.position).normalized;

        // Calcula a distância entre a origem e o mouse
        float distanceToMouse = Vector3.Distance(origin.position, mousePosition);

        // Limita a distância ao alcance máximo
        float distance = Mathf.Min(distanceToMouse, maxDistance);

        // Realiza o Raycast para verificar colisões
        float arrowHeadWidth = arrowHeadSpriteRenderer.bounds.size.x / 2f;

        // Realiza o Raycast com ajuste da largura em X
        RaycastHit2D hit = Physics2D.CircleCast(origin.position, arrowHeadWidth, direction, distance, collisionMask);

        // Se houver colisão e o mouse estiver além da colisão, ajusta a distância até o ponto de colisão
        if (hit.collider != null && distanceToMouse >= hit.distance)
        {
            distance = hit.distance;
        }


        arrowPivot.transform.position = origin.position + direction * distance;

        Vector2 lookDir = mousePosition - origin.position;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        arrowPivot.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Define o ponto inicial e final da linha, ajustando o ponto final em Y
        lineRenderer.SetPosition(0, origin.position);
        lineRenderer.SetPosition(1, arrowPivot.transform.position);


    }

}
