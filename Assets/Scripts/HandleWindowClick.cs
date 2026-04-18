using UnityEngine;
using UnityEngine.EventSystems;

public class HandleWindowClick : MonoBehaviour, IPointerDownHandler
{
    // ----------- Sprites -----------
    [SerializeField] private Sprite windowOffSprite;
    [SerializeField] private Sprite windowOnSprite;
    public bool isOn;

    // ----------- Sound management -----------
    [SerializeField] private AudioClip offSound;
    [SerializeField] private AudioClip onSound;
    private float volume = 1f;
    
    // ----------- Parent scripts -----------
    private CheckSequenceScript checkSequenceScript;
    private CreateSequenceScript createSequenceScript;

    void Start()
    {
        var parentName = transform.parent.name;
        checkSequenceScript = GameObject.Find(parentName).GetComponent<CheckSequenceScript>();
        createSequenceScript = GameObject.Find(parentName).GetComponent<CreateSequenceScript>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (createSequenceScript.canClick)
        {
            if (!isOn)
            {
                GetComponent<SpriteRenderer>().sprite = windowOnSprite;
                SoundFXManagerScript.instance.PlaySoundFXClip(onSound, transform, volume);
                isOn = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = windowOffSprite;
                SoundFXManagerScript.instance.PlaySoundFXClip(offSound, transform, volume);
                isOn = false;
            }
        
            checkSequenceScript.CheckSequence();  // This only calls the function of a PARENT who will be checking the entire logic   
        }
    }
}
