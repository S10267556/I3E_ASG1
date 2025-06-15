using UnityEngine;

/*
* Author: Wong Zhi Lin
* Date: 15 June 2025
* Description: This script controls the behavior of collectible coins in the game. As well as allows the player to change its highlights.
*/

public class CoinBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    MeshRenderer myMeshRenderer;

    [SerializeField]
    Material highlightMap; // Sets the highlight color for the coin

    Material originalMap; // Stores the original color of the coin

    [SerializeField]
    int value = 0; // The value of the coin, which is added to the player's score when collected

    [SerializeField]
    AudioClip coinAudioClip; // Audio clip for playing sounds

    /// <summary>
    /// Collects the coin and adds its value to the player's score, as well as plays a sound effect.
    /// </summary>
    public void collect(PlayerBehaviour player)
    {
        player.ModifyScore(value);
        AudioSource.PlayClipAtPoint(coinAudioClip, transform.position); // Play the coin collection sound
        Destroy(gameObject);
    }

    /// <summary>
    /// Stores the MeshRenderer component and original color of the coin when the script is first initialized.
    /// </summary>
    void Start()
    {
        //Get the MeshRenderer component attached to this GameObject
        //Store it in the myMeshRenderer variable
        myMeshRenderer = GetComponent<MeshRenderer>();

        //store the original color of the coin
        originalMap = myMeshRenderer.material;
    }

    /// <summary>
    ///  Highlights the coin by changing its color to the highlight color.
    /// </summary>
    public void Highlight()
    {
        //change the color of the coin to the highlight color
        myMeshRenderer.material = highlightMap;
    }

    /// <summary>
    ///    Unhighlights the coin by changing its color back to the original color.
    /// </summary>
    public void Unhighlight()
    {
        //change the color of the coin back to the original color
        myMeshRenderer.material = originalMap;
    }
}
