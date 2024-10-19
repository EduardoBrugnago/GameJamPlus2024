using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAim : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        // Converte a posi��o do mouse de espa�o da tela para espa�o do mundo
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Ajusta a posi��o do sprite, mantendo a coordenada Z constante (0 para 2D)
        mousePosition.z = 0;

        // Define a nova posi��o do sprite
        transform.position = mousePosition;
    }
}
