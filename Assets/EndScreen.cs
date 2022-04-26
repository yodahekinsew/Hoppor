using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using TMPro;
using DG.Tweening;

public class EndScreen : MonoBehaviour
{
    public RectTransform rect;

    [Header("High Score Counter")]
    public TextMeshProUGUI highscoreText;

    [Header("End Buttons")]
    public Button playAgainButton;
    public Image playAgainImage;
    public TextMeshProUGUI playAgainText;
    public Button secondChanceButton;
    public Image secondChanceImage;
    public TextMeshProUGUI secondChanceText;
    public Button stopPlayingButton;
    public Image stopPlayingImage;
    public TextMeshProUGUI stopPlayingText;

    private void Start()
    {
        rect.offsetMax = new Vector2(0, -Screen.height);
        rect.offsetMin = new Vector2(0, -Screen.height);
    }

    public void Show()
    {
        rect.offsetMax = Vector2.zero;
        rect.offsetMin = Vector2.zero;

        highscoreText.gameObject.SetActive(true);
        highscoreText.text = "High Score\t<size=125%>" + ScoreCounter.highscore;

        playAgainImage.enabled = true;
        secondChanceImage.enabled = true;
        stopPlayingImage.enabled = true;

        Color imageColor = playAgainImage.color;
        imageColor.a = 1;
        playAgainImage.DOColor(imageColor, UIManager.UI_TRANSITION_TIME).OnComplete(() => { playAgainButton.interactable = true; });
        secondChanceImage.DOColor(imageColor, UIManager.UI_TRANSITION_TIME).OnComplete(() => { secondChanceButton.interactable = !GameStateManager.usedSecondChance && Advertisement.IsReady("rewardedVideo"); });
        stopPlayingImage.DOColor(imageColor, UIManager.UI_TRANSITION_TIME).OnComplete(() => { stopPlayingButton.interactable = true; });
        highscoreText.DOColor(imageColor, UIManager.UI_TRANSITION_TIME);

        playAgainText.enabled = true;
        secondChanceText.enabled = true;
        stopPlayingText.enabled = true;

        Color textColor = playAgainText.color;
        textColor.a = 1;
        playAgainText.DOColor(textColor, UIManager.UI_TRANSITION_TIME);
        stopPlayingText.DOColor(textColor, UIManager.UI_TRANSITION_TIME);
        if (GameStateManager.usedSecondChance) textColor.a = .5f;
        secondChanceText.DOColor(textColor, UIManager.UI_TRANSITION_TIME);
    }

    public void Hide()
    {
        playAgainButton.interactable = false;
        secondChanceButton.interactable = false;
        stopPlayingButton.interactable = false;

        Color imageColor = playAgainImage.color;
        imageColor.a = 0;
        playAgainImage.DOColor(imageColor, UIManager.UI_TRANSITION_TIME).OnComplete(() => { playAgainImage.enabled = false; });
        secondChanceImage.DOColor(imageColor, UIManager.UI_TRANSITION_TIME).OnComplete(() => { secondChanceImage.enabled = false; });
        stopPlayingImage.DOColor(imageColor, UIManager.UI_TRANSITION_TIME).OnComplete(() => { stopPlayingImage.enabled = false; });
        highscoreText.DOColor(imageColor, UIManager.UI_TRANSITION_TIME).OnComplete(() =>
        {
            highscoreText.gameObject.SetActive(false);
            rect.offsetMax = new Vector2(0, -Screen.height);
            rect.offsetMin = new Vector2(0, -Screen.height);
        });

        Color textColor = playAgainText.color;
        textColor.a = 0;
        playAgainText.DOColor(textColor, UIManager.UI_TRANSITION_TIME).OnComplete(() => { playAgainText.enabled = false; });
        secondChanceText.DOColor(textColor, UIManager.UI_TRANSITION_TIME).OnComplete(() => { secondChanceText.enabled = false; });
        stopPlayingText.DOColor(textColor, UIManager.UI_TRANSITION_TIME).OnComplete(() => { stopPlayingText.enabled = false; });
    }
}
