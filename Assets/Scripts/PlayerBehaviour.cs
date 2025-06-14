using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PlayerBehaviour : MonoBehaviour
{
    int score = 0;
    int playerHealth = 100;
    int playerMaxHealth = 100;

    bool canInteract = false;

    CoinBehaviour currentCoin = null;

    DoorBehaviour currentDoor = null;

    HazardBehaviour currentHazard = null;

    CollectibleBehaviour currentCollectible = null;

    [SerializeField]
    Transform spawnPoint;

    [SerializeField]
    float interactionDistance = 5f;

    [SerializeField]
    TextMeshProUGUI scoreText;

    [SerializeField]
    TextMeshProUGUI healthText;

    [SerializeField]
    TextMeshProUGUI infoText;

    [SerializeField]
    Transform respawnPoint;

    [SerializeField]
    GameObject playerCharacter;

    [SerializeField]
    CharacterController characterController;

    [SerializeField]
    Rigidbody rb;

    bool keyObtained = false; // Flag to check if the key has been obtained

    int collectiblesObtained = 0; // Track the number of collectibles obtained

    void Start()
    {
        scoreText.text = "Score: " + score.ToString();
        healthText.text = "Health: " + playerHealth.ToString() + "/" + playerMaxHealth.ToString(); //displays health against max health
    }

    void Update()
    {
        RaycastHit hitInfo;
        Debug.DrawRay(spawnPoint.position, spawnPoint.forward * interactionDistance, Color.red);
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            //Debug.Log("Raycast hit: " + hitInfo.collider.gameObject.name);
            if (hitInfo.collider.CompareTag("Coin"))
            {
                if (currentCoin != null)
                {
                    // If we already have a coin highlighted, unhighlight it
                    currentCoin.Unhighlight();
                }
                canInteract = true; //if the ray hits a coin, set canInteract to true
                currentCoin = hitInfo.collider.gameObject.GetComponent<CoinBehaviour>(); //get the CoinBehaviour component from the detected object
                currentCoin.Highlight(); //highlight the coin to signify it can be collected
            }
            else if (hitInfo.collider.CompareTag("Door"))
            {

                canInteract = true; //if the ray hits a door, set canInteract to true
                currentDoor = hitInfo.collider.GetComponent<DoorBehaviour>();
            }
            else if (hitInfo.collider.GetComponent<CollectibleBehaviour>() != null) //check if the hit object has a collectibleBehaviour component
            {
                canInteract = true; //if the ray hits a collectible, set canInteract to true
                currentCollectible = hitInfo.collider.GetComponent<CollectibleBehaviour>();
                currentCollectible.Highlight(); //highlight the collectible to signify it can be collected
            }
        }
        else if (currentCoin != null)
        {
            // If the raycast did not hit a collectible, unhighlight the current coin
            currentCoin.Unhighlight();
            currentCoin = null; //reset the current coin
        }
        else if (currentCollectible != null)
        {
            // If the raycast did not hit a collectible, unhighlight the current collectible
            currentCollectible.Unhighlight();
            currentCollectible = null; //reset the current collectible
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Player collided with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Coin"))
        {
            currentCoin = collision.gameObject.GetComponent<CoinBehaviour>();
            currentCoin.collect(this);
        }
        else if (collision.gameObject.CompareTag("Hazard"))
        {
            currentHazard = collision.gameObject.GetComponent<HazardBehaviour>();
            currentHazard.injure(this);
        }
    }

    public void ModifyHealth(int amount)
    {
        playerHealth += amount;
        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth; // Ensure health does not exceed max health
        }
        else if (playerHealth <= 0)
        {
            characterController.enabled = false;
            rb.isKinematic = true; // Disable physics interactions
            transform.position = respawnPoint.position;
            playerHealth = playerMaxHealth; // Reset health to max on respawn
            rb.isKinematic = false; //re-enable physics interactions
            characterController.enabled = true; //re-enable character controller

        }
        healthText.text = "Health: " + playerHealth.ToString() + "/" + playerMaxHealth.ToString();
    }

    public void ModifyScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score.ToString();
    }

    void OnInteract()
    {
        Debug.Log("Interacting with object");
        // Handle interaction logic here
        if (canInteract)
        {
            if (currentCoin != null)
            {
                Debug.Log("Interacting with coin");
                currentCoin.collect(this);
                currentCoin = null;
                canInteract = false;
            }
            else if (currentDoor != null)
            {
                if (currentDoor.keyRequired && keyObtained == false)
                {
                    infoText.text = "Key required to open this door.";
                }
                else if (currentDoor.collectiblesNeeded > collectiblesObtained)
                {
                    infoText.text = "Collectibles needed to open this door: " + (currentDoor.collectiblesNeeded - collectiblesObtained);
                }
                else if (currentDoor.pointsRequired > score)
                {
                    infoText.text = "Points required to open this door: " + (currentDoor.pointsRequired - score);
                }
                else
                {
                    infoText.text = "...";
                    currentDoor.Interact();
                }
            }
            else if (currentCollectible != null)
            {
                if (currentCollectible.CompareTag("Key"))
                {
                    keyObtained = true;
                    collectiblesObtained++; // Increment collectibles obtained
                    Destroy(currentCollectible.gameObject); // Destroy the key collectible
                    canInteract = false; //No longer can interact with the key
                    currentCollectible = null;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (currentDoor != null && currentDoor.doorOpen == true)
        {
            currentDoor.Interact();
            canInteract = false;
            currentDoor = null;
        }
        else if (currentDoor != null)
        {
            infoText.text = ""; //removes text 
            currentDoor = null; //reset the current door
        }
    }

}

