using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StartScreen : MonoBehaviour
{
    public Image hopporHeader;
    public TextMeshProUGUI tapAnywhereText;
    public Button playGame;

    public void Hide()
    {
        playGame.interactable = false;

        Color fadedText = tapAnywhereText.color;
        fadedText.a = 0;
        tapAnywhereText.DOColor(fadedText, UIManager.UI_TRANSITION_TIME);

        Color fadedHeader = hopporHeader.color;
        fadedHeader.a = 0;
        hopporHeader.DOColor(fadedHeader, UIManager.UI_TRANSITION_TIME);
    }

    public void Show()
    {
        Color text = tapAnywhereText.color;
        text.a = 1;
        tapAnywhereText.DOColor(text, UIManager.UI_TRANSITION_TIME);

        Color header = hopporHeader.color;
        header.a = 1;
        hopporHeader.DOColor(header, UIManager.UI_TRANSITION_TIME).OnComplete(() =>
        {
            playGame.interactable = true;
        });
    }
}
