using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{

    [SerializeField] private GameObject playerPrefab;
    private float Y_BUFFER = 1.5f; // Player height is 1f. Since centertocell method gets the center of the tile, 0.5f is the height to the top of the tile.

    // Instantiates player prefab at specified location with a y-buffer of 5f.
    public void spawnPlayer(Vector3 spawnPos)
    {
        spawnPos.y += Y_BUFFER; // adds y_buffer to prevent player from spawning inside the terrain
        transform.position = spawnPos;
        GameObject Player = Instantiate(playerPrefab, this.transform);
    }
}
