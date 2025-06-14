using UnityEngine;

public class Injure : MonoBehaviour
{
    HazardBehaviour currentHazard;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hazard"))
        {
            currentHazard = other.gameObject.GetComponent<HazardBehaviour>(); //get the hazard behaviour component, to find out how much damage it does
            PlayerBehaviour player = GetComponent<PlayerBehaviour>();  //allows us to access the player
            currentHazard.injure(player); //use the hazard behaviour to injure the player
        }
    }
}
