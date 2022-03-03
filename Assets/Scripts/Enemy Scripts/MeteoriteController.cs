using UnityEngine;

/**

TODO

    1. Add meteorite particles (impact, flying, etc)
    2. Add raycast to detect impact point based on current velocity. 
        2a. Use raycast to instantiate sometype of marker at impact point to indicate danger to the player

*/

public class MeteoriteController : MonoBehaviour
{
    private float AREA_OF_IMPACT = 5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D rb2d;
    private Animator meteoriteAnimator;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        meteoriteAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if(rb2d.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        Debug.DrawRay(transform.position, rb2d.velocity, Color.red);
    }

    private void explode()
    {
        // Simulates kill area by generating a sphere and getting any objects that are inside
        Collider2D[] objectsHit = Physics2D.OverlapCircleAll(transform.position, AREA_OF_IMPACT, playerLayer);

        // Checks every object inside the 'kill sphere.' Could be redundant as the player is the only object with the Player layer.
        foreach(Collider2D obj in objectsHit)
        {
            if(obj.tag == "Player")
            {
                PlayerHealthController playerReference = obj.GetComponent<PlayerHealthController>();
                if(playerReference.getHealth() > 0)
                    playerReference.damagePlayer(100);
                else  
                    return;
                CinemachineCameraShake.Instance.shakeCamera(2f, 1f);
            } 
        }
    }

    private void destroyObject()
    {
        Destroy(this.gameObject);
    }

    public void addSpeed(Vector2 forceDirection)
    {
        rb2d.AddForce(forceDirection, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D()
    {
        rb2d.bodyType = RigidbodyType2D.Static;
        explode();
        meteoriteAnimator.SetTrigger("explode");
    }
}
