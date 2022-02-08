using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Material[] tileMaterials;
    public static List<TileController> tileController = new List<TileController>();

    public static TileManager instance;
    public Transform[] spawnPoints;
    public GameObject tilePrefab;

     int colums = 6;
     int rows = 6;

    private WaitForSeconds pacing = new WaitForSeconds(0.1f);


    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("There is more than one Tile Manager");
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(GenerateTiles());
    }

    private IEnumerator GenerateTiles()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < colums; j++)
            {
                GameObject tile = Instantiate(tilePrefab, spawnPoints[j]);
                yield return pacing;
            }
        }
        yield return null;
    }

}
