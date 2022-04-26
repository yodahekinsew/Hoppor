using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ScoreCounter : MonoBehaviour
{
    public static int score;
    public static int highscore;

    public Leaderboard leaderboard;
    public Button leaderboardButton;
    public Image leaderboardImage;
    public TextMeshPro text;
    public BounceCounter bounces;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Highscore"))
        {
            highscore = PlayerPrefs.GetInt("Highscore");
        }
        else
        {
            highscore = 0;
            PlayerPrefs.SetInt("Highscore", highscore);
        }

        text.text = "" + highscore;

        Color color = text.color;
        color.a = .5f;
        text.DOColor(color, UIManager.UI_TRANSITION_TIME);
    }

    public void EndGame()
    {
        Color color = text.color;
        color.a = 1;
        text.DOColor(color, UIManager.UI_TRANSITION_TIME);
        text.transform.DOMoveY(6.5f, UIManager.UI_TRANSITION_TIME);
        leaderboard.UpdateLeaderboard(highscore);
    }

    public void StartGame()
    {
        HideLeaderboardButton();
        if (text.text != "0")
        {
            text.DOScale(.5f, .25f * UIManager.UI_TRANSITION_TIME).OnComplete(() =>
              {
                  text.text = "0";
                  text.DOScale(1, .25f * UIManager.UI_TRANSITION_TIME);
              });
        }
    }

    public void PlayAgain()
    {
        text.text = "0";
        score = 0;
        Color color = text.color;
        color.a = .5f;
        text.DOColor(color, UIManager.UI_TRANSITION_TIME);
        text.transform.DOMoveY(0, UIManager.UI_TRANSITION_TIME);
    }

    public void SecondChance()
    {
        Color color = text.color;
        color.a = .5f;
        text.DOColor(color, UIManager.UI_TRANSITION_TIME);
        text.transform.DOMoveY(0, UIManager.UI_TRANSITION_TIME);
    }

    public void StopPlaying()
    {
        ShowLeaderboardButton();
        text.text = "" + highscore;
        score = 0;
        Color color = text.color;
        color.a = .5f;
        text.DOColor(color, UIManager.UI_TRANSITION_TIME);
        text.transform.DOMoveY(0, UIManager.UI_TRANSITION_TIME);
    }

    public void IncrementScore(int amount)
    {
        print("The number of bounces is " + BounceCounter.numBounces);
        score += (amount + BounceCounter.numBounces) * StreakCounter.streak;
        if (BounceCounter.numBounces > 0) bounces.ShowBounces();

        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("Highscore", highscore);
        }

        text.DOScale(.5f, .25f * UIManager.UI_TRANSITION_TIME).OnComplete(() =>
          {
              text.text = "" + score;
              text.DOScale(1, .25f * UIManager.UI_TRANSITION_TIME);
          });
    }

    public void ShowLeaderboardButton()
    {
        leaderboardButton.enabled = true;
        leaderboardImage.DOColor(new Color(1, 1, 1, 1), .5f);
    }

    public void HideLeaderboardButton()
    {
        leaderboardButton.enabled = false;
        leaderboardImage.DOColor(new Color(1, 1, 1, 0), .5f);
    }
}
