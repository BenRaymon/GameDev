using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class ProceduralGeneration : MonoBehaviour
{
    [Range(0,100)]
    public int initialChance; // initial chance for each cell in a grid to become populated.

    [Range(1,8)]
    public int birthValue; // value for which a cell will become populated if neighboring populated cells are greater than this number.

    [Range(1,8)]
    public int deathValue; // value for which a cell will die if neighboring populated cells do not reach this number.

    [Range(1, 10)]
    public int numRepeats; // number of times to run the population checks.

    public int mapSizeRow; // Number of rows to generate in the grid.
    public int mapSizeColumn; // Number of columns to generate in the grid.
    public Tilemap terrain; // Tilemap to place tiles on. Corresponds with a populated cell with a value of 1 in the grid.
    public Tilemap background; // Tilemap to place tiles on. Corresponds with a dead cell with a value of 0 in the grid.
    public RuleTile terrainTile; // Tile to place.
    public Tile backgroundTile; // tile to place.
    
    public RuleTile groundTerrain;
    public RuleTile volcanoTerrain;
    private TimePeriods timePeriods;

    private bool firstChunk; // used to ensure player spawns only after first chunk has spawned.
    [SerializeField] private SpawnPlayer playerSpawner; 

    // Variable to keep track of where new platforms should be generated and deleted.
    private int xCoord = 0;
    private int currentDelete = 0;


    [SerializeField] private GameObject checkpointMarker;
    [SerializeField] private GameObject boundsMarker;

    private int[,] terrainMap; // 2D Array grid representing the map to be generated.

    void Awake()
    {
        timePeriods = new TimePeriods();

        xCoord = PlayerPrefs.GetInt("xCoord", 0);
        Debug.Log("Spawning at xCoord: " + xCoord);

        firstChunk = false;
        setTerrain("init");
        generateMap();
    }

    void Update()
    {
        // for debugging, resets saved xCoord
        if(Input.GetMouseButtonDown(2))
        {
            Debug.Log("Current xCoord: " + xCoord);
            Debug.Log("Resetting");
            PlayerPrefs.SetInt("xCoord", 0);
        }
    }

    public int getXCoord()
    {
        return xCoord;
    }

    // Handles the changing of terrain tiles.
    public void setTerrain(string platform)
    {
        switch(platform)
        {
            case ("Grass Terrain"):
                terrainTile = groundTerrain;
                break;
            case ("Volcanic Terrain"):
                terrainTile = volcanoTerrain;
                break;
            default:
                terrainTile = volcanoTerrain;
                break;
        }
    }

    public void generateMap()
    {
        terrainMap = new int[mapSizeRow, mapSizeColumn];
        initialPopulation(); // initially populates the grid with 0s and 1s representing dead and alive cells based on initialChance

        // Repeats population check to generate organic-looking features
        for(int i = 0; i < numRepeats; i++)
        {
            terrainMap = populationCheck(terrainMap);
        }

        //paintCells(xCoord);
        StartCoroutine(paintCellCoroutine(xCoord));

        xCoord += mapSizeRow; // keeps track of the current right-most x-position of the terrain generated.
    }

    // Coroutine version of removeChunk()
    public IEnumerator removeChunkCorotine()
    {
        //Debug.Log("Current delete: " + currentDelete);
        for(int row = currentDelete; row < currentDelete + mapSizeRow; row++)
        {
            for(int column = 0; column < mapSizeColumn; column++)
            {
                terrain.SetTile(new Vector3Int(row, column, 0), null); // removes painted tiles by setting them to null
                yield return null; // yields execution of function. Resumes at time returned. Null yields execution until the next update.
            }
        }
        currentDelete += mapSizeRow; // Keeps track of the next chunk's left-most x-coordinate to delete
    
        //Debug.Log("finished removing");
    }

    // Coroutine version of paintCells()
    private IEnumerator paintCellCoroutine(int xCoord)
    {
        for(int row = 0; row < mapSizeRow; row++)
        {
            // Moves checkpoint marker to the center point of the terrain generated.
            if(row == mapSizeRow/2)
            {
                //Debug.Log("Moving marker to (" + (row + xCoord) + "," + checkpointMarker.transform.position.y + ")");
                checkpointMarker.transform.position = new Vector2(row + xCoord, checkpointMarker.transform.position.y);
                boundsMarker.transform.position = new Vector2(row + xCoord, boundsMarker.transform.position.y);
            }

            setTerrain(timePeriods.getTimePeriod(row+xCoord));

            for(int column = 0; column < mapSizeColumn; column++)
            {
                if(terrainMap[row,column] == 1)
                {
                    terrain.SetTile(new Vector3Int(row + xCoord, column, 0), terrainTile); // Paints from the left, bottom to top
                }
                else
                    terrain.SetTile(new Vector3Int(row + xCoord, column, 0), backgroundTile);
            }

            yield return null; // yields execution of function. Resumes at time returned. Null yields execution until the next update.

        }

        if(!firstChunk){
            firstChunk = true;
            findSpawnLocation();
        }

        //Debug.Log("Finished painting");
    }

    //Populates the terrainMap with 1s and 0s based on an initialChance value
    //there should theoretically be an initialChance # of 1s for every 100 tiles
    private void initialPopulation()
    {
        for(int x = 0; x < mapSizeRow; x++)
        {
            for(int y = 0; y < mapSizeColumn; y++)
            {
                if(y > mapSizeColumn - 2)
                    terrainMap[x,y] = 1;
                else
                    terrainMap[x,y] = Random.Range(1, 101) < initialChance ? 1 : 0;
            }
        }
    }

    //For each cell in the grid 
    // count the number of neighbors that are in the map bounds that also have tiles (oldMap[x][y]=1)
    // the number of populatedNeighbors determines if the cell should be alive or dead
    private int[,] populationCheck(int[,] oldMap)
    {
        int[,] newMap = new int[mapSizeRow, mapSizeColumn];
        int populatedNeighborCount;
        BoundsInt cellsAroundCurrent = new BoundsInt(-1, -1, 0, 3, 3, 1);

        for(int row = 0; row < mapSizeRow; row++)
        {
            for(int column = 0; column < mapSizeColumn; column++)
            {
                populatedNeighborCount = 0;
                // goes through each of the 8 cells surrounding the current cell
                foreach(var cell in cellsAroundCurrent.allPositionsWithin)
                {
                    // Checks to see if current cell is the original cell being looked at. If it is, skip the current iteration.
                    if(cell.x == 0 && cell.y == 0)
                        continue;
                    // Checks to see if the surrounding neighbor cells are within the map bounds
                    // If they are, add the state of the neighbors to populatedNeighborCount.
                    // No subtraction is necessary to count living cells because non-living cells are set at 0.
                    if( (row + cell.x >= 0 && row + cell.x < mapSizeRow) && (column + cell.y >= 0 && column + cell.y < mapSizeColumn) )
                        populatedNeighborCount += oldMap[row + cell.x, column + cell.y];
                    // else
                    //     populatedNeighborCount++; // used for generating borders
                }
                
                // If the current cell is alive, checks to see if there are enough populated neighbors for it to remain alive.
                if(oldMap[row, column] == 1)
                {
                    // if there are not enough living neighbors, kill the cell. Else, keep the cell alive.
                    if(populatedNeighborCount < deathValue)
                        newMap[row, column] = 0;
                    else
                        newMap[row,column] = 1;
                }

                // If the current cell is dead, check to see if there are enough populated neighbors for it to be populated.
                if(oldMap[row,column] == 0)
                {
                    // If there are enough living neighbors, populate the cell. Else, keep the cell dead.
                    if(populatedNeighborCount > birthValue)
                        newMap[row,column] = 1;
                    else
                        newMap[row,column] = 0;
                }
            }
        }

        return newMap;
    }
        
    // Finds the first available piece of terrain to spawn the player at.
    public void findSpawnLocation()
    {
        Vector3Int cellPos;
        for(int row = 0; row < mapSizeRow; row++)
        {
            for(int column = mapSizeColumn - 1; column >= 0; column--)
            {
                if(terrainMap[row,column] == 1)
                {
                    cellPos = new Vector3Int(row + (xCoord - mapSizeRow),column,0);
                    Vector3 spawnLocation = terrain.GetCellCenterWorld(cellPos);
                    playerSpawner.spawnPlayer(spawnLocation); // spawns player at location
                    return;
                }
            }
        }
    }
}