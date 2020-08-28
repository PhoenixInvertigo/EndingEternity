using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunMineScript : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col)
    {
        Enemy myEnemyScript = col.GetComponent<Enemy>();
        Debug.Log("StunMine Triggered");
        if(myEnemyScript != null)
        {
            myEnemyScript.stunTimer = 1.5f;
            Deactivate();
        }
    }

    public void Deactivate()
    {
        Destroy(gameObject);
        //this.gameObject.SetActive(false);
        Debug.Log("StunMine deactivated");
    }
 

}
