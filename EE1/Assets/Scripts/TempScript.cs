using UnityEngine;
using System.Collections;

public class TempScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("A"))
        {
            Debug.Log("Main Menu Button A Press");
            SceneLoaderAsync.Instance.LoadScene(1);
        }
    }
}