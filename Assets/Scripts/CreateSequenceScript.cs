using UnityEngine;
using System.Linq;
using System;
using System.Collections;

public class CreateSequenceScript : MonoBehaviour
{
    [SerializeField] private int windowsNumToClick;
    public int[] windowsForSequence; 
    
    [SerializeField] private Sprite windowOffSprite;
    [SerializeField] private Sprite windowOnSprite;
    
    [SerializeField] private AudioClip offSound;
    [SerializeField] private AudioClip onSound;
    private float volume = 1f;

    private CreateWindowsScript createWindowsScript;
    private bool showedWindowsForSequence = false;
    private bool finishedSpawningWindows = false;

    void Start()
    {
        createWindowsScript = GetComponent<CreateWindowsScript>();
        
        windowsForSequence = new int[windowsNumToClick];
        for (int i = 0; i < windowsForSequence.Length; i++) {
            windowsForSequence[i] = -1;  // Initializing everything to -1 just for the while loop below not to freeze
        }
        
        CreateRandomSequence();
    }

    void Update()
    {
        finishedSpawningWindows = createWindowsScript.finishedSpawningWindows;
        
        if (!showedWindowsForSequence && finishedSpawningWindows)
        {
            StartCoroutine(ShowWindowsSequence());
            showedWindowsForSequence = true;  // We need to run coroutine only once, so making sure we won't be running it every update
        }
    }

    public void CreateRandomSequence()
    {
        for (int i = 0; i < windowsNumToClick; i++)
        {
            int randomWindow = UnityEngine.Random.Range(0, 64);
            while (windowsForSequence.Contains(randomWindow)) randomWindow = UnityEngine.Random.Range(0, 64);  // To avoid repetitios
            windowsForSequence[i] = randomWindow;
        }

        Array.Sort(windowsForSequence);  // Sort in ascending order
        for (int i = 0; i < windowsNumToClick; i++)
        {
            Debug.Log(windowsForSequence[i]);
        }
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
        
    }
}
