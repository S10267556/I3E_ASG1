using UnityEngine;
using TMPro;

/*
* Author: Wong Zhi Lin
* Date: 15 June 2025
* Description: This script handles the behavior of hazards in the game, such as enemires and traps. It allows the players 
to defeat enemies by shooting at them, then dropping loot accordingly.
*/

public class HazardBehaviour : MonoBehaviour
{

    [SerializeField]
    int damage = -1; // The amount of damage the hazard inflicts on the player

    public int EnemyHealth = 5; // The health of the enemy, which decreases when hit by a bullet

    [SerializeField]
    GameObject EnemyLootCoin; //The coin which the enemy drops when defeated

    [SerializeField]
    GameObject EnemyLootSpecial; // The special loot which the enemy drops when defeated

    [SerializeField]
    Transform EnemyPosition; // The position where the enemy loot will appear

    [SerializeField]
    int AppearCoins = 0; // How many coins the enemy will drop

    [SerializeField]
    bool isSpecial = false; // Whether the enemy drops special loot

    public AudioSource hazardAudioSource; // Audio source for playing sounds

    void Start()
    {
        hazardAudioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to the hazard
    }

    public void injure(PlayerBehaviour player)
    {
        player.ModifyHealth(damage);
        hazardAudioSource.Play(); // Play the hazard sound when the player is injured
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && gameObject.CompareTag("Enemy"))
        {
            if (EnemyHealth > 0)
            {
                EnemyHealth--;
            }
            else
            {
                if (AppearCoins > 0)
                {
                    for (int i = 0; i < AppearCoins; i++)
                    {
                        Instantiate(EnemyLootCoin, EnemyPosition.position, EnemyPosition.rotation);
                        EnemyPosition.position += new Vector3(0.5f, 0, 0); // Adjust position for next coin
                    }
                }

                if (isSpecial == true)
                {
                    Instantiate(EnemyLootSpecial, EnemyPosition.position, EnemyPosition.rotation);
                    EnemyPosition.position += new Vector3(0.5f, 0, 0);
                    for (int i = 0; i < AppearCoins; i++)
                    {
                        Instantiate(EnemyLootCoin, EnemyPosition.position, EnemyPosition.rotation);
                        EnemyPosition.position += new Vector3(0.5f, 0, 0); // Adjust position for next coin
                    }
                }

                Destroy(gameObject);
            }
        }
    }
}
