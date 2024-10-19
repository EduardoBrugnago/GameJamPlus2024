using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeControler : MonoBehaviour
{
    public int health = 2;
    public GerenciadorFase lvlControler;

    public void Start()
    {
        lvlControler = FindFirstObjectByType<GerenciadorFase>();
    }

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
        EnemyControler enemy = GetComponent<EnemyControler>();
        if (enemy != null)
        {
            lvlControler.ValidateTarget(enemy);
        }
        Destroy(gameObject);
    }
}
