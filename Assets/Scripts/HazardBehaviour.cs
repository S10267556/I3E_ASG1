using UnityEngine;
using TMPro;

public class HazardBehaviour : MonoBehaviour
{

    [SerializeField]
    int damage = -1;

    public int EnemyHealth = 5;

    [SerializeField]
    GameObject EnemyLootCoin;

    [SerializeField]
    GameObject EnemyLootSpecial;

    [SerializeField]
    Transform EnemyPosition;

    [SerializeField]
    int AppearCoins = 0;

    [SerializeField]
    bool isSpecial = false;

    public void injure(PlayerBehaviour player)
    {
        player.ModifyHealth(damage);
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
