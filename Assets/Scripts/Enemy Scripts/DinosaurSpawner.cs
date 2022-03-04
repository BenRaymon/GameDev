using UnityEngine;

public class DinosaurSpawner : MonoBehaviour
{
    public GameObject dinosaurPrefab;

    private GameObject player;

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindWithTag("Player");

        if(Input.GetKeyDown(KeyCode.F))
        {
            spawnDinosaur();
        }
    }

    private void spawnDinosaur()
    {
        GameObject temporaryDinosaur = Instantiate(dinosaurPrefab, player.transform.position - new Vector3(15, 0, 0), player.transform.rotation) as GameObject;
    }
}
