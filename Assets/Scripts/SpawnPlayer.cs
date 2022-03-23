using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{

    [SerializeField] private GameObject playerPrefab;

    public void spawnPlayer(Vector2 spawnPos)
    {
        spawnPos = new Vector2(spawnPos.x + 5, spawnPos.y + 5);
        transform.position = spawnPos;
        GameObject Player = Instantiate(playerPrefab, this.transform);
    }
}
