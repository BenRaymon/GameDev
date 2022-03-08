using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{

    [SerializeField] private GameObject terrainSpawner;
    [SerializeField] private GameObject playerPrefab;

    void Start()
    {
        Invoke("spawnPlayer", 1f);
    }

    private void spawnPlayer()
    {
        Vector2 spawnPos = terrainSpawner.GetComponent<ProceduralGeneration>().findLocation();
        spawnPos = new Vector2(spawnPos.x + 5, spawnPos.y + 5);
        transform.position = spawnPos;
        GameObject Player = Instantiate(playerPrefab, this.transform);
    }
}
