using UnityEngine;

public class CollectibleBehaviour : MonoBehaviour
{
    MeshRenderer myMeshRenderer;

    [SerializeField]
    Material highlightMap;

    Material originalMap;

    //public void collect(PlayerBehaviour player)
    //{
    //    Destroy(gameObject);
    //} ???????

    void Start()
    {
        //Get the MeshRenderer component attached to this GameObject
        //Store it in the myMeshRenderer variable
        myMeshRenderer = GetComponent<MeshRenderer>();

        //store the original color of the coin
        originalMap = myMeshRenderer.material;
    }

    public void Highlight()
    {
        //change the color of the collectible to the highlight color
        myMeshRenderer.material = highlightMap;
    }

    public void Unhighlight()
    {
        //change the color of the collectible back to the original color
        myMeshRenderer.material = originalMap;
    }
}
