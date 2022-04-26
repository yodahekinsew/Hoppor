using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum GameState
{
    Start,
    Play,
    End
}

public class GameStateManager : MonoBehaviour
{
    public static GameState state = GameState.Start;
    public static bool usedSecondChance = false;
    public UIManager ui;
    public LevelSpawner levels;
    public new AudioManager audio;
    public ScoreCounter score;
    public StreakCounter streak;

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void StartGame()
    {
        state = GameState.Play;
        ui.StartGame();
        audio.StartGameLoop();
        levels.StartGame();
        score.StartGame();
        streak.StartGame();
    }

    public void EndGame()
    {
        state = GameState.End;
        ui.EndGame();
        audio.StopGameLoop();
        levels.EndGame();
        score.EndGame();
        streak.EndGame();
    }

    public void SecondChance()
    {
        usedSecondChance = true;
        state = GameState.Play;
        ui.SecondChance();
        audio.StartGameLoop();
        levels.SecondChance();
        score.SecondChance();
        streak.SecondChance();
    }

    public void PlayAgain()
    {
        usedSecondChance = false;
        state = GameState.Play;
        ui.PlayAgain();
        audio.StartGameLoop();
        levels.PlayAgain();
        score.PlayAgain();
        streak.PlayAgain();
    }

    public void StopPlaying()
    {
        usedSecondChance = false;
        state = GameState.Start;
        ui.StopPlaying();
        levels.StopPlaying();
        score.StopPlaying();
        streak.StopPlaying();
    }
}
