using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool isOpen = false;
    public float openAngle = 90f;
    public float closeAngle = 0f;
    public float smooth = 2f;

    public void OpenDoor()
    {
        isOpen = true;
    }

    public void CloseDoor()
    {
        isOpen = false;
    }

    void Update()
    {
        Quaternion targetRotation = Quaternion.Euler(0, isOpen ? openAngle : closeAngle, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}
