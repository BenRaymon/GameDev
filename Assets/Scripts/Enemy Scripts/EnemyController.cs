using UnityEngine;

/**

TODO
    1. Create and add enemy attack animation
    2. Improve enemy movement
        NOTE: using velocity is too slow, addforce can sometimes lead to enemies shooting off into the distance
    3. Add some sort of pathing that prevents enemies from running off a cliff.
*/
public class EnemyController : MonoBehaviour
{
    public int enemyHealth = 100;
    private float chaseDistance = 10f;
    public float enemySpeed;

    private GameObject player;
    private Transform playerLocation;
    private Rigidbody2D rb2d;
    private Animator enemyAnimator;
    private enum enemyState {idle, run, attack}
    private enemyState state;
    private bool facingRight = true;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb2d = GetComponent<Rigidbody2D>();
        playerLocation = player.GetComponent<Transform>();
        enemyAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // Calculates distance to player
        Vector2 playerDistance = new Vector2(playerLocation.position.x - transform.position.x, 0f);

        // Moves enemy only if the distance to player is less than the pre-set chase distance.
        if(Vector2.Distance(transform.position, playerLocation.position) < chaseDistance)
            rb2d.AddForce(playerDistance);
        
        if(rb2d.velocity.x > .1f && !facingRight)   
            flip();
        else if(rb2d.velocity.x < -.1f && facingRight)
            flip();

        updateEnemyState();
    }

    void FixedUpdate()
    {
        
    }

    // Flips enemy using localscale instead of sprite.flipx so that the circle collider maintains its position
    private void flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;

        transform.localScale = currentScale;
        facingRight = !facingRight;
    }

    private void updateEnemyState()
    {
        if(rb2d.velocity.x > .1f || rb2d.velocity.x < -.1f)
        {
            state = enemyState.run;
        }
        else if(Mathf.Approximately(rb2d.velocity.x, 0f) && Mathf.Approximately(rb2d.velocity.y, 0f))
        {
            state = enemyState.idle;
        }

        enemyAnimator.SetInteger("state", (int)state);
    }

    public void takeDamage(int damage)
    {
        enemyHealth -= damage;
        if(enemyHealth <= 0)
            Destroy(this.gameObject);
    }

    // Runs when a collision is detected and continues until the collision stops.
    void OnTriggerStay2D(Collider2D col)
    {
        GameObject objectHit = col?.gameObject;
        if(objectHit.tag == "Player")
        {
            PlayerHealthController playerReference = objectHit.GetComponent<PlayerHealthController>();
            if(playerReference.getHealth() > 0)
                playerReference.damagePlayer(1);
            else  
                return;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
