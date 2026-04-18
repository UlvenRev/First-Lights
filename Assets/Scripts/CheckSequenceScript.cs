using UnityEngine;
using System.Linq;

public class CheckSequenceScript : MonoBehaviour
{
    // ----------- Sprites -----------
    [SerializeField] private Sprite windowOffSprite;

    // ----------- Create Sequence Script public stuff -----------
    private CreateSequenceScript createSequenceScript;
    private int[] windowsToCheck;
    
    // ----------- Sound management -----------
    [SerializeField] private AudioClip offSound;
    private float volume = 1f;

    void Start()
    {
        createSequenceScript = GetComponent<CreateSequenceScript>();
    }

    public void GetWindowSequence(int[] windowsForSequence)
    {
        windowsToCheck = windowsForSequence;  // Request the sequence of random windows to check
    }
    
    public void CheckSequence()
    {
        int correctCount = 0;
        int wrongCount = 0;
    
        HandleWindowClick[] allWindows = GetComponentsInChildren<HandleWindowClick>();

        foreach (var window in allWindows)
        {
            int windowIndex = System.Array.IndexOf(allWindows, window);

            if (window.isOn)
            {
                if (windowsToCheck.Contains(windowIndex)) correctCount++;
                else wrongCount++;
            }

            if (correctCount == windowsToCheck.Length || wrongCount == windowsToCheck.Length) break;
        }

        if (correctCount == windowsToCheck.Length && wrongCount == 0)
        {
            Debug.Log("--------------- SEQUENCE COMPLETED!");
            Debug.Log("Creating a new sequence. Look at the screen and remember...");
        
            ResetWindows();
            createSequenceScript.StartNewRound();
        } else if (wrongCount + correctCount >= windowsToCheck.Length)
        {
            Debug.Log("SEQUENCE FAILED");
            ResetWindows();
        }  
    }

    private void ResetWindows()  // Set each window back to turned off and isOn variable to false
    {
        SoundFXManagerScript.instance.PlaySoundFXClip(offSound, transform, volume);
        foreach (Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().sprite = windowOffSprite;
            child.GetComponent<HandleWindowClick>().isOn = false;
        }
    }
}
