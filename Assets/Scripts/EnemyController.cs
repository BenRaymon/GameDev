using UnityEngine;

/**

TODO
    1. Use raycast to detect player and then follow
    2. Add function to attack player
        2a. Collision detection
    3. Create enemy sprite and prefab
    4. Create enemy animation

*/
public class EnemyController : MonoBehaviour
{
    public int enemyHealth = 100;

    public void takeDamage(int damage)
    {
        enemyHealth -= damage;
        if(enemyHealth <= 0)
            Destroy(this.gameObject);
    }
}
