using UnityEngine;

public class InteractScript : MonoBehaviour
{
    public float interactDistance = 5f;
    public DoorScript door;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider.CompareTag("Door"))
            {
                DoorScript hitDoor = hit.collider.GetComponent<DoorScript>();

                if (!hitDoor.isOpen)
                {
                    hitDoor.OpenDoor();
                }
                else
                {
                    hitDoor.CloseDoor();
                }
            }
        }
    }
}
