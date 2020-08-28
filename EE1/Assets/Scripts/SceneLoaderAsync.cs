using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoaderAsync : MonoBehaviour
{

    public static SceneLoaderAsync Instance;
    private float _loadingProgress;
    public float LoadingProgress { get { return _loadingProgress; } }


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

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadScene(int sceneToLoad)
    {
        //kick off the one co-routine to rule them all
        Debug.Log("Starting LoadScene");
        StartCoroutine(LoadScenesInOrder(sceneToLoad));
    }

    private IEnumerator LoadScenesInOrder(int upcomingScene)
    {
        //LoadSceneAsync() returns an AsyncOperation
        //So it only continues past this point when the operation has finished
        //This needs to be the loader
        Debug.Log("Starting Loading Screen Load");
        yield return SceneManager.LoadSceneAsync(0);

        //as soon as we've finished loading the loading screen, start loading the game
        //Scene we're going to
        Debug.Log("Starting Loaded Scene Load");
        yield return StartCoroutine(LoadNewScene(upcomingScene));
    }

    private IEnumerator LoadNewScene(int sceneNumber)
    {
        Debug.Log("Firing off scene " + sceneNumber as string);
        var asyncScene = SceneManager.LoadSceneAsync(sceneNumber);
        bool pressToContinue = false;

        //this value stops the scene form displaying when it's finished loading
        asyncScene.allowSceneActivation = false;

        while (!asyncScene.isDone)
        {
            //loading bar progress
            _loadingProgress = Mathf.Clamp01(asyncScene.progress / 0.9f) * 100.0f;

            //scene has loaded as much as possible. Last 10% can't be done async
            if (asyncScene.progress >= 0.9f)
            {
                //we finally show the scene
                //slapped in an extra check to allow for loading to work right
                pressToContinue = true;
                //asyncScene.allowSceneActivation = true;
                Debug.Log("Level is ready! Press A to continue!");
            }

            if (pressToContinue == true)
            {
                if (Input.GetButtonDown("A"))
                {
                    asyncScene.allowSceneActivation = true;
                }
            }

            yield return null;

        }
    }
}

