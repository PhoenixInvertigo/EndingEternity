//This manages the gamestate itself, controlling what the player can do in any given state

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public enum GameState { MAIN, PLAY, PAUSE, CUTSCENE };
    public GameState currentGameState = new GameState();
    public List<SavedDroppableList> SavedLists = new List<SavedDroppableList>();
    public delegate void SaveDelegate(object sender, EventArgs args);
    public static event SaveDelegate SaveEvent;

    public static GameManager Instance;


    //Copy of our player if we ever need it game-wide
    public GameObject playerCopy;
    /*
	 Okay, so, notes from the tutorial we took this thing from first (slightly modified):
	 "Transition target is set by ExitTransitionScript component when collided with.
	 This enables us to spawn the player at a spawn point when the next scene is loaded.
	 To use this, go to the destination scene, make an empty GameObject and position it
	 where the player is supposed to spawn, and use Copy on the transform component.
	 Go to the source scene, make an empty GameObject, and paste component values. 
	 This will position the game object at some arbitrary position. Assign this game object as
	 transition target in the ExitTransitionScript. Player will move to that location when next scene is loaded."

	Now then! We can use that with our spawn point idea, which we're planning to use for instantiating custom enemies based
	on world events throughout the eras anyway. Can probably combine the two and make spawn points a really useful component of things.
	 */
    public Transform TransitionTarget;

    public PlayerStatistics LocalCopyOfData = new PlayerStatistics();
    public PlayerStatistics savedPlayerData = new PlayerStatistics();
    public bool IsSceneBeingLoaded = false;
    public bool IsSceneBeingTransitioned = false;
    public bool IsFirstTimeBeGentle = false;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        if (TransitionTarget == null)
        {
            TransitionTarget = gameObject.transform;
        }

    }

    void Start()
    {
        //currentGameState = GameState.MAIN;
        //This will set to MAIN when we have it ready to load a main menu. See above. Right now, it's gonna PLAY for testing.
        currentGameState = GameState.PLAY;
        //Application.targetFrameRate = 60;
    }


    void Update()
    {

    }

    public void InitializeScenelist()
    {
        if (SavedLists == null)
        {
            Debug.Log("Saved lists was null");
            SavedLists = new List<SavedDroppableList>();
        }

        bool found = false;

        for (int i = 0; i < SavedLists.Count; i++)
        {
            if (SavedLists[i].SceneID == SceneManager.GetActiveScene().buildIndex)
            {
                found = true;
                Debug.Log("Scene was found in saved lists!");
            }
        }

        if (!found)
        {
            SavedDroppableList newList = new SavedDroppableList(SceneManager.GetActiveScene().buildIndex);
            SavedLists.Add(newList);

            Debug.Log("Created new list!");
        }
    }

    public SavedDroppableList GetListForScene()
    {
        for (int i = 0; i < SavedLists.Count; i++)
        {
            if (SavedLists[i].SceneID == SceneManager.GetActiveScene().buildIndex)
                return SavedLists[i];
        }

        Debug.Log("Total list count: " + SavedLists.Count.ToString() + ", not found index: " + SceneManager.GetActiveScene().buildIndex.ToString());
        return null;
    }

    //This will reset the enemy list for a level. Make sure levelNumber is the build index
    public void ClearListForLevel(int levelNumber)
    {
        for (int i = 0; i < SavedLists.Count; i++)
        {
            if (SavedLists[i].SceneID == levelNumber)
            {
                SavedLists[i].SavedEnemies.Clear();
            }
        }
    }

    public void FireSaveEvent()
    {
        GetListForScene().SavedEnemies = new List<SavedDroppableEnemy>();
        //If we have any functions in the event:
        if (SaveEvent != null)
        {
            SaveEvent(null, null);
        }
    }

    public void SaveData()
    {
        if (!Directory.Exists("Saves"))
            Directory.CreateDirectory("Saves");

        FireSaveEvent();

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create("Saves/save.binary");
        FileStream saveEnemies = File.Create("Saves/saveEnemies.binary");

        LocalCopyOfData = Player.Instance.localPlayerData;

        formatter.Serialize(saveFile, LocalCopyOfData);
        formatter.Serialize(saveEnemies, SavedLists);

        saveFile.Close();
        saveEnemies.Close();

        Debug.Log("Saved!");
    }

    public void LoadData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open("Saves/save.binary", FileMode.Open);
        FileStream saveEnemies = File.Open("Saves/saveEnemies.binary", FileMode.Open);

        LocalCopyOfData = (PlayerStatistics)formatter.Deserialize(saveFile);
        SavedLists = (List<SavedDroppableList>)formatter.Deserialize(saveEnemies);

        saveFile.Close();
        saveEnemies.Close();

        Debug.Log("Loaded!");
    }
}
