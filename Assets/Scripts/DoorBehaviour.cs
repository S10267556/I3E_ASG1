using UnityEngine;

/*
* Author: Wong Zhi Lin
* Date: 15 June 2025
* Description: This script contains the behaviour for a door in the game.
*/

public class DoorBehaviour : MonoBehaviour
{
    AudioSource doorAudioSource; // Audio source for playing sounds

    public bool doorOpen = false; // Track the state of the door

    public bool keyRequired = false; // Flag to check if a key is required to open the door

    public int pointsRequired = 0; // Points required to open the door

    public int collectiblesNeeded = 0; // Number of collectibles needed to open the door

    public bool finalDoor = false; // Flag to check if this is the final door in the game

    void Start()
    {
        doorAudioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to the door
    }

    public void Interact()
    {
        if (doorOpen == false)
        {
            Vector3 doorRotation = transform.eulerAngles;
            doorRotation.y += 90f; // Rotate the door by 90 degrees on the Y-axis
            transform.eulerAngles = doorRotation;
            doorOpen = true; // Set the door state to open
            doorAudioSource.Play(); // Play the door opening sound
        }
        else
        {
            Vector3 doorRotation = transform.eulerAngles;
            doorRotation.y -= 90f; // Rotate the door back by 90 degrees on the Y-axis
            transform.eulerAngles = doorRotation;
            doorOpen = false; // Set the door state to closed
            doorAudioSource.Play(); // Play the door closing sound
        }
    }
}
