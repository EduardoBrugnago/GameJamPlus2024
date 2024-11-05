using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputEventTrace;
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
    public PlayerController playerController;

    public TextMeshProUGUI pointsDisplay;
    public TextMeshProUGUI comboDisplay;
    public GameObject Ui_State;

    public float slowdownFactor = 0.05f;
    public float slowdownLength = 3f;
    public float oldF;
    public float olfL;
    private Coroutine slowmotionCoroutine;

    ReplayControler replayControler;

    public void DoSlowmotion()
    {
        if (!replayControler.isReplay)
        {
            CancelSlowmotion();

            Time.timeScale = slowdownFactor;
            Time.fixedDeltaTime = Time.timeScale * .03f;

            slowmotionCoroutine = StartCoroutine(SlowmotionTimer());
        }   
    }

    IEnumerator SlowmotionTimer()
    {
        yield return new WaitForSeconds(0.18f); // Espera por 1 segundo
        CancelSlowmotion();
    }
    public void CancelSlowmotion()
    {
        if (slowmotionCoroutine != null)
        {
            StopCoroutine(slowmotionCoroutine); 
            slowmotionCoroutine = null; 
        }
        Time.timeScale = oldF;
        Time.fixedDeltaTime = olfL;
    }


    void Start()
    {
        replayControler = FindFirstObjectByType<ReplayControler>();
        oldF = Time.timeScale;
        olfL = Time.fixedDeltaTime;
        player = GameObject.FindGameObjectWithTag("Player");

        enemyList.AddRange(FindObjectsByType<EnemyControler>(0));
        if (targetList.Count > 0)
        {
            currentTarget = targetList[0];
            currentIndex = 0;
            if (player != null)
            {
                PlayerController controlerPlayer = player.GetComponent<PlayerController>();
                if (controlerPlayer != null)
                {
                    controlerPlayer.TargetHandler(currentTarget);
                }
            }
        }
    
        if(player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
    }

    public bool ValidateTarget(EnemyControler target)
    {
        if (replayControler.isReplay)
        {
            return true;
        }

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
        if (replayControler.isReplay)
        {
            return false;
        }

        if (currentIndex >= targetList.Count - 1)
        {
            return true;
        }
        else
        {         
            return false;
        }
    }
    // Método para gerenciar o BulletTime

    // Método para gerenciar um combo de pontos
    public void HandleCombo(int targetPoints)
    {
        if (!replayControler.isReplay)
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
        
    }

    public void LostCombo()
    {
        currentCombo = 1;
        totalHits = 0;
        UpdateDisplays();
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
        if(playerController == null)
        {
            playerController.DisablePlayerControl(true);
        }

        CinemachineVirtualCamera virtualCamera = FindFirstObjectByType<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            float originalOrthoSize = virtualCamera.m_Lens.OrthographicSize;
            float originalDutch = virtualCamera.m_Lens.Dutch;
            Sequence cameraSequence = DOTween.Sequence();
            Cinemachine3rdPersonFollow thirdPersonFollow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            if(thirdPersonFollow != null)
            {
                thirdPersonFollow.Damping.x = 0.5f;
                thirdPersonFollow.Damping.y = 0.5f;
                thirdPersonFollow.Damping.z = 0.5f;

            }

            cameraSequence
               .Join(DOTween.To(() => virtualCamera.m_Lens.Dutch, x => virtualCamera.m_Lens.Dutch = x, -12f, 0.13f))
               .Join(DOTween.To(() => virtualCamera.m_Lens.OrthographicSize, x => virtualCamera.m_Lens.OrthographicSize = x, 6f, 0.13f))
               .OnComplete(() =>
            {
                if (target != null)
                {
                    Destroy(target.gameObject);
                }

                CancelSlowmotion();
                virtualCamera.m_Lens.Dutch = originalDutch;
                virtualCamera.m_Lens.OrthographicSize = originalOrthoSize;

                // Chama o StartReplay imediatamente após o ajuste
                ResetInterface controlerReset = Ui_State.GetComponent<ResetInterface>();
                if (controlerReset != null)
                {
                    replayControler.StartReplay();
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
            }
            else
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



