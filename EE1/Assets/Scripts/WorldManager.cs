
//This manages all savable events and states of the World
//LevelMaster in the Saves tutorial

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class WorldManager : MonoBehaviour
{

    //This is likely going to need to be much more complex later
    //Don't forget to add prefabs to this in the editor
    //Also, this script needs to have an object in each level
    public int loadEnemyState = 0;
    public static WorldManager Instance;
    public float voidPool = 0f;

    public GameObject EnemyPrefab0;
    //Make sure that each prefab's enemyID and case in here match so that they're loading the right thing

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        GameManager.Instance.InitializeScenelist();

        if (true)
        // if (GameManager.Instance.IsSceneBeingLoaded || GameManager.Instance.IsSceneBeingTransitioned)
        {
            SavedDroppableList localList = null;

            localList = GameManager.Instance.GetListForScene();


            if (localList != null)
            {
                Debug.Log("Saved enemy count: " + localList.SavedEnemies.Count);

                if (localList.SavedEnemies.Count == 0)
                {
                    loadEnemyState = 1;
                }

                for (int i = 0; i < localList.SavedEnemies.Count; i++)
                {
                    //run logic here that pulls enemyIDs to decide which prefab to load
                    if (GetEnemyPrefab(localList.SavedEnemies[i].enemyID) != null)
                    {
                        GameObject spawnedEnemy = (GameObject)Instantiate(GetEnemyPrefab(localList.SavedEnemies[i].enemyID));
                        spawnedEnemy.transform.position = new Vector2(localList.SavedEnemies[i].positionX, localList.SavedEnemies[i].positionY);
                    }
                }
            }
            else
            {
                Debug.Log("Local list was null!");
            }
            Debug.Log("loadEnemyState is " + loadEnemyState as string);

        }

    }

    //Make a script that attaches to the WorldManager that has the various list setups of enemies for each individual level
    //Use a variable like FirstTimeBeGentle to override the list placement above
    //Load out the enemy list for FTBG from the subscript
    //Let it save after that and get loaded normally
    //Each time you reset an era, reset FTBG
    //Make sure the enemy list placement for FTBG is checking the event dependencies of previous eras for rare spawns etc.
    //You're doing great, and even if it's hard right now, remember how much more progress we made this time than any other time
    //You're not a failure. You're doing fucking fantastic, and this is farther in one week than you ever thought you'd be able to do again.
    //You've got this. <3

    GameObject GetEnemyPrefab(int enemyID)
    {
        switch (enemyID)
        {
            case 0:
                return EnemyPrefab0;
            default:
                Debug.Log("No enemy prefab found");
                return null;

        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddToVoidPool(float incVoidDamage)
    {
        if (incVoidDamage >= 0)
        {
            voidPool += incVoidDamage;
        }
    }

    public void RemoveFromVoidPool(float usedVoidDamage)
    {
        if(usedVoidDamage >= 0)
        {
            voidPool -= usedVoidDamage;
        }
    }
}
