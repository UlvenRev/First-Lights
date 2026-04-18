using UnityEngine;
using System.Linq;
using System;
using System.Collections;

public class CreateSequenceScript : MonoBehaviour
{
    [SerializeField] private int windowsNumToClick;
    private int[] windowsForSequence; 
	private int totalWindowsNumber;
    
    public bool canClick;  // For Check Sequence
    
    // ----------- Sprites -----------
    [SerializeField] private Sprite windowOffSprite;
    [SerializeField] private Sprite windowOnSprite;
    
    // ----------- Sound management -----------
    [SerializeField] private AudioClip offSound;
    [SerializeField] private AudioClip onSound;
    private float volume = 1f;

    // ----------- Scripts  -----------
    private CreateWindowsScript createWindowsScript;
    private CheckSequenceScript checkSequenceScript;

    void Awake()  // On Awake so that it creates windowsForSequence faster than Check Sequence Script requests it
    {
        createWindowsScript = GetComponent<CreateWindowsScript>();
        checkSequenceScript = GetComponent<CheckSequenceScript>();
        
		totalWindowsNumber = createWindowsScript.windowsAmount;
    }

    public void StartNewRound()
    {
        canClick = false;
        
        windowsForSequence = new int[windowsNumToClick];
        for (int i = 0; i < windowsForSequence.Length; i++) {
            windowsForSequence[i] = -1;  // Initializing everything to -1 just for the while loop below not to freeze
        }
        
        CreateRandomSequence();
    }

    public void CreateRandomSequence()
    {
        for (int i = 0; i < windowsNumToClick; i++)
        {
            int randomWindow = UnityEngine.Random.Range(0, totalWindowsNumber);
            while (windowsForSequence.Contains(randomWindow)) randomWindow = UnityEngine.Random.Range(0, totalWindowsNumber);  // To avoid repetitios
            windowsForSequence[i] = randomWindow;
        }

        Array.Sort(windowsForSequence);  // Sort in ascending order
        for (int i = 0; i < windowsNumToClick; i++)
        {
            Debug.Log(windowsForSequence[i]);
        }

        checkSequenceScript.GetWindowSequence(windowsForSequence);

        StartCoroutine(ShowWindowsSequence());
    }

    IEnumerator ShowWindowsSequence()
    {
        var childIndex = 0;
        var currentSequenceWindowIndex = 0;
        
        // Turn on all windows from the sequence:
        SoundFXManagerScript.instance.PlaySoundFXClip(onSound, transform, volume);
        foreach (Transform child in transform)
        {
            SpriteRenderer spriteRend = child.GetComponent<SpriteRenderer>();  // To be able to change the sprite 

            if (currentSequenceWindowIndex < windowsForSequence.Length)
            {
                if (childIndex == windowsForSequence[currentSequenceWindowIndex])
                {
                    spriteRend.sprite = windowOnSprite;
                    currentSequenceWindowIndex++;
                }
            }

            childIndex++;
        }
        
        // Wait for 2 seconds:
        yield return new WaitForSeconds(2f);
        
        // Reset all the windows back to default:
        SoundFXManagerScript.instance.PlaySoundFXClip(offSound, transform, volume);
        foreach (Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().sprite = windowOffSprite;
        }

        canClick = true;
    }
}
