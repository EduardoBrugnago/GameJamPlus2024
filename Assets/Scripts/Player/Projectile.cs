using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int dmg = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
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
                        if(life != null)
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
