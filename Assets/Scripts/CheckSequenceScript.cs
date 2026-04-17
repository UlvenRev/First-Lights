using UnityEngine;

public class CheckSequenceScript : MonoBehaviour
{
    [SerializeField] private Sprite windowOffSprite;
    [SerializeField] private Sprite windowOnSprite;

    private CreateSequenceScript createSequenceScript;
    private int[] windowsToLight;
    
    [SerializeField] private AudioClip offSound;
    private float volume = 1f;

    void Start()
    {
        createSequenceScript = GetComponent<CreateSequenceScript>();
        windowsToLight = createSequenceScript.windowsForSequence;
    }
    
    public void CheckSequence()
    {
        bool completed = true;
        int childIndex = 0;
        int targetWindow = 0;  // For windowsToLight array

        int windowsOn = 0;
        
        foreach (Transform child in transform)
        {
            var sprite = child.GetComponent<SpriteRenderer>().sprite;
            
            if (targetWindow < windowsToLight.Length && childIndex == windowsToLight[targetWindow])
            {
                // Move on to the next window index in the windowsToLight array
                if (sprite == windowOnSprite)
                {
                    targetWindow++;
                    windowsOn++;
                }
                else  completed = false;  // For this SINGLE FRAME on which checkSequence was called, the sequence is NOT completed
            }
            else
            {
                if (sprite == windowOnSprite)  // One random window that is NOT supposed to be in the sequence is turned on
                {
                    // Therefore for this frame the sequence is not correct - can exit the loop
                    completed = false;
                    windowsOn++;
                }
            }
            
            childIndex++;
        }

        if (completed)
        {
            Debug.Log("SEQUENCE COMPLETED!");
        } 
        else if (windowsOn >= windowsToLight.Length)
        {
            Debug.Log("Sequence failed!!!!");
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
