using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeControler : MonoBehaviour
{
    public int health = 2;
    public GerenciadorFase lvlControler;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

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
        Destroy(gameObject);
    }

    public void ValidateDmgTypeByTarget(bool isTeleport, int dmg)
    {
        if(!isTeleport)
        {
            HandleDamege(dmg);
        } else
        {
            EnemyControler enemy = GetComponent<EnemyControler>();
            if (enemy != null)
            {
                bool isValid = lvlControler.ValidateTarget(enemy);

                if (isValid)
                {
                    bool isLastEnemy = lvlControler.ValidateLastEnemy(enemy);
                    Debug.Log(isLastEnemy + " - é o ultimo?");
                    if (!isLastEnemy)
                    {
                        HandleDamege(999);
                    } else
                    {
                        lvlControler.HandleWin(enemy);
                    }
                    
                }
                else
                {
                    PlayerController playerControl = player.GetComponent<PlayerController>();
                    if (playerControl != null)
                    {
                        playerControl.HandleDeath();
                    }
                }
            }
            else
            {
                HandleDamege(999);
            }
        }
      
    }
}
