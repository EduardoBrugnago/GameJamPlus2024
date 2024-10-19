using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    public TargetType Nota;
    public int deathPoints = 10;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtém o componente SpriteRenderer

        HandleSprite();


    }

    private void HandleSprite()
    {
        switch (Nota)
        {
            case TargetType.Do:
                spriteRenderer.color = Color.red; // Cor para Nota 1
                break;
            case TargetType.Re:
                spriteRenderer.color = Color.green; // Cor para Nota 2
                break;
            case TargetType.Mi:
                spriteRenderer.color = Color.blue; // Cor para Nota 3
                break;
            case TargetType.Fa:
                spriteRenderer.color = Color.yellow; // Cor para Nota 3
                break;
            default:
                spriteRenderer.color = Color.red; // Cor padrão
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
