using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthController : MonoBehaviour
{
    private int playerHealth = 100;
    private Animator playerAnimator;
    private Rigidbody2D rb2d;

    void Update()
    {
        checkHealth();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public int getHealth()
    {
        return playerHealth;
    }

    public void damagePlayer(int damage)
    {
        playerHealth -= damage;
    }

    private void checkHealth()
    {
        if(playerHealth <= 0)
        {
            death();
        }
    }

    private void death()
    {
        playerAnimator.SetTrigger("death");
        rb2d.bodyType = RigidbodyType2D.Static;
    }

    private void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
