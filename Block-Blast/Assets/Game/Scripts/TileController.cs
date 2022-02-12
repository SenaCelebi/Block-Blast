using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    [HideInInspector]
    public int color; 

    public TileType type;
    public Vector3Int coordinates;
    
   
    public void Initialize(Vector3Int coordinates,TileType type)
    {
        this.type = type;
        this.coordinates = coordinates;
        this.gameObject.transform.GetChild((int)type).gameObject.SetActive(true);
        color = (int)type;
        SetName(coordinates);  
    }

    public void SetName(Vector3Int coordinates)
    {
        name = "X: " + coordinates.x + " Y: " + coordinates.y;
    }
    
    private void OnMouseDown()
    {
        TileManager.instance.DestroyTiles(this);
        Debug.Log("Clicked Object X: "+ this.coordinates.x+ " Clicked Object Y: " + this.coordinates.y);
    }

   
}
