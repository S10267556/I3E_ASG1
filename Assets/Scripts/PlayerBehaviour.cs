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

    public static bool doorOpen = false;

    [SerializeField]
    Transform spawnPoint;

    [SerializeField]
    float interactionDistance = 5f;

    [SerializeField]
    TextMeshProUGUI scoreText;

    [SerializeField]
    TextMeshProUGUI healthText;

    [SerializeField]
    Transform respawnPoint;

    [SerializeField]
    GameObject playerCharacter;

    [SerializeField]
    CharacterController characterController;

    [SerializeField]
    Rigidbody rb;

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
                Debug.Log("Interacting with door");
                currentDoor.Interact();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player is looking at " + other.gameObject.name);
        if (other.CompareTag("Door"))
        {
            canInteract = true;
            currentDoor = other.gameObject.GetComponent<DoorBehaviour>();
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            currentDoor.Interact();
            canInteract = false;
            currentDoor = null;
        }
    }

}

