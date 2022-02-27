using UnityEngine;

/**

TODO

    1. Change collision detection from ray to circle
        NOTE: Physics.OverlapCircle triggers a collision from its own collider. Unable
        Unable to find a fix to allow it to ignore itself but collide with everything else.
    2. Add meteorite sprite
    3. Add meteorite animations
    4. Add meteorite particles (impact, flying, etc)
    5. Add raycast to detect impact point based on current velocity. 
        5a. Use raycast to instantiate sometype of marker at impact point to indicate danger to the player

*/

public class MeteoriteController : MonoBehaviour
{
    private float areaOfImpact = 5f;
    [SerializeField] private LayerMask targetLayer;
    private Rigidbody2D rb2d;
    [SerializeField] private AudioSource explosionSoundEffect;

    private enum meteoriteState {falling, exploding}
    private meteoriteState state;
    private Animator meteoriteAnimator;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        meteoriteAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        updateAnimationState();
        if(rb2d.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void explode()
    {
        // Simulates kill area by generating a sphere and getting any objects that are inside
        Collider2D[] objectsHit = Physics2D.OverlapCircleAll(transform.position, areaOfImpact, targetLayer);

        // Checks every object inside the 'kill sphere.' Could be redundant as the player is the only object with the Player layer.
        foreach(Collider2D obj in objectsHit)
        {
            if(obj.tag == "Player")
            {
                PlayerHealthController playerReference = obj.GetComponent<PlayerHealthController>();
                if(playerReference.getHealth() > 0)
                    playerReference.damagePlayer(75);
                else  
                    return;
                CinemachineCameraShake.Instance.shakeCamera(2f, 2f);
            } 
        }
    }

    private void updateAnimationState()
    {
        if(rb2d.velocity != Vector2.zero)
            state = meteoriteState.falling;
    }

    void OnCollisionEnter2D()
    {
        state = meteoriteState.exploding;
        explosionSoundEffect.Play();
        explode();
        Destroy(this.gameObject, .2f);
    }
}
