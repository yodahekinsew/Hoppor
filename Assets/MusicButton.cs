using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MusicButton : MonoBehaviour
{
    public Image buttonImage;
    public Image musicImage;
    public Image crossImage;
    public AudioManager audioManager;

    // public AudioManager audio;
    public bool toggled = false;

    void Start()
    {
        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetInt("Music", 0);
        }
        else toggled = PlayerPrefs.GetInt("Music") == 0 ? false : true;
        
        if (toggled) audioManager.ToggleMute();

        crossImage.enabled = toggled;
    }

    public void Toggle()
    {
        toggled = !toggled;
        audioManager.ToggleMute();
        
        PlayerPrefs.SetInt("Music", toggled ? 1 : 0);

        crossImage.enabled = toggled;
    }

    public void Show()
    {
        Color color = musicImage.color;
        color.a = 1;
        musicImage.gameObject.SetActive(true);
        buttonImage.DOColor(color, UIManager.UI_TRANSITION_TIME / 2);
        crossImage.DOColor(color, UIManager.UI_TRANSITION_TIME / 2);
        musicImage.DOColor(color, UIManager.UI_TRANSITION_TIME / 2);
    }

    public void Hide()
    {
        Color color = musicImage.color;
        color.a = 0;
        buttonImage.DOColor(color, UIManager.UI_TRANSITION_TIME / 2);
        crossImage.DOColor(color, UIManager.UI_TRANSITION_TIME / 2);
        musicImage.DOColor(color, UIManager.UI_TRANSITION_TIME / 2).OnComplete(() =>
          {
              musicImage.gameObject.SetActive(false);
          });
    }
}
