using UnityEngine;

public class DinosaurSpawner : MonoBehaviour
{

    /*

    NOTE: Dinosaurs will spawn even when the player has not "entered" the grass terrain.
          Because of how the generation works, the player could be 100 blocks behind the current
          terrain.
    FIX:  Add a check for what rule tile the player is currently on. Only spawn dinosaurs if the
          RuleTile matches the grass terrain.

    */

    public GameObject dinosaurPrefab;

    private GameObject player;
    private float time = 3f;
    
    private TimePeriods timePeriods;

    void Start()
    {
        timePeriods = new TimePeriods();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(!player)
            player = GameObject.FindWithTag("Player");
        
        //Start spawning dinos if the time period is Grass Terrain
        //Based on the position of the player
        if(player && timePeriods.getTimePeriod(player.transform.position.x) == "Grass Terrain")
        {
            dinosaurTimer();
        }
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
