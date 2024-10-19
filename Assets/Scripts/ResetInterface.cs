using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class ResetInterface : MonoBehaviour
{

    public GameObject resetTime;
    public GameObject playerReset;

    public Image imageToAnimatePlayer;
    public Image imageToAnimateTime;

    public PlayerController movimentControler;
    public RectTransform bgTransaction;
    public CanvasGroup textTime;

    public bool interfaceOpen = false; 

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player != null)
        {
            movimentControler  = player.GetComponent<PlayerController>();
        }
       
    }

    public void Reset()
    {
     
        CanvasGroup resetCanvasGroup = playerReset.GetComponent<CanvasGroup>();

        Sequence mySequence = DOTween.Sequence();

        float screenWidth = Screen.width;
        float startPositionX = screenWidth + bgTransaction.sizeDelta.x / 2;
        float endPositionX = -startPositionX;

        bgTransaction.anchoredPosition = new Vector2(startPositionX, bgTransaction.anchoredPosition.y);



        if (resetCanvasGroup != null)
        {
            mySequence.Append(resetCanvasGroup.DOFade(0, 0.75f));
            mySequence.Join(imageToAnimatePlayer.transform.DOScale(0.4f, 0.75f));
            mySequence.Join(imageToAnimatePlayer.DOFade(0, 0.75f));
            mySequence.Join(textTime.DOFade(0, 0.5f));

            mySequence.Append(bgTransaction.DOAnchorPosX(0, 1f).SetEase(Ease.OutQuad));

            mySequence.AppendCallback(() =>
            {
                movimentControler.ResetScene();
                Sequence mySequenceTwo = DOTween.Sequence();
                mySequenceTwo.Append(bgTransaction.DOAnchorPosX(endPositionX, 1f).SetEase(Ease.InQuad));
               

                mySequenceTwo.AppendCallback(() =>
                {
                    playerReset.SetActive(false);

                });
        
            });

        }
    }

}
