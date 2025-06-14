using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public bool doorOpen = false; // Track the state of the door

    public bool keyRequired = false; // Flag to check if a key is required to open the door

    public int pointsRequired = 0; // Points required to open the door

    public int collectiblesNeeded = 0; // Number of collectibles needed to open the door

    public void Interact()
    {
        if (doorOpen == false)
        {
            Vector3 doorRotation = transform.eulerAngles;
            doorRotation.y += 90f; // Rotate the door by 90 degrees on the Y-axis
            transform.eulerAngles = doorRotation;
            doorOpen = true; // Set the door state to open
        }
        else
        {
            Vector3 doorRotation = transform.eulerAngles;
            doorRotation.y -= 90f; // Rotate the door back by 90 degrees on the Y-axis
            transform.eulerAngles = doorRotation;
            doorOpen = false; // Set the door state to closed
        }
    }
}
