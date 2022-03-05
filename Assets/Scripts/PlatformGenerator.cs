using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    private GameObject thePlatform;
    public Transform generationPoint;
    public float distanceBetween;

    private float platformWidth;
    private float groundHeight = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        setTerrain("Volcanic Platform"); // initially sets the spawned platforms to volcanic
        platformWidth = thePlatform.GetComponent<BoxCollider2D>().size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < generationPoint.position.x){
            transform.position = new Vector2(transform.position.x + platformWidth + distanceBetween, groundHeight);
            GameObject temporaryPlatform = Instantiate(thePlatform, transform.position, transform.rotation) as GameObject; // Instantiates as gameobject for Destroy()
            Destroy(temporaryPlatform, 5f); // Destroys platform in ~5 seconds
        }
    }

    public void setTerrain(string terrain)
    {
        thePlatform = Resources.Load(terrain) as GameObject;
    }
}
