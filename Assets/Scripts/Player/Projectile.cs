using UnityEngine;

public class Projectile : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Destr�i o proj�til quando colide com qualquer coisa
        Destroy(gameObject);
    }
}
