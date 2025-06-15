using UnityEngine;

/*
* Author: Wong Zhi Lin
* Date: 15 June 2025
* Description: This script allows the box to release enemies when shot, and also drop a health potion if specified.
*/

public class PBox : MonoBehaviour
{
    [SerializeField]
    GameObject PandorasEnemies; // The enemy prefab to instantiate

    [SerializeField]
    GameObject HealthPotion; // The health potion prefab to instantiate

    [SerializeField]
    Transform PandorasBoxPosition; // The position where the enemies and health potion will appear

    [SerializeField]
    bool AppearSpecial = true; // Whether to instantiate the health potion

    [SerializeField]
    int AppearEnemy = 0; // How many enemies to spawn

    [SerializeField]
    GameObject Player; // To set the player reference for the enemies

    [SerializeField]
    AudioClip BoxAudioClip; // Audio clip for playing sounds

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            AudioSource.PlayClipAtPoint(BoxAudioClip, transform.position); // Play the box opening sound
            Destroy(gameObject);
            if (AppearSpecial == true)
            {
                Instantiate(HealthPotion, PandorasBoxPosition.position, PandorasBoxPosition.rotation);
                PandorasBoxPosition.position += new Vector3(1f, 0, 0); // Adjust position for next item
            }
            for (int i = 0; i < AppearEnemy; i++)
            {
                GameObject enemy = Instantiate(PandorasEnemies, PandorasBoxPosition.position, PandorasBoxPosition.rotation);
                PandorasBoxPosition.position += new Vector3(1f, 0, 0); // Adjust position for next enemy
                enemy.GetComponent<Follow>().Player = Player; // Set the player reference for the enemy
            }
        }
    }
}