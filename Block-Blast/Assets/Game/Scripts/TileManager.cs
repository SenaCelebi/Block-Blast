using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public TileController[,] tileArr = new TileController[6,6];
    public static TileManager instance;
    public Transform[] spawnPoints;
    public GameObject tilePrefab;

    int colums = 6;
    int rows = 6;
    /*
    int A = 4;
    int B = 7;
    int C = 9;
    */
    private WaitForSeconds pacing = new WaitForSeconds(0.1f);

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There is more than one Tile Manager");
        }
        else
        {
            instance = this;
        }
    }

    private void StartGame()
    {
        StartCoroutine(GenerateTiles());
    }

    public void DestroyTiles(TileController tileController)
    {
        List<TileController> neighbors = FindBlocks(tileController);

        if (neighbors.Count > 1)
        {

            StartCoroutine(Check(tileController, neighbors));

        }
    }
    private IEnumerator Check(TileController tileController, List<TileController> neighbors)
    {
        foreach (TileController t in neighbors)
        {
            Destroy(t.gameObject);
        }
        UpdateTileList();
        yield return new WaitForSeconds(0.1f);
        foreach (TileController t in neighbors)
        {
            CreateTileInColumn(t.coordinates);
            WaitForSeconds a = new WaitForSeconds(0.1f);
            yield return a;
        }
    }

    private IEnumerator ReassignCoordinates()
    {
        yield return new WaitForEndOfFrame();
        for(int x = 0; x<6; x++)
        {
            for(int y =0; y<6;y++)
            {
                if(tileArr[x,y] != null)
                {
                    TileController tile = tileArr[x,y];
                    int parentIndex = 0;
                    for (int i = 0; i < spawnPoints.Length; i++)
                    {
                        if (spawnPoints[i] == tile.transform.parent)
                        {
                            parentIndex = i;
                            break;
                        }
                    }
                    tile.coordinates = new Vector3Int(parentIndex, tile.transform.GetSiblingIndex(), 0);
                    tile.SetName(tile.coordinates);
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
        UpdateTileList();
    }

    private void CreateTileInColumn(Vector3Int coordinate)
    {
        GameObject tile = Instantiate(tilePrefab, spawnPoints[coordinate.x]);
        int randomNum = Random.Range(0, 4);
        tile.GetComponent<TileController>().Initialize(coordinate, (TileType)randomNum);
        StartCoroutine(ReassignCoordinates());
    }

    private IEnumerator GenerateTiles()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < colums; j++)
            {
                GameObject tile = Instantiate(tilePrefab, spawnPoints[j]);
                Vector3Int coordinate = new Vector3Int(j, i, 0);
                int randomNum = Random.Range(0, 4);
                tile.GetComponent<TileController>().Initialize(coordinate, (TileType)randomNum);
              
                yield return pacing;
            }
        }
        UpdateTileList();
        yield return new WaitForSeconds(0.2f);
        ChangeMaterails();
        yield return null;
    }

    private void UpdateTileList()
    {
        tileArr = new TileController[6, 6];
        int x = 0;
        foreach (Transform g in spawnPoints)
        {
            //Debug.Log(g.name);
            int y = 0;
            foreach (Transform t in g.transform)
            {
                //Debug.Log(t.name +" tile");
                Vector3Int coordinate = new Vector3Int(x, y, 0);             
                t.GetComponent<TileController>().coordinates = coordinate;
                tileArr[x, y] = t.GetComponent<TileController>();
                y++;
            }
            x++;
        }
        ChangeMaterails();

    }

    private void ChangeIcons(List<TileController> list, int colorCode)
    {
        foreach (TileController t in list)
        {
            t.transform.GetChild((int)t.type).gameObject.SetActive(false);
            t.type = (TileType)colorCode;
            t.transform.GetChild(colorCode).gameObject.SetActive(true);         
        }
    }

    private void ChangeMaterails()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
            List<TileController> listTemp = FindBlocks(tileArr[i,j]);
            int size = listTemp.Count;
            int colorCode = (int) listTemp[0].type;

           // Debug.Log("COLOR CODE: " + colorCode);

                if (size > 4 && size < 8)
                {
                    //First Icon
                    colorCode = listTemp[0].color + 4;
                    colorCode = Mathf.Clamp(colorCode, 0, 15);
                    ChangeIcons(listTemp, colorCode);

                }
                else if (size >= 8 && size < 10)
                {
                    //Second Icon
                    colorCode = listTemp[0].color + 8;
                    colorCode = Mathf.Clamp(colorCode, 0, 15);
                    ChangeIcons(listTemp, colorCode);
                   
                }
                else if (size >= 10)
                {
                    //Third Icon
                    colorCode = listTemp[0].color + 12;
                    colorCode = Mathf.Clamp(colorCode, 0, 15);
                    ChangeIcons(listTemp, colorCode);
                }
            }
        }
    }

    private List<GameObject> SearchNeighbors(TileController tile)
    {
        List<GameObject> neighbors = new List<GameObject>();
        int xIndex = tile.coordinates.x;
        int yIndex = tile.coordinates.y;

        if (xIndex - 1 >= 0 && xIndex - 1 < 6)
        {
            if (tileArr[xIndex - 1, yIndex] != null && tileArr[xIndex-1,yIndex].color == tile.color)
            {
                neighbors.Add(tileArr[xIndex - 1,yIndex].gameObject);
                // Debug.Log("LEFT X: "+tileList[left].coordinates.x);
                //  Debug.Log("LEFT Y: " + tileList[left].coordinates.y);
            }

        }
        if (xIndex + 1 >= 0 && xIndex + 1 < 6)
        {
            if (tileArr[xIndex + 1, yIndex ] != null && tileArr[xIndex + 1, yIndex].color == tile.color)
            {
                neighbors.Add(tileArr[xIndex + 1, yIndex].gameObject);
                // Debug.Log("LEFT X: "+tileList[left].coordinates.x);
                //  Debug.Log("LEFT Y: " + tileList[left].coordinates.y);
            }
        }
        if (yIndex + 1 >= 0 && yIndex + 1 < 6)
        {
            if (tileArr[xIndex, yIndex + 1] != null && tileArr[xIndex, yIndex+1].color == tile.color)
            {
                neighbors.Add(tileArr[xIndex , yIndex+1].gameObject);
                // Debug.Log("LEFT X: "+tileList[left].coordinates.x);
                //  Debug.Log("LEFT Y: " + tileList[left].coordinates.y);
            }

        }
        if (yIndex - 1 >= 0 && yIndex - 1 < 6)
        {
            if (tileArr[xIndex, yIndex - 1] != null && tileArr[xIndex , yIndex-1].color == tile.color)
            {
                neighbors.Add(tileArr[xIndex , yIndex-1].gameObject);
                // Debug.Log("LEFT X: "+tileList[left].coordinates.x);
                //  Debug.Log("LEFT Y: " + tileList[left].coordinates.y);
            }
        }
       
        //  Debug.Log("NEIGHBORS: "+ neighbors.Count);

        return neighbors;
    }

    private List<TileController> FindBlocks(TileController tile)
    {
        List<GameObject> tileBlockList = new List<GameObject>();
        Stack<TileController> tileStack = new Stack<TileController>();
        if (tile.gameObject != null)
        {
        tileBlockList.Add(tile.gameObject);
        tileStack.Push(tile);

        while (tileStack.Count > 0)
        {
            List<GameObject> currentNeighbors = SearchNeighbors(tileStack.Peek());
            tileStack.Pop();
            foreach (GameObject g in currentNeighbors)
            {
                if (!tileBlockList.Contains(g))
                {
                    tileBlockList.Add(g);
                    tileStack.Push(g.GetComponent<TileController>());
                    }
            }
        }
        }

        List<TileController> tileControllerList = new List<TileController>();
      
        foreach (GameObject g in tileBlockList)
        {
            tileControllerList.Add(g.GetComponent<TileController>());
        }
        
        return tileControllerList;
    }

}
