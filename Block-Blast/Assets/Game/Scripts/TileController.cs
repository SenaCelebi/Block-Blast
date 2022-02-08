using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    public void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
   
    private void OnMouseDown()
    {
        Debug.Log("clicked");
    }

   
}
