using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int dmg = 99;
    public GameObject parent = null;

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (parent == collision.gameObject)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerC = collision.gameObject.GetComponent<PlayerController>();
            if (playerC != null)
            {
                playerC.HandleDamege(dmg);

            }

            Destroy(gameObject);
        }
    }
}
