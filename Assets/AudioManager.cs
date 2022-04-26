using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    public float volume = .5f;
    public float transitionDuration = .5f;
    public float gameStartFadeDuration = 2f;
    public AudioSource gameLoop;
    public AudioSource startingLoop;
    public AudioSource soundFX;

    private bool muted = false;
    private float gameVolume = 0f;
    private float startingVolume = 1;
    private float fxVolume = 1;

    private void Awake()
    {
        gameLoop.volume = 0;
        startingLoop.volume = 0;
        fxVolume = volume * fxVolume;
    }

    private void Start()
    {
        startingLoop.Play();
        startingLoop.DOFade(volume * startingVolume, gameStartFadeDuration);
    }

    public void StartGameLoop()
    {
        gameVolume = 1;
        startingVolume = 0;
        gameLoop.Play();
        gameLoop.time = startingLoop.time % 12;
        gameLoop.DOFade(volume * gameVolume, transitionDuration);
        startingLoop.DOFade(volume * startingVolume, transitionDuration).OnComplete(startingLoop.Stop);
    }

    public void StopGameLoop()
    {
        gameVolume = 0;
        startingVolume = 1;
        startingLoop.Play();
        startingLoop.time = gameLoop.time % 12;
        startingLoop.DOFade(volume * startingVolume, transitionDuration);
        gameLoop.DOFade(volume * gameVolume, transitionDuration / 2).OnComplete(gameLoop.Stop);
        gameLoop.DOPitch(0.5f, transitionDuration / 2).OnComplete(() =>
                 {
                     gameLoop.pitch = 1;
                 });
    }

    public void ToggleMute()
    {
        if (!muted)
        {
            muted = true;
            gameLoop.volume = 0;
            startingLoop.volume = 0;
            soundFX.volume = 0;
        }
        else
        {
            muted = false;
            gameLoop.volume = gameVolume * volume;
            startingLoop.volume = startingVolume * volume;
            soundFX.volume = fxVolume * volume;
        }
    }
}
