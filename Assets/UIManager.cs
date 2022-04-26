using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static float UI_TRANSITION_TIME = 1;

    public StartScreen startScreen;
    public EndScreen endScreen;

    private void Start()
    {
        startScreen.Show();
    }

    public void StartGame()
    {
        startScreen.Hide();
    }

    public void EndGame()
    {
        endScreen.Show();
    }

    public void PlayAgain()
    {
        endScreen.Hide();
    }

    public void SecondChance()
    {
        endScreen.Hide();
    }

    public void StopPlaying()
    {
        startScreen.Show();
        endScreen.Hide();
    }
}
