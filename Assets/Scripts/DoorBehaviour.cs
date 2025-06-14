using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
   public void Interact()
    {
        if (PlayerBehaviour.doorOpen == false)
        {
            Vector3 doorRotation = transform.eulerAngles;
            doorRotation.y += 90f; // Rotate the door by 90 degrees on the Y-axis
            transform.eulerAngles = doorRotation;
            PlayerBehaviour.doorOpen = true; // Set the door state to open
        }
        else
        {
            Vector3 doorRotation = transform.eulerAngles;
            doorRotation.y -= 90f; // Rotate the door back by 90 degrees on the Y-axis
            transform.eulerAngles = doorRotation;
            PlayerBehaviour.doorOpen = false; // Set the door state to closed
        }
    }
}
