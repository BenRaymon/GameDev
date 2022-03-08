using UnityEngine;

public class DinosaurSpawner : MonoBehaviour
{
    public GameObject dinosaurPrefab;

    private GameObject player;
    private float time = 3f;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.currentAge == "Grass Platform")
            dinosaurTimer();
    }

    private void dinosaurTimer()
    {
        if(time > 0f)
            time -= Time.deltaTime;
        else
        {
            time = 3f;
            spawnDinosaur();
        }
    }

    private void spawnDinosaur()
    {
        GameObject temporaryDinosaur = Instantiate(dinosaurPrefab, player.transform.position - new Vector3(8, 0, 0), player.transform.rotation) as GameObject;
    }
}
