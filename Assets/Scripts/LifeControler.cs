using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeControler : MonoBehaviour
{
    public int health = 2;
    public GerenciadorFase lvlControler;
    private GameObject player;

    public Sprite deadSprite; // O sprite que será trocado ao morrer
    private Collider2D col;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;


    private void Start()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (col != null)
            col.enabled = false;
        if (rb != null)
            rb.simulated = false;

        // Trocar o sprite
        if (deadSprite != null && spriteRenderer != null)
            spriteRenderer.sprite = deadSprite;
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
