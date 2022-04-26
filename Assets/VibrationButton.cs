using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class VibrationButton : MonoBehaviour
{
    public Image buttonImage;
    public Image vibrationImage;
    public Image crossImage;
    public bool toggled = false;

    void Start()
    {
        if (!PlayerPrefs.HasKey("Vibration"))
        {
            PlayerPrefs.SetInt("Vibration", 0);
        }
        else toggled = PlayerPrefs.GetInt("Vibration") == 0 ? false : true;

        crossImage.enabled = toggled;
        if (toggled) Vibration.Disable();
        else Vibration.Enable();
    }

    public void Toggle()
    {
        toggled = !toggled;
        PlayerPrefs.SetInt("Vibration", toggled ? 1 : 0);

        crossImage.enabled = toggled;
        if (toggled) Vibration.Disable();
        else Vibration.Enable();
    }

    public void Show()
    {
        Color color = vibrationImage.color;
        color.a = 1;
        vibrationImage.gameObject.SetActive(true);
        buttonImage.DOColor(color, UIManager.UI_TRANSITION_TIME / 2);
        crossImage.DOColor(color, UIManager.UI_TRANSITION_TIME / 2);
        vibrationImage.DOColor(color, UIManager.UI_TRANSITION_TIME / 2);
    }

    public void Hide()
    {
        Color color = vibrationImage.color;
        color.a = 0;
        buttonImage.DOColor(color, UIManager.UI_TRANSITION_TIME / 2);
        crossImage.DOColor(color, UIManager.UI_TRANSITION_TIME / 2);
        vibrationImage.DOColor(color, UIManager.UI_TRANSITION_TIME / 2).OnComplete(() =>
          {
              vibrationImage.gameObject.SetActive(false);
          });
    }
}
