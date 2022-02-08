using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Tile tile;
 

 
    public void Initialize(Tile tile)
    {
        this.tile = tile;
        meshRenderer.material = TileManager.instance.tileMaterials[(int)tile.type];

    }
   
    private void OnMouseDown()
    {
        Debug.Log("clicked");
    }

   
}
