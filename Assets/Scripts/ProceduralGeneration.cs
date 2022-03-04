using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class ProceduralGeneration : MonoBehaviour
{
    [Tooltip("Initial chance for a cell to become alive")]
    [Range(0,100)]
    public int initialChance;

    [Tooltip("If alive neighboring cells >= birthLimit, selected cell becomes alive")]
    [Range(1,8)]
    public int birthLimit;

    [Tooltip("If neighboring alive cells <= deathLimit, selected cell dies")]
    [Range(1,8)]
    public int deathLimit;

    [Tooltip("Number of times to repeat the life cycle")]
    [Range(1, 10)]
    public int numRepeats;

    public Vector3Int tileMapSize; // tile map size
    public Tilemap topMap; // Tile to place on top
    public Tilemap bottomMap; // Tile to place on bottom
    public Tile topTile;
    public Tile bottomTile;

    int width;
    int height;

    private int count = 0; // Number of save files
    private int[,] terrainMap; // 2D array map of 0s and 1s denoting living and dead cells

    public void doSim(int numRepeats)
    {
        clearMap(false);
        width = tileMapSize.x;
        height = tileMapSize.y;

        if(terrainMap == null)
        {
            terrainMap = new int[width, height];
            initPos();
        }

        for(int i = 0; i < numRepeats; i++)
        {
            terrainMap = genTilePos(terrainMap);
        }

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(terrainMap[x,y] == 1)
                    topMap.SetTile(new Vector3Int(-x + width/2, -y + height/2, 0), topTile);
                    bottomMap.SetTile(new Vector3Int(-x + width/2, -y + height/2, 0), bottomTile);
            }
        }
    }

    public int[,] genTilePos(int[,] oldMap)
    {
        int[,] newMap = new int[width, height];
        int neighorCount;
        BoundsInt myB = new BoundsInt(-1, -1, 0, 3, 3, 1);

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                neighorCount = 0;
                foreach(var b in myB.allPositionsWithin)
                {
                    if(b.x == 0 && b.y == 0) continue;
                    if (x + b.x >= 0 && x + b.x < width && y + b.y >=0 && y + b.y < height)
                    {
                        neighorCount += oldMap[x + b.x, y + b.y];
                    }
                    // else
                    // {
                    //     neighorCount++;
                    // }
                }
                if(oldMap[x,y] == 1)
                {
                    if(neighorCount < deathLimit) newMap[x,y] = 0;
                    else
                    {
                        newMap[x,y] = 1;
                    }
                }
                if(oldMap[x,y] == 0)
                {
                    if(neighorCount > birthLimit) newMap[x,y] = 1;
                    else
                    {
                        newMap[x,y] = 0;
                    }
                }
            }
        }

        return newMap;
    }

    public void clearMap(bool complete)
    {
        topMap.ClearAllTiles();
        bottomMap.ClearAllTiles();

        if(complete)
            terrainMap = null;
    }

    public void initPos()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                terrainMap[x,y] = Random.Range(1, 101) < initialChance ? 1 : 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            doSim(numRepeats);
        }
        if(Input.GetMouseButtonDown(1))
        {
            clearMap(true);
        }
    }
}
