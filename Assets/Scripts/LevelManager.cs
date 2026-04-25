using JetBrains.Annotations;
using Unity.Cinemachine;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private CreateSequenceScript[] buildings;
    [SerializeField] private CinemachineCamera[] cameras;

    private int currentLevelIndex;  // Starting with the first building

    private int levelsPassed;
    private int numberOfLevels = 3;

    void Start()
    {
        StartLevel(0);
    }

    public void StartLevel(int level)
    {
        foreach (var cam in cameras) cam.Priority = 10;
        cameras[level].Priority = 20;  // Making the current level camera with the highest priority so it displays instead of all others
        
        buildings[level].StartNewRound();  // Tell the current level's building to start
    }

    public void OnLevelComplete()
    {
        levelsPassed++;
        if (levelsPassed < numberOfLevels)
        {
            StartLevel(currentLevelIndex);
        }
        else
        {
            currentLevelIndex++;
            levelsPassed = 0;
            if (currentLevelIndex < buildings.Length)
            {
                StartLevel(currentLevelIndex);
            }
            else
            {
                Debug.Log("YOU WIN THE WHOLE GAME!");  
            }
        }
    }
}
