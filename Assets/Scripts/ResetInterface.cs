using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class ResetInterface : MonoBehaviour
{

    public CanvasGroup resetTime;
    public PlayerController movimentControler;

    public RectTransform bgTransaction;
    public CanvasGroup textTime;

    public CanvasGroup victoryText;

    public CanvasGroup startContainer;
    public TextMeshProUGUI startText;
    public bool interfaceOpen = false;

    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            movimentControler = player.GetComponent<PlayerController>();
        }

        OnLoadScene();
    }

 
    public void OnLoadScene()
    {
        if (movimentControler != null)
        {
            movimentControler.onAction = true;
        }

        resetTime.gameObject.SetActive(false);
        resetTime.alpha = 0;

        bool firstLoad = FindFirstObjectByType<ControlerLevels>().firstLoad;
        Debug.Log(firstLoad);
        if (!firstLoad)
        {
            startContainer.gameObject.SetActive(true);
            startContainer.alpha = 0; // Garantindo que comece invisível
            startText.text = "LIGHTS!";
            startContainer.DOFade(1, 0.5f).OnComplete(() =>
            {
                Sequence mySequence = DOTween.Sequence();
                FindFirstObjectByType<PlayerSounds>().PlaySfx(PlayerSounds.SfxState.Start);
                mySequence.Append(startText.DOFade(0, 1f)) // Esconde o texto
               .AppendCallback(() => startText.text = "CAMERA!") // Troca o texto
               .Append(startText.DOFade(1, 1f)) // Faz aparecer
               .Append(startText.DOFade(0, 0.75f)) // Esconde o texto
               .AppendCallback(() => startText.text = "ACTION!") // Troca o texto
               .Append(startText.DOFade(1, 0.75f)).Append(startText.DOFade(0, 0.75f)).AppendCallback(() =>
               {
                   startContainer.gameObject.SetActive(false);
                   bgTransaction.gameObject.SetActive(false);
                   FindFirstObjectByType<ControlerLevels>().firstLoad = true;
                   if (movimentControler != null)
                   {
                       movimentControler.onAction = false;
                       movimentControler.startTimer = true;
                   }
               });

            });
        } else
        {
            bgTransaction.gameObject.SetActive(true);
            float screenWidth = Screen.width;
            float endPositionX = -(screenWidth * 2.25f);

            bgTransaction.DOAnchorPosX(endPositionX, 1f).OnComplete(() =>
            {
                if (bgTransaction != null)
                {
                    bgTransaction.anchoredPosition = new Vector2(Screen.width, bgTransaction.anchoredPosition.y);
                }
                bgTransaction.gameObject.SetActive(false);
                if (movimentControler != null)
                {
                    movimentControler.onAction = false;
                    movimentControler.startTimer = true;
                }
                
            });
        }
        
    }
    public void Reset()
    {
        if (movimentControler != null)
        {
            movimentControler.onAction = true;
            movimentControler.startTimer = false;
        }

       
        float screenWidth = Screen.width;
        float startPositionX = screenWidth * 2.25f;

        bgTransaction.anchoredPosition = new Vector2(startPositionX, bgTransaction.anchoredPosition.y);
        bgTransaction.gameObject.SetActive(true);
        gameObject.SetActive(true);
        // Move bgTransaction para o centro do canvas
        DOVirtual.DelayedCall(0.5f,() => { bgTransaction.DOAnchorPosX(0, 1f).OnComplete(() =>
            {
                // Ativa textTime e faz o fade in
                resetTime.gameObject.SetActive(true);
                resetTime.alpha = 0; // Garantindo que comece invisível
                resetTime.DOFade(1, 1f).OnComplete(() =>
                {
                    // Espera 1 segundo
                    DOVirtual.DelayedCall(0.5f, () =>
                    {
                        // Faz o fade out
                        resetTime.DOFade(0, 1f).OnComplete(() =>
                        {
                            // Chama ResetScene no movimentControler
                            if (movimentControler != null)
                            {
                                movimentControler.ResetScene();
                            }
                        });
                    });
                });
            });
        });
    }

    public void VictoryModal()
    {
        if (movimentControler != null)
        {
            movimentControler.onAction = true;
        }

        if (victoryText == null)
        {
            return;
        }

        victoryText.alpha = 0;
        victoryText.gameObject.SetActive(true);
        FindFirstObjectByType<PlayerSounds>().PlaySfx(PlayerSounds.SfxState.End);
        victoryText.DOFade(1, 1f).OnComplete(() =>
        {
            victoryText.DOFade(0, 1f).OnComplete(() =>
            {
                if (movimentControler != null)
                {
                    FindFirstObjectByType<ControlerLevels>().LoadNextScene();
                }
            });
        });
    }
}


