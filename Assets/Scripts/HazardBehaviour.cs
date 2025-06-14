using UnityEngine;

public class HazardBehaviour : MonoBehaviour
{
    [SerializeField]
    int damage = -1;

    public void injure(PlayerBehaviour player)
    {
        player.ModifyHealth(damage);
    }
}
