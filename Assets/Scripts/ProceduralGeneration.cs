using UnityEngine;
using UnityEngine.Tilemaps;
using System.Text;

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

    [SerializeField] private Grid tileGrid;

    private int[,] terrainMap; // 2D Array grid representing the map to be generated.

    void Awake()
    {
        terrainMap = new int[mapSizeRow, mapSizeColumn];
        initialPopulation(); // initially populates the grid with 0s and 1s representing dead and alive cells based on initialChance

        // Repeats population check to generate organic-looking features
        for(int i = 0; i < numRepeats; i++)
        {
            terrainMap = populationCheck(terrainMap);
        }

        paintCells();
    }

    // Fills cells with appropriate tile based on whether the cell is alive or dead.
    private void paintCells()
    {
        for(int row = 0; row < mapSizeRow; row++)
        {
            for(int column = 0; column < mapSizeColumn; column++)
            {
                if(terrainMap[row,column] == 1)
                {
                    Vector3Int test = new Vector3Int(row, column, 0);
                    terrain.SetTile(new Vector3Int(row, column, 0), terrainTile); // Paints from the left, bottom to top
                    // Debug.Log("Painting from: " + test);
                }
                else
                    terrain.SetTile(new Vector3Int(row, column, 0), backgroundTile);
            }
        }
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
<<<<<<< HEAD
        
        
=======
>>>>>>> c76c7e0 (improved procedural generation, added rule tile.)
    public Vector2 findLocation()
    {
        Vector2 spawnLocation = new Vector2(0,0);
        for(int row = 0; row < mapSizeRow; row++)
        {
            for(int column = mapSizeColumn - 1; column >= 0; column--)
            {
                if(terrainMap[row,column] == 1)
                {
                    Debug.Log(terrain.GetTile(new Vector3Int(row, column, 0)));
                    spawnLocation = new Vector2(row,column);
                    Debug.Log("Spawning: " + spawnLocation);
                    return spawnLocation;
                }
            }
        }
        return spawnLocation;
    }

    private void printArray()
    {
        StringBuilder sb = new StringBuilder();
        for(int x = 0; x < mapSizeRow; x++)
        {
            for(int y = 0; y < mapSizeColumn; y++)
            {
                sb.Append(terrainMap[x,y]);
                sb.Append(" ");
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }
}