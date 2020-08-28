using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//Put this on a trigger collider that you want to exit level on

public class ExitTransitionScript : MonoBehaviour
{

    //level we're going to
    public int TargetedSceneIndex;
    //location in new level to load to
    public Transform TargetPlayerLocation;

    void CheckForFirst(int whichScene)
    {
        //do stuff to check if this is first time going to new level
        //if yes, fire off a call to GameManager.Instance.IsFirstTimeBeGentle to set it to true
    }

    public void OnTriggerEnter2D()
    {
        GameManager.Instance.TransitionTarget.position = TargetPlayerLocation.position;

        GameManager.Instance.IsSceneBeingTransitioned = true;
        GameManager.Instance.FireSaveEvent();

        CheckForFirst(TargetedSceneIndex);

        SceneLoaderAsync.Instance.LoadScene(TargetedSceneIndex);
        //SceneManager.LoadScene (TargetedSceneIndex);
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
