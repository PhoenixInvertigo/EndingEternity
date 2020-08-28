using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour
{



    public Transform targetLoc;
    public float OutX, OutY;


    // Use this for initialization
    void Start()
    {
        OutX = targetLoc.position.x;
        OutY = targetLoc.position.y;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Player collided with portal");
        Player.Instance.transform.position = new Vector2(OutX, OutY);

    }
}

