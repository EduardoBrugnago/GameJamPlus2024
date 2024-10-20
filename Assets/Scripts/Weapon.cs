using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 1f;
    public bool isPlayer = false;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlayer)
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.HandleDamege(999);
            }
        } else
        {
            EnemyControler enemy = collision.GetComponent<EnemyControler>();
            if (enemy != null)
            {
                enemy.GetComponent<LifeControler>().HandleDamege(999);
            }
        }
       
    }
}
