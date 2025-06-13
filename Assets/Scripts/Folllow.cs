using UnityEngine;

public class Folllow : MonoBehaviour
{

    public GameObject ChaserEnemy;
    public GameObject Player;
    public float speed;
    // Update is called once per frame
    void Update()
    {
        ChaserEnemy.transform.position = Vector3.MoveTowards(ChaserEnemy.transform.position, Player.transform.position, speed);
    }
}
