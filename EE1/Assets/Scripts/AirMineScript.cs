using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMineScript : MonoBehaviour
{

    public float airDmg = 0;
    public GameObject blueHitbox;

    public void Detonate()
    {
        if (Player.Instance.showHitboxen == true)
        {
            Debug.Log("Beginning the AirMine Hitboxen");
            GameObject myHitbox1 = (GameObject)Instantiate(blueHitbox);
            GameObject myHitbox2 = (GameObject)Instantiate(blueHitbox);
            GameObject myHitbox3 = (GameObject)Instantiate(blueHitbox);
            HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
            HitboxScript myHitboxScript2 = myHitbox2.GetComponent<HitboxScript>();
            HitboxScript myHitboxScript3 = myHitbox3.GetComponent<HitboxScript>();
            myHitbox1.transform.localScale = new Vector3(3f, 3f, 0);
            myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
            myHitboxScript1.framesTilGone = 30;
            myHitbox2.transform.localScale = new Vector3(2f, 4f, 0);
            myHitbox2.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
            myHitboxScript2.framesTilGone = 30;
            myHitbox3.transform.localScale = new Vector3(4f, 2f, 0);
            myHitbox3.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
            myHitboxScript3.framesTilGone = 30;
            Debug.Log("AirMine Hitboxen should be visually placed");
        }

        Debug.Log("Beginning AirMine Triggerhits");
        //hitbox 1's triggerhit
        Entity.TriggerHits(this.transform.position.x - .48f, this.transform.position.x + .48f, this.transform.position.y + .48f, this.transform.position.y - .48f, airDmg, "Air", 100, "Directional");
        //hitbox 2's triggerhit (vertical small)
        Entity.TriggerHits(this.transform.position.x - .32f, this.transform.position.x + .32f, this.transform.position.y + .64f, this.transform.position.y - .64f, airDmg, "Air", 50, "Directional");
        //hitbox 3's triggerhit (horizontal small)
        Entity.TriggerHits(this.transform.position.x - .64f, this.transform.position.x + .64f, this.transform.position.y + .32f, this.transform.position.y - .32f, airDmg, "Air", 50, "Directional");
        Debug.Log("AirMine's Triggerhits should be complete");

        Destroy(gameObject);
        //this.gameObject.SetActive(false);
        Debug.Log("Mine Detonated");
    }
}

