using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    public TargetType Nota;
    public int deathPoints = 10;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obt�m o componente SpriteRenderer
        HandleSprite();
    }

    private void HandleSprite()
    {
        spriteRenderer.material.SetColor("_ColorChangeTarget", new Color(159f / 255f, 159f / 255f, 159f / 255f));
        spriteRenderer.material.SetColor("_ColorChangeTarget2", new Color(38f / 255f, 38f / 255f, 38f / 255f));
        spriteRenderer.material.SetFloat("_ColorChangeTolerance", 0.594f);
        switch (Nota)
        {
            case TargetType.Do:
                spriteRenderer.material.SetColor("_ColorChangeNewCol", Color.red);
                spriteRenderer.material.SetColor("_ColorChangeNewCol2", Color.red);
                break;
            case TargetType.Re:
                spriteRenderer.material.SetColor("_ColorChangeNewCol", Color.green);
                spriteRenderer.material.SetColor("_ColorChangeNewCol2", Color.green);
                break;
            case TargetType.Mi:
                spriteRenderer.material.SetColor("_ColorChangeNewCol", Color.blue);
                spriteRenderer.material.SetColor("_ColorChangeNewCol2", Color.blue);
                break;
            case TargetType.Fa:
                spriteRenderer.material.SetColor("_ColorChangeNewCol", new Color(1, 0, 1));
                spriteRenderer.material.SetColor("_ColorChangeNewCol2", new Color(1, 0, 1));
                break;
            default:
                spriteRenderer.material.SetColor("_ColorChangeNewCol", Color.red);
                spriteRenderer.material.SetColor("_ColorChangeNewCol2", Color.red);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
