using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{

    public float waitForSpawn = 1;
    public GameObject prefab;

    //So, you're going to put these on spawnpoint anchors in the level
    //When it's the first time a level's been loaded, these will fire off and make the prefab attached to them via the editor
    //Otherwise, it's going to load the data from before

    //There always needs to be an enemy in each level called like "WhatIsDeadCanNeverDie" that isn't killable, to act as a placeholder
    //which keeps the list from being empty and resetting the level when everything is killed

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (waitForSpawn > 0)
        {
            waitForSpawn -= Time.deltaTime;
        }
        if (waitForSpawn < 0)
        {
            waitForSpawn = 0;
        }
        if (waitForSpawn == 0)
        {
            if (WorldManager.Instance.loadEnemyState == 1)
            {
                Spawn();
            }
            Destroy(gameObject);
            //this.gameObject.SetActive(false);
        }
    }

    void Spawn()
    {
        Instantiate(prefab);
    }
}

