using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
	private int counter = -1; // set to -1 so that the first platform on which the player hits the checkpoint is not deleted.
	[SerializeField] private GameObject proceduralGenerator;

	// Checks for any collisions
	void OnTriggerEnter2D(Collider2D hit)
    {
		// If the player enters the checkpoint collider, generate the next piece of map
		if(hit.name == "Player")
		{
			proceduralGenerator.GetComponent<ProceduralGeneration>().generateMap();
			counter++;
		}

		// Deletes previous platforms except for the first time the player hits the checkpoint collider.
		if(counter > 0)
		{
			proceduralGenerator.GetComponent<ProceduralGeneration>().removeChunk();
		}
    }
}
