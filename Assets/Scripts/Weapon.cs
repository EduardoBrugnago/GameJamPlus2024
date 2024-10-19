using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 1f;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyControler enemy = collision.GetComponent<EnemyControler>();
        if (enemy != null)
        {
            enemy.GetComponent<LifeControler>().HandleDamege(2);
        }

        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.HandleDamege(2);
        }
    }
}
