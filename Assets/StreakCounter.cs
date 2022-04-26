using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class StreakCounter : MonoBehaviour
{
    public static int streak = 1;

    [Header("Timer")]
    public float timeToComplete;
    private float levelStartTime;

    [Header("Bar Fill")]
    public Transform fill;
    public float maxScale; // Scale of fill bar when at max fill

    [Header("Streak")]
    public TextMeshPro text;

    private bool countingDown = false;

    void Update()
    {
        if (!countingDown) return;

        if (Time.time - levelStartTime > timeToComplete)
        {
            streak = 1;
            text.text = "x1";
        }

        float fillAmount = (timeToComplete - Mathf.Min(timeToComplete, Time.time - levelStartTime)) / timeToComplete * maxScale;
        fill.localScale = new Vector3(fillAmount, 1, 1);
    }

    public void NextLevel()
    {
        if (!countingDown)
        {
            countingDown = true;
            streak++;
            text.text = "x" + streak;
        }
        else if (Time.time - levelStartTime <= timeToComplete)
        {
            streak++;
            text.text = "x" + streak;
        }
        levelStartTime = Time.time;
        fill.localScale = new Vector3(maxScale, 1, 1);
    }

    public void StartGame()
    {
        transform.DOMoveY(10, UIManager.UI_TRANSITION_TIME);
    }

    public void EndGame()
    {
        countingDown = false;
        fill.localScale = new Vector3(maxScale, 1, 1);
        streak = 1;
        text.text = "x1";

        transform.DOMoveY(15, UIManager.UI_TRANSITION_TIME);
    }

    public void PlayAgain()
    {
        transform.DOMoveY(10, UIManager.UI_TRANSITION_TIME);
    }

    public void SecondChance()
    {
        transform.DOMoveY(10, UIManager.UI_TRANSITION_TIME);
    }

    public void StopPlaying()
    {
    }
}
