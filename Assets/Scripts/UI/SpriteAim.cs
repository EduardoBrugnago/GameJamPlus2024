using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAim : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        // Converte a posição do mouse de espaço da tela para espaço do mundo
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Ajusta a posição do sprite, mantendo a coordenada Z constante (0 para 2D)
        mousePosition.z = 0;

        // Define a nova posição do sprite
        transform.position = mousePosition;
    }
}
