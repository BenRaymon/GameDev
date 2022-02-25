using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteController : MonoBehaviour
{
    private float areaOfImpact = 3f;
    private float areaofEffect = 6f;
    [SerializeField] private LayerMask targetLayer;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            explode();
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

        // Effect area
        Collider2D[] objectsImpacted = Physics2D.OverlapCircleAll(transform.position, areaofEffect, targetLayer);
        foreach(Collider2D obj in objectsImpacted)
        {
            Debug.Log("Player damaged " + obj.name);
            CinemachineCameraShake.Instance.shakeCamera(5f, 1f);
            obj.GetComponent<PlayerController>().damagePlayer(1);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaOfImpact);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, areaofEffect);
    }
}
