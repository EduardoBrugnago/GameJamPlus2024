using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int dmg = 1;
    public GameObject parent = null;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (parent == collision.gameObject)
        {
            return;
        }
        if (!collision.gameObject.CompareTag("Player"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerShooting playerShooting = player.GetComponent<PlayerShooting>();
                if (playerShooting != null)
                {
                    playerShooting.canShoot = true;

                    if (collision.gameObject.CompareTag("Destructible") || collision.gameObject.CompareTag("Enemy"))
                    {
                        LifeControler life = collision.gameObject.GetComponent<LifeControler>();
                        if (life != null)
                        {
                            life.ValidateDmgTypeByTarget(false, dmg, null);
                        }
                    }
                }

            }
            Destroy(gameObject);
        }
    }
}
