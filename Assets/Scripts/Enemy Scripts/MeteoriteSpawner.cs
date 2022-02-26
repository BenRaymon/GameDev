using UnityEngine;

/**

TODO

    1. Configure spawner object to follow player.
    2. Configure spawner to instantiate at a random location above the player, but within the bounds of the screen.
    3. Configure spawner to give each meteorite a random velocity (so that they fly at an angle).
    4. Add raycast to detect impact point based on current velocity. 
        4a. Use raycast to instantiate sometype of marker at impact point to indicate danger to the player.

*/

public class MeteoriteSpawner : MonoBehaviour
{
    public GameObject meteoritePrefab;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            spawnMeteorite();
        }
    }

    private void spawnMeteorite()
    {
        GameObject temporaryMeteorite = Instantiate(meteoritePrefab) as GameObject;
    }
}
