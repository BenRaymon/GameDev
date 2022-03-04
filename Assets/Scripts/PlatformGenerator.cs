using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject thePlatform;
    public Transform generationPoint;
    public float distanceBetween;

    private float platformWidth;
    private float groundHeight = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        platformWidth = thePlatform.GetComponent<BoxCollider2D>().size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < generationPoint.position.x){
            transform.position = new Vector2(transform.position.x + platformWidth + distanceBetween, groundHeight);
            Instantiate(thePlatform, transform.position, transform.rotation);
        }
    }
}
