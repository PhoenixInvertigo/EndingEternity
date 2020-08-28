using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    //GameObject orange and yellow boxen & rigidbody
    public GameObject redHitbox;
    public GameObject orangeBox;
    public GameObject yellowBox;
    public Rigidbody2D myRigid;

    //Variable to check whether Fireball or Ember
    public string isWhat = "None";
    public float damage = 0f;
    public string direction = "None";

    public bool directionSet = false;
    bool beginClock = false;
    bool readyToMove = false;

    int frameDuration = 300;
    float speed = 7.5f;
    public Vector3 speedDir = Vector3.zero;



    //Fix box layout based on direction
    void FixBoxen()
    {
        float offset = .12f;

        switch (direction)
        {
            case "Up":
                orangeBox.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - offset, 0);
                yellowBox.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 2 * offset, 0);
                speedDir = Vector3.up;
                break;
            case "Down":
                orangeBox.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + offset, 0);
                yellowBox.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2 * offset, 0);
                speedDir = Vector3.down;
                break;
            case "Left":
                orangeBox.transform.position = new Vector3(this.transform.position.x + offset, this.transform.position.y, 0);
                yellowBox.transform.position = new Vector3(this.transform.position.x + 2* offset, this.transform.position.y, 0);
                speedDir = Vector3.left;
                break;
            case "Right":
                orangeBox.transform.position = new Vector3(this.transform.position.x - offset, this.transform.position.y, 0);
                yellowBox.transform.position = new Vector3(this.transform.position.x - 2 * offset, this.transform.position.y, 0);
                speedDir = Vector3.right;
                break;
            default:
                break;
        }
    }

    void Move()
    {
        myRigid.MovePosition(transform.position + speedDir * speed * Time.deltaTime);       
    }

    private void FixedUpdate()
    {
        if (readyToMove == true)
        {
            Move();
        }
    }

    //Update: When frames = 0, despawn
    private void Update()
    {
        //Make sure to receive Damage, Direction, and Type before setting this to true
        if(directionSet == true)
        {
            FixBoxen();
            directionSet = false;
            beginClock = true;
            readyToMove = true;
            Debug.Log("Ready to Move");
        }

        if (beginClock == true)
        {
            frameDuration--;
        }

        if(frameDuration == 0)
        {
            Despawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        if(isWhat == "Fireball")
        {
            if(Player.Instance.showHitboxen == true)
            {
                Debug.Log("Beginning Fireball Hitbox");
                GameObject myHitbox = (GameObject)Instantiate(redHitbox);
                HitboxScript myHitboxScript = myHitbox.GetComponent<HitboxScript>();
                myHitbox.transform.localScale = new Vector3(1.5f, 1.5f, 0);
                myHitbox.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                myHitboxScript.framesTilGone = 60;
            }

            Entity.TriggerHitsBuff(this.transform.position.x - .24f, this.transform.position.x + .24f, this.transform.position.y + .24f, this.transform.position.y - .24f, damage, "Fire", "Fireball", 10f, .1f * damage, 50, direction);
            Despawn();
        }

        if(isWhat == "Ember")
        {
            if (Player.Instance.showHitboxen == true)
            {
                Debug.Log("Beginning Ember Hitbox");
                GameObject myHitbox = (GameObject)Instantiate(redHitbox);
                HitboxScript myHitboxScript = myHitbox.GetComponent<HitboxScript>();
                myHitbox.transform.localScale = new Vector3(1f, 1f, 0);
                myHitbox.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                myHitboxScript.framesTilGone = 60;
            }

            Entity.TriggerHits(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y + .16f, this.transform.position.y - .16f, damage, "Fire", 25, direction);
            Despawn();
        }
    }

    //Despawn function
    void Despawn()
    {
        Destroy(gameObject);
        //this.gameObject.SetActive(false);
    }
}
