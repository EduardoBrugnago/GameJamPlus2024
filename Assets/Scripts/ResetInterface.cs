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

    public bool interfaceOpen = false;

    void Start()
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
        if(movimentControler != null)
        {
            movimentControler.onAction = true;
        }
        
        resetTime.gameObject.SetActive(false);
        resetTime.alpha = 0;
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
            }
        });
    }
    public void Reset()
    {
        if (movimentControler != null)
        {
            movimentControler.onAction = true;
        }
        gameObject.SetActive(true);
        bgTransaction.gameObject.SetActive(true);
        float screenWidth = Screen.width;
        float startPositionX = screenWidth * 2.25f;

        bgTransaction.anchoredPosition = new Vector2(startPositionX, bgTransaction.anchoredPosition.y);

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

        bgTransaction.gameObject.SetActive(true);
        float screenWidth = Screen.width;
        float startPositionX = screenWidth * 2.25f;

        bgTransaction.anchoredPosition = new Vector2(startPositionX, bgTransaction.anchoredPosition.y);

        if (victoryText == null)
        {
            return;
        }

        victoryText.alpha = 0;
        victoryText.gameObject.SetActive(true);

        victoryText.DOFade(1, 1f).OnComplete(() =>
        {
        Sequence mySequence = DOTween.Sequence();

        mySequence.Join(victoryText.transform.DORotate(new Vector3(0, 0, 16f), 1.0f, RotateMode.FastBeyond360)
        .SetLoops(1, LoopType.Yoyo)
        .OnComplete(() =>
        {
            // Inicia a rotação adicional de -16f e 16f
            victoryText.transform.DORotate(new Vector3(0, 0, -16f), 1.0f)
                .OnComplete(() =>
                {
                    victoryText.transform.DORotate(new Vector3(0, 0, 16f), 1.0f); 
                });
            }));


            // Animação de Zoom in e out
            mySequence.Join(victoryText.transform.DOScale(1.01f, 1.0f).SetEase(Ease.OutQuad));
            mySequence.AppendCallback(() =>
            {
                if (movimentControler != null)
                {
                    movimentControler.ResetScene();
                }
            });
        });
    }
}
