using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public enum TargetType
{
    None,
    Do,
    Re,
    Mi,
    Fa,
}

[System.Serializable]

public class GerenciadorFase : MonoBehaviour
{
    public TargetType currentTarget;
    public int currentIndex = 0;
    // Lista de alvos
    public List<TargetType> targetList;
    public List<EnemyControler> enemyList = new List<EnemyControler>();
    public int currentCombo;
    public int currentPoints;

    private int comboHitNeeded = 3;
    private int totalHits = 0;
    private GameObject player;
    public TextMeshProUGUI pointsDisplay;
    public TextMeshProUGUI comboDisplay;
    public GameObject Ui_State;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        enemyList.AddRange(FindObjectsByType<EnemyControler>(0));
        if(targetList.Count > 0)
        {
            currentTarget = targetList[0];
            currentIndex = 0;
            if(player != null)
            {
                PlayerController controlerPlayer = player.GetComponent<PlayerController>();
                if(controlerPlayer != null)
                {
                    controlerPlayer.TargetHandler(currentTarget);
                }
            }
        }
        

    }

    public bool ValidateTarget(EnemyControler target)
    {
        
        if (target != null)
        {
            if (target.Nota == currentTarget)
            {
                return true;

            }
            else
            {
                HandleLoose();
                return false;
            }
        }
        else
        {
            return true;
        }
    }


    public bool ValidateLastEnemy(EnemyControler target)
    {
        if (currentIndex >= targetList.Count - 1)
        {
            HandleWin(target);
            return true;
        }
        else
        {
            HandleCombo(target.deathPoints);
            return false;
        }
    }
    // Método para gerenciar o BulletTime
    public void BulletTime() 
    {
        // Lógica para ativar/desativar BulletTime
    }

    // Método para gerenciar um combo de pontos
    private void HandleCombo(int targetPoints)
    {
        totalHits++;

        if (totalHits >= comboHitNeeded)
        {
            currentCombo++;
            totalHits = 0;

            if (currentCombo > 4)
                currentCombo = 4;
        }

        int pontosParaAdicionar = (int)Mathf.Pow(2, currentCombo);
        currentPoints += pontosParaAdicionar * targetPoints;
        UpdateDisplays();
        ValidateNextTarget();
    }

    private void UpdateDisplays()
    {
        // Atualiza o texto do display de pontos
        pointsDisplay.text = currentPoints.ToString();

        // Atualiza o texto do display de combo
        comboDisplay.text = currentCombo.ToString() + "x"; 
    }

    // Método para levar a falha
    private void HandleLoose()
    {
        currentCombo = 1;
        totalHits = 0; 
    }

    public void HandleWin(EnemyControler target)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Vector3 originalPosition = mainCamera.transform.position;
            Quaternion originalRotation = mainCamera.transform.rotation;

            mainCamera.transform.DOMove(originalPosition + new Vector3(0, 0, -2f), 0.5f).SetEase(Ease.OutQuad);
            mainCamera.transform.DORotate(originalRotation * new Vector3(0, 0, 8f), 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                // Destrói o target após a animação
                if (target != null)
                {
                    Destroy(target.gameObject);
                }

                ResetInterface controlerReset = Ui_State.GetComponent<ResetInterface>();
                if (controlerReset != null)
                {
                    controlerReset.VictoryModal();
                }
            });
        }
        else
        {
            if (target != null)
            {
                Destroy(target.gameObject);
            }

            ResetInterface controlerReset = Ui_State.GetComponent<ResetInterface>();
            if (controlerReset != null)
            {
                controlerReset.VictoryModal();
            }
 
        }
    }


    private void ValidateNextTarget()
    {
        TargetType proximaNota = GetNextTarget(currentTarget);
  
        if (proximaNota != 0)
        {
            bool haveEnemy = false;
            foreach (var EnemyControler in enemyList)
            {
                if (EnemyControler.Nota == proximaNota)
                {
                    haveEnemy = true;
                }
            }
            if (haveEnemy)
            {
              
                currentTarget = proximaNota;
                if (player != null)
                {
                    PlayerController controlerPlayer = player.GetComponent<PlayerController>();
                    if (controlerPlayer != null)
                    {
                        controlerPlayer.TargetHandler(currentTarget);
                    }
                }
            } else
            {
                HandleLoose();
            }
        }
       
    }

    // Método para obter a próxima nota musical
    private TargetType GetNextTarget(TargetType notaAtual)
    {
        if (currentIndex == -1 || currentIndex >= targetList.Count - 1)
        {
            currentIndex = targetList.Count - 1;
            return 0;
        }

        // Avança para o próximo alvo
        int indiceProximo = (currentIndex + 1) % targetList.Count;
        currentIndex = indiceProximo;
        return targetList[indiceProximo];
    }

}


