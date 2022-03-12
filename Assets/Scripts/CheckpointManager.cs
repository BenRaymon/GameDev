using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
	private int counter = -1;
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
			Debug.Log("removing chunk");
			proceduralGenerator.GetComponent<ProceduralGeneration>().removeChunk();
		}

		counter++;
    }
}
