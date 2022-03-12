using UnityEngine;

/**

TODO

    1. Configure spawner object to follow player.
    2. Configure spawner to instantiate at a random location above the player, but within the bounds of the screen.
    3. Configure spawner to give each meteorite a random velocity (so that they fly at an angle)..

*/

public class MeteoriteSpawner : MonoBehaviour
{
    public GameObject meteoritePrefab;

    private GameObject player;
    private float time = 2f;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(!player)
            player = GameObject.FindWithTag("Player");
        
        if(GameController.currentAge == "Volcanic Terrain"){
            meteoriteTimer();
        }
    }

    private void meteoriteTimer()
    {
        // // Spawns a meteorite every 2 seconds
        if(time > 0f)
            time -= Time.deltaTime;
        else
        {
            time = 2f;
            //spawnMeteorite();
        }
    }

    private void spawnMeteorite()
    {
        GameObject temporaryMeteorite = Instantiate(meteoritePrefab, player.transform.position + new Vector3(8, 8, 0), player.transform.rotation) as GameObject;
        temporaryMeteorite.GetComponent<MeteoriteController>().addSpeed(new Vector2(Random.Range(-5f, 5f), Random.Range(-1f, -5f)));
    }
}
