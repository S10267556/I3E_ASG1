using UnityEngine;

public class PBox : MonoBehaviour
{
    [SerializeField]
    GameObject PandorasEnemies;

    [SerializeField]
    GameObject HealthPotion;

    [SerializeField]
    Transform PandorasBoxPosition;

    [SerializeField]
    bool AppearSpecial = true;

    [SerializeField]
    int AppearEnemy = 0;

    [SerializeField]
    GameObject Player;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
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