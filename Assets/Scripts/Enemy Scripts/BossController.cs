using UnityEngine;

public class BossController : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D bossBody;
    
    // Animation setup
    private Animator bossAnimator;
    private enum bossState {idle, walk, attack, charge, kick, stun, death}
    private bossState state;

    private int bossHealth = 1000;
    private float CHASE_DISTANCE = 30f;
    private bool facingRight = true;
    private bool canChasePlayer;

    private Vector2 playerDistance;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bossBody = GetComponent<Rigidbody2D>();
        bossAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // Calculates distance to player
        playerDistance = new Vector2(player.transform.position.x - transform.position.x, 0f);
        if((Vector2.Distance(transform.position, player.transform.position) < CHASE_DISTANCE) && playerDistance.normalized != Vector2.zero)
        {
            canChasePlayer = true;
        }
        else
        {
            canChasePlayer = false;
        }

        if(bossBody.velocity.x > .1f && !facingRight)   
            flip();
        else if(bossBody.velocity.x < -.1f && facingRight)
            flip();
        
        updateBossState();
    }

    void FixedUpdate()
    {
        if(canChasePlayer)
        {
            bossBody.AddForce(10f*playerDistance.normalized, ForceMode2D.Impulse);
        }
    }

    private void flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;

        transform.localScale = currentScale;
        facingRight = !facingRight;
    }

    void updateBossState()
    {
        if(bossBody.velocity.x > .1f || bossBody.velocity.x < -.1f)
        {
            state = bossState.walk;
        }
        else if(Mathf.Approximately(bossBody.velocity.x, 0f) && Mathf.Approximately(bossBody.velocity.y, 0f))
        {
            state = bossState.idle;
        }

        bossAnimator.SetInteger("state", (int)state);
    }
}
