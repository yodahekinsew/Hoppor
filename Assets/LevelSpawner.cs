using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class LevelSpawner : MonoBehaviour
{
    public static float LEVEL_TRANSITION_TIME = 1;
    public static bool transitioning = false;

    public bool testingLevels = false;

    public ScoreCounter score;
    public StreakCounter streak;

    public Transform spikeBarrier;
    public TextMeshPro instructions;

    public Level currentLevel; // The Level you're currently playing on
    public Level lastLevel; // The last level you played on (will showed up faded out on screen)
    [Header("Levels")]
    public List<string> difficulties;
    private Dictionary<string, List<Object>> leftLevels = new Dictionary<string, List<Object>>();
    private Dictionary<string, List<Object>> centerLevels = new Dictionary<string, List<Object>>();
    private Dictionary<string, List<Object>> rightLevels = new Dictionary<string, List<Object>>();
    private List<Object> testLevels;
    private List<Object> recentLevels;
    public GameObject startLevel;
    private float levelInitialPosition; // The initial position of a spawned level
    private float levelStartPosition = -6f; // The start position for a level [the start block should be rooted here]
    private float nextLevelPosition = -2f;

    [Header("Difficulty Score Caps")]
    public List<int> difficultyScoreCaps;

    private void Start()
    {
        foreach (string difficulty in difficulties)
        {
            leftLevels.Add(difficulty, new List<Object>(Resources.LoadAll("Levels/Left Levels/" + difficulty, typeof(GameObject))));
            centerLevels.Add(difficulty, new List<Object>(Resources.LoadAll("Levels/Center Levels/" + difficulty, typeof(GameObject))));
            rightLevels.Add(difficulty, new List<Object>(Resources.LoadAll("Levels/Right Levels/" + difficulty, typeof(GameObject))));
        }
        testLevels = new List<Object>(Resources.LoadAll("Levels/Test Levels", typeof(GameObject)));
        recentLevels = new List<Object>();

        levelInitialPosition = Camera.main.orthographicSize + 1;
        StartScreen();
    }

    public void StartScreen()
    {
        currentLevel = Instantiate(startLevel,
            new Vector3(0, -levelInitialPosition - 3, transform.position.z),
            Quaternion.identity).GetComponent<Level>();
        currentLevel.transform.DOMoveY(levelStartPosition, LEVEL_TRANSITION_TIME);
    }

    public void NextLevel()
    {
        streak.NextLevel();

        transitioning = true;
        score.IncrementScore(1);

        Level nextLevel = SpawnNewLevel();
        nextLevel.transform.DOMoveY(nextLevelPosition, LEVEL_TRANSITION_TIME);

        float moveAmount = currentLevel.GetExitPosition().y - levelStartPosition;
        currentLevel.transform.DOMoveY(currentLevel.transform.position.y - moveAmount, LEVEL_TRANSITION_TIME);
        currentLevel.SemiDeactivate();

        Level previousLevel = lastLevel;
        lastLevel.transform.DOMoveY(lastLevel.transform.position.y - moveAmount, LEVEL_TRANSITION_TIME).OnComplete(() =>
        {
            Destroy(previousLevel.gameObject);
            transitioning = false;
        });
        lastLevel.Deactivate();

        lastLevel = currentLevel;
        currentLevel = nextLevel;
    }

    public void StartGame()
    {
        lastLevel = currentLevel;
        currentLevel = SpawnNewLevel();
        currentLevel.transform.DOMoveY(nextLevelPosition, LEVEL_TRANSITION_TIME);
        lastLevel.transform.DOMoveY(levelStartPosition, LEVEL_TRANSITION_TIME);

        instructions.DOColor(new Color(1, 1, 1, 1), 1f);

        spikeBarrier.DOMoveY(-11, LEVEL_TRANSITION_TIME);
    }

    public void GoBackLevel()
    {
        lastLevel.Reactivate();
        lastLevel.transform.DOMoveY(lastLevel.transform.position.y + 5, LEVEL_TRANSITION_TIME);

        currentLevel.transform.DOMoveY(currentLevel.transform.position.y + 5, LEVEL_TRANSITION_TIME);
    }

    public Level SpawnNewLevel()
    {
        List<Object> availableLevels = new List<Object>();
        if (testingLevels) availableLevels.AddRange(testLevels);
        else
        {
            int index = 0;
            foreach (string difficulty in difficulties)
            {
                if (ScoreCounter.score < difficultyScoreCaps[index]) break;
                if (currentLevel.useLeftLevels) availableLevels.AddRange(leftLevels[difficulty]);
                if (currentLevel.useCenterLevels) availableLevels.AddRange(centerLevels[difficulty]);
                if (currentLevel.useRightLevels) availableLevels.AddRange(rightLevels[difficulty]);
                index++;
            }
            availableLevels.RemoveAll(l => recentLevels.Contains(l));
        }
        Object chosenLevel = availableLevels[Random.Range(0, availableLevels.Count)];
        if (!testingLevels)
        {
            recentLevels.Insert(0, chosenLevel);
            if (recentLevels.Count > 10) recentLevels.RemoveAt(10);
        }
        return Instantiate(
            (GameObject)chosenLevel,
            new Vector3(0, levelInitialPosition, transform.position.z),
            Quaternion.identity).GetComponent<Level>();
    }

    public void EndGame()
    {
        float distance = levelInitialPosition - lastLevel.transform.position.y;
        currentLevel.transform.DOMoveY(currentLevel.transform.position.y + distance, LEVEL_TRANSITION_TIME);
        lastLevel.transform.DOMoveY(lastLevel.transform.position.y + distance, LEVEL_TRANSITION_TIME);

        spikeBarrier.DOMoveY(-20, LEVEL_TRANSITION_TIME);
    }

    public void PlayAgain()
    {
        Destroy(currentLevel.gameObject);
        Destroy(lastLevel.gameObject);
        currentLevel = Instantiate(startLevel,
            new Vector3(0, -levelInitialPosition - 3, transform.position.z),
            Quaternion.identity).GetComponent<Level>();

        lastLevel = currentLevel;
        currentLevel = SpawnNewLevel();
        currentLevel.transform.DOMoveY(nextLevelPosition, LEVEL_TRANSITION_TIME);
        lastLevel.transform.DOMoveY(levelStartPosition, LEVEL_TRANSITION_TIME);

        spikeBarrier.DOMoveY(-11, LEVEL_TRANSITION_TIME);
    }

    public void SecondChance()
    {
        // currentLevel.transform.DOMoveY(currentLevel.transform.position.y - 25, LEVEL_TRANSITION_TIME);
        // lastLevel.transform.DOMoveY(lastLevel.transform.position.y - 25, LEVEL_TRANSITION_TIME);
        PlayAgain();
    }

    public void StopPlaying()
    {
        Destroy(currentLevel.gameObject);
        Destroy(lastLevel.gameObject);
        spikeBarrier.DOMoveY(-20, LEVEL_TRANSITION_TIME);
        StartScreen();
    }
}
