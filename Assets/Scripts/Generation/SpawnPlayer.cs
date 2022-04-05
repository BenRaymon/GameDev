using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{

    [SerializeField] private GameObject playerPrefab;

    // Instantiates player prefab at specified location with a y-buffer of 5f.
    public void spawnPlayer(Vector2 spawnPos)
    {
        spawnPos = new Vector2(spawnPos.x, spawnPos.y + 5f);
        transform.position = spawnPos;
        GameObject Player = Instantiate(playerPrefab, this.transform);
    }
}
