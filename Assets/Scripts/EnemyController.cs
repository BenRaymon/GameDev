using UnityEngine;

/**

TODO
    1. Add function to attack player
        1. Collision detection
    2. Create and add enemy animation
    3. Improve enemy movement
        NOTE: using velocity is too slow, addforce can sometimes lead to enemies shooting off into the distance

*/
public class EnemyController : MonoBehaviour
{
    public int enemyHealth = 100;
    public float chaseDistance;
    public float enemySpeed;

    public GameObject player;
    private Transform playerLocation;
    private Rigidbody2D rb2d;
    private SpriteRenderer enemySprite;
    private Animator enemyAnimator;
    private enum enemyState {idle, run, attack}
    private enemyState state;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();
        playerLocation = player.GetComponent<Transform>();
        enemyAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 playerDistance = new Vector2(playerLocation.position.x - transform.position.x, 0f);

        if(Vector2.Distance(transform.position, playerLocation.position) < chaseDistance)
            rb2d.AddForce(playerDistance);
        
        if(rb2d.velocity.x > .1f)
            enemySprite.flipX = false;
        else if(rb2d.velocity.x < -.1f)
            enemySprite.flipX = true;

        updateEnemyState();
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
}
