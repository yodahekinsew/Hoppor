using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum LevelPosition
{
    Left,
    Center,
    Right
}

public class Level : MonoBehaviour
{
    public LevelSpawner spawner;

    [Header("Available Follow Levels")]
    public bool useLeftLevels;
    public bool useCenterLevels;
    public bool useRightLevels;

    public Platform exitPlatform;

    private float screenHeight;

    private void Start()
    {
        spawner = GameObject.Find("/Level Spawner").GetComponent<LevelSpawner>();
        screenHeight = Camera.main.orthographicSize;
    }

    public void Exit()
    {
        spawner.NextLevel();
    }

    public Vector3 GetExitPosition()
    {
        return exitPlatform.GetPosition();
    }

    public void Deactivate()
    {
        foreach (Platform platform in GetComponentsInChildren<Platform>())
        {
            platform.Deactivate();
        }
    }

    public void SemiDeactivate()
    {
        foreach (Platform platform in GetComponentsInChildren<Platform>())
        {
            if (platform.IsExit()) continue;
            platform.SemiDeactivate();
        }
    }

    public void Reactivate()
    {
        foreach (Platform platform in GetComponentsInChildren<Platform>())
        {
            if (platform.IsExit()) continue;
            platform.Reactivate();
        }
    }
}
