using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeControler : MonoBehaviour
{
    public int health = 2;

    public void HandleDamege(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            health = 0;
            HandleDeath();
        }
    }

    public void HandleDeath()
    {
        Destroy(gameObject);
    }
}
