using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

/*
* Author: Wong Zhi Lin
* Date: 15 June 2025
* Description: This script handles the player's behavior, which is largely interaction with the game world.
* It allows the player to collect coins, interact with doors, obtain collectibles, and manage health and score.
*/

public class PlayerBehaviour : MonoBehaviour
{
    int score = 0; //keeps the player's score
    int playerHealth = 100; //keeps the player's health
    int playerMaxHealth = 100; //keeps the player's max health

    bool canInteract = false; //Tracks whether the item can be interacted with

    CoinBehaviour currentCoin = null; //holds the coin that the player is currently interacting with

    DoorBehaviour currentDoor = null; //holds the door that the player is currently interacting with

    HazardBehaviour currentHazard = null; //holds the hazard that the player is currently interacting with

    CollectibleBehaviour currentCollectible = null; //holds the collectible that the player is currently interacting with

    [SerializeField]
    Transform spawnPoint; //Allows user to set the spawn point for raycasting

    [SerializeField]
    float interactionDistance = 5f; //the distance at which the player can interact with objects/which the raycast can reach

    [SerializeField]
    TextMeshProUGUI scoreText; // Text to display the player's score

    [SerializeField]
    TextMeshProUGUI healthText; // Text to display the player's health

    public TextMeshProUGUI infoText; // Text to display information about the current interaction

    [SerializeField]
    TextMeshProUGUI congratsText; // Text to display the congratulations message once all collectibles are obtained

    [SerializeField]
    Image congratsBg; // Background colour for the congratulations message

    [SerializeField]
    GameObject FinalScene; // The final scene that appears when the player completes the game

    [SerializeField]
    TextMeshProUGUI finalScoreText; // Text to display the final score

    [SerializeField]
    TextMeshProUGUI finalCollectiblesText; // Text to display the final collectibles count

    [SerializeField]
    Transform respawnPoint; // The point where the player will respawn

    [SerializeField]
    GameObject playerCharacter; // The player character GameObject

    [SerializeField]
    CharacterController characterController; // To set the player's character controller

    [SerializeField]
    Rigidbody rb; // To set the player's rigidbody

    bool keyObtained = false; // Flag to check if the key has been obtained

    bool gunObtained = false; // Flag to check if the gun has been obtained

    int collectiblesObtained = 0; // Track the number of collectibles obtained

    [SerializeField]
    GameObject projectile; // Allows player to set the projectile to be fired

    [SerializeField]
    float fireStrength = 0f; // How strong the projectile will be fired/the force applied to the projectile

    /// <summary>
    /// Spawns the player in, displaying both health and score on the screen, while hiding the ending ui.
    /// </summary>
    void Start()
    {
        scoreText.text = "Score: " + score.ToString();
        healthText.text = "Health: " + playerHealth.ToString() + "/" + playerMaxHealth.ToString(); //displays health against max health
        congratsBg.gameObject.SetActive(false); // Hide the congrats background initially
        FinalScene.gameObject.SetActive(false); // Hide the final scene initially
    }

    /// <summary>
    /// Sends a raycast out from a spawnpoint infront of the player to detect interactable objects.
    ///  It checks for coins, doors, collectibles, hazards, and displays relevant information.
    /// If the raycast hits a coin, it highlights the coin and allows interaction.
    ///  If it hits a door, it allows interaction with the door.
    /// If it hits a collectible, it highlights the collectible and allows interaction.
    /// If it hits an enemy, it displays the enemy's health.
    /// If it hits a Pandora's Box, it displays a message indicating that it can be opened with a strong force, signaling that "F" is to be pressed.
    ///  If the raycast does not hit any interactable objects, it clears the highlighted objects and info text.
    /// </summary>
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

    /// <summary>
    /// When the player collides with objects, it checks if the object is a coin, hazard, or enemy.
    ///  If it is a coin, it collects the coin and updates the score.
    /// If it is a hazard, it injures the player and plays the hazard sound.
    ///  If it is an enemy, it injures the player and plays the hazard sound, displaying a message about the injury.
    /// </summary>
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
            currentHazard.hazardAudioSource.Play(); // Play the hazard sound
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

    /// <summary>
    /// Modifies the player's health by the amount specified.
    ///  If the health exceeds the maximum health, it is capped at the maximum.
    /// If the health drops to zero or below, it stops player movement, returning the player's position to the respawn point, before allowing the player to continue moving.
    ///  If the player respawns, their health is reset to the maximum health.
    /// </summary>


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

    /// <summary>
    /// Modifies the player's score by the amount specified.
    /// It updates the score text to reflect the new score.
    /// </summary>
    
    public void ModifyScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score.ToString();
    }

    /// <summary>
    ///  Handles player interaction with objects when the interact button is pressed.
    /// It checks if the player can interact with a coin, door, or collectible.
    ///  If the player interacts with a coin, it collects the coin and updates the score, as well as displays a message.
    /// If the player interacts with a door, it checks if a key is required, if enough collectibles or points are available, and then opens the door or displays an appropriate message.
    ///  If the player interacts with a collectible, it checks the type of collectible (key, gun, heal, or gold) and applies the corresponding effects, such as obtaining a key, gun, healing the player, or adding points to the score.
    /// </summary>
    void OnInteract()
    {
        Debug.Log("Interacting with object");
        // Handle interaction logic here
        if (canInteract)
        {
            if (currentCoin != null)
            {
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
                    infoText.text = "Turn back. You can't leave empty handed. - " + collectiblesObtained + "/" + currentDoor.collectiblesNeeded + " Secrets";
                }
                else if (currentDoor.pointsRequired > score)
                {
                    infoText.text = "Pathetic. -  " + (currentDoor.pointsRequired - score) + "/" + currentDoor.pointsRequired + " Points";
                }
                else if (currentDoor.finalDoor == true)
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

    /// <summary>
    ///  Checks if all collectibles have been obtained.
    /// If the player has obtained the required number of collectibles, it displays a congratulations message.
    /// </summary>
    void AllCollectiblesObtained()
    {
        if (collectiblesObtained >= 5) // Assuming 5 is the total number of collectibles needed
        {
            congratsText.text = "You found all of the special items. Keep your guard up.";
            congratsBg.gameObject.SetActive(true); // Show the background
        }
    }

    /// <summary>
    /// Game ending scene.
    ///  It activates the final scene, displays the final score and collectibles obtained, as well as hides the player character.
    /// </summary>
    public void Final()
    {
        FinalScene.gameObject.SetActive(true); // Activate the final scene
        finalScoreText.text = "Points earned: " + score.ToString();
        finalCollectiblesText.text = "Special items collected: " + collectiblesObtained.ToString() + " / 5";
        playerCharacter.SetActive(false); // Hide the player character
    }

    /// <summary>
    /// Used to close the door when the player exits the trigger collider of the door.
    /// If the door is open, it closes the door and sets canInteract to false.
    /// </summary>
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

    /// <summary>
    ///  Fires a projectile from the spawn point if the player has obtained a gun.
    /// If the gun is not obtained, it displays a message as a hint that the player should look for a gun.
    /// </summary>
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

