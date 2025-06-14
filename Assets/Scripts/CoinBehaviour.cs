using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    MeshRenderer myMeshRenderer;

    [SerializeField]
    Material highlightMap;

    Material originalMap;

    [SerializeField]
    int value = 0;

    public void collect(PlayerBehaviour player)
    {
        player.ModifyScore(value);
        Destroy(gameObject);
    }

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
        //change the color of the coin to the highlight color
        myMeshRenderer.material = highlightMap;
    }

    public void Unhighlight()
    {
        //change the color of the coin back to the original color
        myMeshRenderer.material = originalMap;
    }
}
