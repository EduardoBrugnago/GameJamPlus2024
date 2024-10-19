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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        enemyList.AddRange(FindObjectsByType<EnemyControler>(0));
        if(targetList.Count > 0)
        {
            currentTarget = targetList[0];
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
            Debug.Log(target.Nota);
            if (target.Nota == currentTarget)
            {
                HandleCombo(target.deathPoints);
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

    // Método para gerenciar o BulletTime
    public void BulletTime() 
    {
        // Lógica para ativar/desativar BulletTime
    }

    // Método para gerenciar um combo de pontos
    private void HandleCombo(int targetPoints)
    {
        Debug.Log("Calculando pontos");
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

    private void HandleWin()
    {
        Debug.Log("Ganhou: " + currentPoints);
    }


    private void ValidateNextTarget()
    {
        TargetType proximaNota = GetNextTarget(currentTarget);
        Debug.Log("Procurando proximo alvo");
        if (proximaNota == 0)
        {
            HandleWin();
        } else
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
              
                currentTarget = proximaNota ;
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
        Debug.Log("Procurando proximo alvo e limpando array");
        int indiceAtual = targetList.IndexOf(notaAtual);

        // Se a nota não estiver na lista, retorne null
        if (indiceAtual == -1)
        {
            return 0;
        }

        // Avança para o próximo alvo
        int indiceProximo = (indiceAtual + 1) % targetList.Count;
        return targetList[indiceProximo];
    }

}


