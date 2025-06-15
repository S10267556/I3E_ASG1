using UnityEngine;

/*
* Author: Wong Zhi Lin
* Date: 15 June 2025
* Description: This script makes the enemy follow the player.
*/

public class Follow : MonoBehaviour
{

    public GameObject ChaserEnemy; // The enemy that will chase the player
    public GameObject Player; // The player that the enemy will follow
    public float speed;
    // Update is called once per frame

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        ChaserEnemy.transform.position = Vector3.MoveTowards(ChaserEnemy.transform.position, Player.transform.position, speed);
    }
}
