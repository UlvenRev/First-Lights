using UnityEngine;
using UnityEngine.EventSystems;

public class HandleWindowClick : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Sprite windowOffSprite;
    [SerializeField] private Sprite windowOnSprite;
    private bool isOn = false;

    [SerializeField] private AudioClip offSound;
    [SerializeField] private AudioClip onSound;
    private float volume = 1f;

    public void OnPointerDown(PointerEventData eventData)
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
    }
}
