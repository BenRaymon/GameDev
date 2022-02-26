using UnityEngine;

/**

TODO
    1. Create and add enemy attack animation
    2. Improve enemy movement
        NOTE: using velocity is too slow, addforce can sometimes lead to enemies shooting off into the distance
    3. Find a way to get prefabbed dinosaurs to spawn with player object set

*/
public class EnemyController : MonoBehaviour
{
    public int enemyHealth = 100;
    public float chaseDistance;
    public float enemySpeed;

    public GameObject player;
    private Transform playerLocation;
    private Rigidbody2D rb2d;
    private Animator enemyAnimator;
    private enum enemyState {idle, run, attack}
    private enemyState state;
    private bool facingRight = true;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerLocation = player.GetComponent<Transform>();
        enemyAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 playerDistance = new Vector2(playerLocation.position.x - transform.position.x, 0f);

        if(Vector2.Distance(transform.position, playerLocation.position) < chaseDistance)
            rb2d.AddForce(playerDistance);
        
        if(rb2d.velocity.x > .1f && !facingRight)   
            flip();
        else if(rb2d.velocity.x < -.1f && facingRight)
            flip();

        updateEnemyState();
    }

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

    void OnCollisionStay2D(Collision2D col)
    {
        GameObject objectHit = col.gameObject;
        if(objectHit.tag == "Player")
        {
            objectHit.GetComponent<PlayerController>().damagePlayer(1);
        }
    }
}
