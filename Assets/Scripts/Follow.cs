using UnityEngine;

public class Follow : MonoBehaviour
{

    public GameObject ChaserEnemy;
    public GameObject Player;
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
