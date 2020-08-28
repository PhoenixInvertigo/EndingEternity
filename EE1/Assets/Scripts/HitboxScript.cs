using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxScript : MonoBehaviour
{
    public int framesTilGone = -1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(framesTilGone > 0)
        {
            framesTilGone--;
        }

        if(framesTilGone == 0)
        {
            Destroy(gameObject);
            //this.gameObject.SetActive(false);
            Debug.Log("Hitbox Despawned");
        }
    }

   
}
