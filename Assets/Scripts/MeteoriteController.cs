using UnityEngine;

/**

TODO

    1. Change collision detection from ray to circle
        NOTE: Physics.OverlapCircle triggers a collision from its own collider. Unable
        Unable to find a fix to allow it to ignore itself but collide with everything else.
    2. Add meteorite sprite
    3. Add meteorite animations
    4. Add meteorite particles (impact, flying, etc)

*/

public class MeteoriteController : MonoBehaviour
{
    private float areaOfImpact = 3f;
    private float extraHeightTest = 0.02f;
    [SerializeField] private LayerMask targetLayer;
    private CircleCollider2D meteoriteCollider;
    private Rigidbody2D rb2d;

    void Start()
    {
        meteoriteCollider = GetComponent<CircleCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(meteoriteCollisionDetector())
        {
            explode();
            Destroy(this.gameObject);
        }
    }

    private void explode()
    {
        // Kill area
        Collider2D[] objectsHit = Physics2D.OverlapCircleAll(transform.position, areaOfImpact, targetLayer);
        foreach(Collider2D obj in objectsHit)
        {
            Debug.Log("Player damaged " + obj.name);
            CinemachineCameraShake.Instance.shakeCamera(10f, 2f);
            obj.GetComponent<PlayerController>().damagePlayer(1);
        }
    }

    private bool meteoriteCollisionDetector()
    {
        RaycastHit2D rayCast = Physics2D.Raycast(meteoriteCollider.bounds.center, Vector2.down, meteoriteCollider.bounds.extents.y + extraHeightTest);
        
        return rayCast;
    }
}
