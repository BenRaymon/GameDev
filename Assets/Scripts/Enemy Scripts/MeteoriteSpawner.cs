using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        //Start spawning meteors if the time period is Volcanic Terrain
        //Based on the position of the player
        if(player && timePeriods.getTimePeriod(player.transform.position.x) == "Volcanic Terrain" && SceneManager.GetActiveScene().name == "Level01"){
            meteoriteTimer();
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            spawnPlayerMeteorite();
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
            spawnMeteorite();
        }
    }

    private void spawnMeteorite()
    {
        GameObject temporaryMeteorite = Instantiate(meteoritePrefab, player.transform.position + new Vector3(8, 8, 0), player.transform.rotation) as GameObject;
        temporaryMeteorite.GetComponent<MeteoriteController>().addSpeed(new Vector2(Random.Range(-5f, 5f), Random.Range(-1f, -5f)));
    }

    public void spawnPlayerMeteorite()
    {
        // if facing left VELOCITY is negative, else VELOCITY positive
        string direction = player.GetComponent<ImprovedMovement>().getFacingDirection();
        float multiplier = direction == "left" ? -1f : 1f;

        GameObject temporaryMeteorite = Instantiate(meteoritePrefab, player.transform.position + new Vector3(2 * multiplier, 0, 0), player.transform.rotation) as GameObject;
        temporaryMeteorite.GetComponent<MeteoriteController>().addSpeed(new Vector2(5f * multiplier, 0f));
        temporaryMeteorite.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
    }
}
