using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public Vector3 position;
    public Quaternion rotation;
    public string prefabName;

    public TargetType Nota;
    public int type;
}
public class ReplayControler : MonoBehaviour
{
    // Start is called before the first frame update
    private List<EnemyData> enemiesData = new List<EnemyData>();
    public GameObject enemyPrefab;
    public GameObject enemyRangedPrefab;
    public GerenciadorFase gerenciadorFase;
    ResetInterface controlerReset;

    public Vector2 playerPosition;
    public List<Quaternion> playerRotation;
    public List<Vector2> actionList;
    public List<float> actionIntervals = new List<float>();

    private float lastActionTime = 0f;

    public bool isReplay = false;
    public GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = player.transform.position;
        gerenciadorFase = FindFirstObjectByType<GerenciadorFase>();
        controlerReset = FindFirstObjectByType<ResetInterface>();
        SaveEnemiesState();
        lastActionTime = Time.unscaledTime;
    }


    public void SaveFrameData(Vector2 position, Quaternion rotation, Vector2 hasShot)
    {
        float currentTime = Time.unscaledTime;
        float interval = currentTime - lastActionTime;
        lastActionTime = currentTime;

        playerRotation.Add(rotation);
        actionList.Add(hasShot);
        actionIntervals.Add(interval);
    }


    public void SaveEnemiesState()
    {
        enemiesData.Clear();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            EnemyData data = new EnemyData();
            data.position = enemy.transform.position;
            data.rotation = enemy.transform.rotation;
            data.prefabName = enemy.name; // Caso use prefabs nomeados

            // Adicione aqui os valores dos scripts que você quer salvar
            EnemyControler enemyController = enemy.GetComponent<EnemyControler>();
            if (enemyController != null)
            {
                data.Nota = enemyController.Nota;
                SniperAI scriptSniper = enemy.GetComponent<SniperAI>();
                if (scriptSniper != null)
                {
                    data.type = 1;
                }
                else
                {
                    data.type = 0;
                }
                
            }

            enemiesData.Add(data);
        }
    }

    public void RestoreEnemies()
    {
        foreach (EnemyData data in enemiesData)
        {
            
            if(data.type == 0)
            {
                if (enemyPrefab != null)
                {
                    GameObject enemyClone = Instantiate(enemyPrefab, data.position, data.rotation);

                    // Restaure os valores dos scripts
                    EnemyControler enemyController = enemyClone.GetComponent<EnemyControler>();
                    if (enemyController != null)
                    {
                        enemyController.Nota = data.Nota;
                    }
                }
            }  else if (data.type == 1)
            {
                if (enemyRangedPrefab != null)
                {
                    GameObject enemyClone = Instantiate(enemyRangedPrefab, data.position, data.rotation);

                    // Restaure os valores dos scripts
                    EnemyControler enemyController = enemyClone.GetComponent<EnemyControler>();
                    if (enemyController != null)
                    {
                        enemyController.Nota = data.Nota;
                    }
                }
            }
        }
    }

    private IEnumerator ReplayMovement()
    {
        for (int i = 0; i < playerRotation.Count; i++)
        {
            
            player.GetComponent<PlayerController>().circleTransform.rotation = playerRotation[i];

            if (i == 0)
            {
                RestoreEnemies();
                player.transform.position = playerPosition;
            }

            player.GetComponent<PlayerShooting>().FakeTeleport(actionList[i]);


            // Espera pelo intervalo específico antes de passar para o próximo quadro
            print(actionIntervals[i]);
            yield return new WaitForSeconds(i == 0 ? 1 : actionIntervals[i]);
        }

        controlerReset.VictoryModal();
    }
    
    public void StartReplay() {
        //gerenciadorFase.gameObject.SetActive(false);
        
        PlayerController playerControler = player.GetComponent<PlayerController>();
        if (playerControler != null)
        {
            playerControler.onAction = true;
        }

        isReplay = true;
        StartCoroutine(ReplayMovement());
    }
}
