using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{

    [SerializeField] private GameObject terrainSpawner;
    [SerializeField] private GameObject playerPrefab;

    void Start()
    {
        Invoke("spawnPlayer", .5f);
    }

    private void spawnPlayer()
    {
        Vector2 spawnPos = terrainSpawner.GetComponent<ProceduralGeneration>().findLocation();
        spawnPos = new Vector2(spawnPos.x + 3, spawnPos.y + 8);
        transform.position = spawnPos;
        GameObject Player = Instantiate(playerPrefab, this.transform);
    }
}
