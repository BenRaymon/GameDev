using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinosaurSpawner : MonoBehaviour
{
    public GameObject dinosaurPrefab;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            spawnDinosaur();
        }
    }

    private void spawnDinosaur()
    {
        GameObject temporaryDinosaur = Instantiate(dinosaurPrefab) as GameObject;
    }
}
