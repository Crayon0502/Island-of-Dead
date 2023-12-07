using UnityEngine;
using StarterAssets;

public class InteractScript : MonoBehaviour
{
    private StarterAssetsInputs playerInput;
    public float interactDistance = 5f;
    public DoorScript door;

    void Start()
    {
        playerInput = GetComponentInParent<StarterAssetsInputs>();
    }

    void Update()
    {
        if (playerInput.interaction)
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        playerInput.interaction = false;
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