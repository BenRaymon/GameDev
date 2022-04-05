using UnityEngine;

public class BoundsManager : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D hit)
    {
		GameObject objectHit = hit?.gameObject;
        
		// If the player enters the bounds colliders, kill them
		if(objectHit.tag == "Player")
		{
			PlayerHealthController playerReference = objectHit.GetComponent<PlayerHealthController>();
            if(playerReference.getHealth() > 0)
                playerReference.damagePlayer(100);
            else  
                return;
		}
    }
}
