using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool isOpen = false;
    public float openAngle = 90f;
    public float closeAngle = 0f;
    public float smooth = 2f;
    public AudioClip openSound; 
    public AudioClip closeSound; 
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
        PlayDoorSound(openSound);
    }

    public void CloseDoor()
    {
        isOpen = false;
        PlayDoorSound(closeSound);
    }

    void Update()
    {
        Quaternion targetRotation = Quaternion.Euler(0, isOpen ? openAngle : closeAngle, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }

    void PlayDoorSound(AudioClip sound)
    {
        if (audioSource != null && sound != null)
        {
            audioSource.PlayOneShot(sound);
        }
    }
}
