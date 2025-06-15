using UnityEngine;

/*
* Author: Wong Zhi Lin
* Date: 15 June 2025
* Description: This script controls the highlights of collectible items in the game, which are special items like health, guns, and keys.
*/

public class CollectibleBehaviour : MonoBehaviour
{
    MeshRenderer myMeshRenderer;

    [SerializeField]
    Material highlightMap; // Sets the highlight color for the collectible

    Material originalMap; // Stores the original color of the collectible

    /// <summary>
    ///  Renders the collectible's mesh and initializes the original color.
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
    ///  Highlights the collectible by changing its color to the highlight color.
    /// </summary>
    public void Highlight()
    {
        //change the color of the collectible to the highlight color
        myMeshRenderer.material = highlightMap;
    }

    /// <summary>
    ///   Unhighlights the collectible by changing its color back to the original color.
    /// </summary>
    public void Unhighlight()
    {
        //change the color of the collectible back to the original color
        myMeshRenderer.material = originalMap;
    }
}
