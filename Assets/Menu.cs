using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Menu : MonoBehaviour
{
    public Image vibrationImage;
    public Image audioImage;
    public Button menuToggle;
    public VibrationButton vibrationToggle;
    public MusicButton musicToggle;
    private bool showingMenu;

    public void ToggleMenu()
    {
        if (!showingMenu) ShowMenu();
        else HideMenu();
    }

    public void ShowMenu()
    {
        showingMenu = true;
        menuToggle.interactable = false;
        menuToggle.transform.DORotate(new Vector3(0, 0, 180), UIManager.UI_TRANSITION_TIME / 2).OnComplete(() =>
        {
            menuToggle.interactable = true;
        });

        vibrationToggle.Show();
        musicToggle.Show();
    }

    public void HideMenu()
    {
        showingMenu = false;
        menuToggle.interactable = false;
        menuToggle.transform.DORotate(Vector3.zero, UIManager.UI_TRANSITION_TIME / 2).OnComplete(() =>
        {
            menuToggle.interactable = true;
        });

        vibrationToggle.Hide();
        musicToggle.Hide();
    }
}
