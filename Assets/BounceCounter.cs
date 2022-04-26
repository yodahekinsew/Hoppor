using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BounceCounter : MonoBehaviour
{
    public static int numBounces = 0;
    public TextMeshPro bounceText;

    public void ShowBounces()
    {
        bounceText.text = "+" + numBounces + " Bounces";
        Color color = bounceText.color;
        color.a = .5f;
        bounceText.DOColor(color, UIManager.UI_TRANSITION_TIME / 2).OnComplete(() =>
          {
              color.a = 0;
              bounceText.DOColor(color, UIManager.UI_TRANSITION_TIME);
          });
    }
}
