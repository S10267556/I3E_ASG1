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

    public TextMeshProUGUI infoText;

    [SerializeField]
    TextMeshProUGUI congratsText;

    [SerializeField]
    Image congratsBg;

    [SerializeField]
    GameObject FinalScene;

    [SerializeField]
    TextMeshProUGUI finalScoreText;

    [SerializeField]
    TextMeshProUGUI finalCollectiblesText;

    [SerializeField]
    Transform respawnPoint;

    [SerializeField]
    GameObject playerCharacter;

    [SerializeField]
    CharacterController characterController;

    [SerializeField]
    Rigidbody rb;

    bool keyObtained = false; // Flag to check if the key has been obtained

    bool gunObtained = false; // Flag to check if the gun has been obtained

    int collectiblesObtained = 0; // Track the number of collectibles obtained

    [SerializeField]
    GameObject projectile;

    [SerializeField]
    float fireStrength = 0f;


    void Start()
    {
        scoreText.text = "Score: " + score.ToString();
        healthText.text = "Health: " + playerHealth.ToString() + "/" + playerMaxHealth.ToString(); //displays health against max health
        congratsBg.gameObject.SetActive(false); // Hide the congrats background initially
        FinalScene.gameObject.SetActive(false); // Hide the final scene initially
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
                if (currentCollectible != null)
                {
                    // If we already have a collectible highlighted, unhighlight it
                    currentCollectible.Unhighlight();
                }
                canInteract = true; //if the ray hits a collectible, set canInteract to true
                currentCollectible = hitInfo.collider.GetComponent<CollectibleBehaviour>();
                currentCollectible.Highlight(); //highlight the collectible to signify it can be collected
            }
            else if (hitInfo.collider.CompareTag("Enemy"))
            {
                HazardBehaviour hazard = hitInfo.collider.GetComponent<HazardBehaviour>();
                if (hazard != null && hazard.EnemyHealth > 0)
                {
                    infoText.text = "Health: " + hazard.EnemyHealth;
                }
            }
            else if (hitInfo.collider.CompareTag("PandorasBox"))
            {
                infoText.text = "You reckon that a strong 'F'orce could open this.";
            }
        }
        else if (currentCoin != null)
        {
            // If the raycast did not hit a collectible, unhighlight the current coin
            currentCoin.Unhighlight();
            canInteract = false; //set canInteract to false
            currentCoin = null; //reset the current coin
        }
        else if (currentCollectible != null)
        {
            // If the raycast did not hit a collectible, unhighlight the current collectible
            currentCollectible.Unhighlight();
            canInteract = false; //set canInteract to false
            currentCollectible = null; //reset the current collectible
        }
        else if (currentHazard != null)
        {
            currentHazard = null;
            infoText.text = ""; // Clear the info text if no hazard is detected
        }
        else if (hitInfo.collider.CompareTag("PandorasBox") == false)
        {
            infoText.text = ""; // Clear the info text if no interactable object is detected
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
        else if (collision.gameObject.GetComponent<HazardBehaviour>() != null)
        {
            currentHazard = collision.gameObject.GetComponent<HazardBehaviour>();
            currentHazard.injure(this);
            if (collision.gameObject.CompareTag("Hazard"))
            {
                infoText.text = "Do your eyes grow on the back of your head? Watch it.";
            }
            else if (collision.gameObject.CompareTag("Enemy"))
            {
                infoText.text = "A stabbing pain, a throbbing ache. You look down and see nothing, but something is off about you.";
            }
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
            infoText.text = "Watch your surroundings.";
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
                infoText.text = "That's not going to save you. Keep moving.";
                currentCoin.collect(this);
                currentCoin = null;
                canInteract = false;
            }
            else if (currentDoor != null)
            {
                if (currentDoor.keyRequired && keyObtained == false)
                {
                    infoText.text = "The corroded padlock creaks, but does not budge. You need a key.";
                }
                else if (currentDoor.collectiblesNeeded > collectiblesObtained)
                {
                    infoText.text = "Turn back. You can't leave empty handed. - " + (currentDoor.collectiblesNeeded - collectiblesObtained) + "/" + currentDoor.collectiblesNeeded + " Secrets";
                }
                else if (currentDoor.pointsRequired > score)
                {
                    infoText.text = "Pathetic. -  " + (currentDoor.pointsRequired - score) + "/" + currentDoor.pointsRequired + " Points";
                }
                else if (currentDoor.collectiblesNeeded == collectiblesObtained)
                {
                    infoText.text = "You feel a sense of dread as you open the door.";
                    Final();
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
                    infoText.text = "Rusted. But it will do.";
                }
                else if (currentCollectible.CompareTag("Gun"))
                {
                    gunObtained = true;
                    infoText.text = "Don't hesitate. Shoot.";
                }
                else if (currentCollectible.CompareTag("Heal"))
                {
                    ModifyHealth(20); // Heal the player by 20 health points
                    infoText.text = "Just a sip forces bile up your throat. Keep it down. It might just save you.";
                }
                else if (currentCollectible.CompareTag("Gold"))
                {
                    ModifyScore(10000); // Add 10,000 points to the score
                    infoText.text = "Look away. Look away. Look away. Look away. Look away.";
                }
                collectiblesObtained++; // Increment collectibles obtained
                Destroy(currentCollectible.gameObject); // Destroy the collectible
                canInteract = false; //No longer can interact with the collectible
                currentCollectible = null;
                AllCollectiblesObtained(); // Check if all collectibles have been obtained
            }
        }
    }

    void AllCollectiblesObtained()
    {
        if (collectiblesObtained >= 5) // Assuming 5 is the total number of collectibles needed
        {
            congratsText.text = "You found all of the special items. Keep your guard up.";
            congratsBg.gameObject.SetActive(true); // Show the background
        }
    }

    public void Final()
    {
        FinalScene.gameObject.SetActive(true); // Activate the final scene
        finalScoreText.text = "Points earned: " + score.ToString();
        finalCollectiblesText.text = "Special items collected: " + collectiblesObtained.ToString() + " / 5";
        playerCharacter.SetActive(false); // Hide the player character
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

    void OnFire()
    {
        if (gunObtained == true)
        {
            GameObject newProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
            Vector3 fireForce = spawnPoint.forward * fireStrength;
            newProjectile.GetComponent<Rigidbody>().AddForce(fireForce);
        }
        else
        {
            infoText.text = "You smell traces of gunpowder near you.";
        }
    }
}

