using UnityEngine;

public class Projectile : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Destrói o projétil quando colide com qualquer coisa
        Destroy(gameObject);
    }
}
