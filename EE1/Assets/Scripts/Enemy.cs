using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Enemy : Entity
{

    //public Entity eScript = null;
    protected float knockbackCoefficient = 1;
    protected float hitpoints = 50;
    protected float maxHP = 50;
    protected float vitality = 3;
    protected float spirit = 0;
    protected float maxSpirit = 0;
    protected float focus = 0;
    protected float speed = 0;
    protected float fireDmg = 0;
    protected float waterDmg = 0;
    protected float airDmg = 0;
    protected float earthDmg = 0;
    protected float lightDmg = 0;
    protected float voidDmg = 0;
    protected float fireRes = 0;
    protected float waterRes = 0;
    protected float airRes = 0;
    protected float earthRes = 0;
    protected float lightRes = 0;
    protected float voidRes = 0;
    protected int voidPower = 0;
    protected float aggroRange = 0;

    public string enemyName;
    public int enemyID;
    public Rigidbody2D myRB;

    public GameObject redHitbox;
    public GameObject orangeHitbox;
    public GameObject yellowHitbox;
    public GameObject greenHitbox;
    public GameObject blueHitbox;
    public GameObject indigoHitbox;
    public GameObject purpleHitbox;

    protected int curFrame = 0;
    protected int remainingFrames = 0;
    protected delegate void currentAction();
    protected currentAction curAction = null;
    //protected string curAction = "None";
    protected string direction = "Up";

    protected int xPixels = 0;
    protected int yPixels = 0;

    protected string[] buffs;
    protected float[] buffTimers;
    protected float[] buffMagnitudes;
    public int numBuffs = 5;

    protected float pulseTimer = 0f;
    public float stunTimer = 0f;

    protected bool playerInRange = false;
    protected bool rooted = false;

    void Awake()
    {
        buffs = new string[numBuffs];
        buffTimers = new float[numBuffs];
        buffMagnitudes = new float[numBuffs];

        for (int i = 0; i < numBuffs; i++)
        {
           // Debug.Log("Starting initialization of arrays. i = " + i);
            buffs[i] = "None";
            buffTimers[i] = 0f;
            buffMagnitudes[i] = 0f;
        }
    }

    void Start()
    {
        Debug.Log("This Enemy is adding itself to the SaveFunction: " + enemyName);
        GameManager.SaveEvent += SaveFunction;


    }

    private void OnEnable()
    {
        Entity.Pool.Add(this);
        
        //incBuff = null;
    }
    private void OnDisable()
    {
        Entity.Pool.Remove(this);
        Debug.Log("Removing self from Entity Pool");
    }

    //Disable this function when you're ready to start testing saving and level transitions
    void OnDestroy()
    {
        Debug.Log("This Enemy is removing itself from the SaveFunction: " + enemyName);
        GameManager.SaveEvent -= SaveFunction;
    }

    public void SaveFunction(object sender, EventArgs args)
    {
        //You're probably going to be super confused at what I was doing here by the time you get around to using this functionality
        //So here's the lowdown: You're going to make spawnpoint objects
        //When the level loads, it's going to run some logic to figure out which enemies should exist
        //It's then going to spawn them at the spawnpoints
        //That's the idea, at least. Figure it out, future me.

        SavedDroppableEnemy enemy = new SavedDroppableEnemy();
        //Set the constructor stats for the Enemy thingie
        enemy.positionX = transform.position.x;
        enemy.positionY = transform.position.y;
        enemy.enemyID = enemyID;

        GameManager.Instance.GetListForScene().SavedEnemies.Add(enemy);
    }

    void FixedUpdate()
    {

    }

    void Update()
    {
        //Checking if there's damage on the Entity Script
        /*if ((eScript.incDmg != 0) || (eScript.incBuff != null) || (eScript.incKnockback != 0))
        {
            CheckHits();
        }*/

        if (GameManager.Instance.currentGameState == GameManager.GameState.PLAY)
        {
            Regen();
            //Timers();
        }

        if (hitpoints <= 0)
        {
            OnDeath();
        }

        BuffManagement();
    }

    void Pulse()
    {
        VoidHealing();
    }

    protected void Regen()
    {
        if (hitpoints < maxHP)
        {
            hitpoints += Time.deltaTime * vitality;
        }

        if (spirit < maxSpirit)
        {
            spirit += Time.deltaTime * focus;
        }
    }

    protected void VoidHealing()
    {

        if ((WorldManager.Instance.voidPool > 0) && (hitpoints < maxHP) && (voidPower != 0))
        {
            if (WorldManager.Instance.voidPool > (maxHP - hitpoints) * 10 / voidPower)
                {
                    float healing = (maxHP - hitpoints) * 10 / voidPower;

                    WorldManager.Instance.RemoveFromVoidPool(healing);
                    hitpoints += healing * voidPower / 10;
                }
            else
                {
                    float healing = WorldManager.Instance.voidPool;

                    WorldManager.Instance.RemoveFromVoidPool(healing);
                    hitpoints += healing * voidPower / 10;
                }
        }
    }

    protected void BuffManagement()
    {
        for(int i = 0; i < numBuffs; i++)
        {
            if ((buffs[i] != "None") && (buffTimers[i] == 0))
            {
                UnloadBuff(buffs[i], i);
            }
        }
    }

    protected void UnloadBuff(string buff, int slot)
    {
        switch (buff)
        {
            case "Root":
                //buffs[slot] = "None";
                rooted = false;
                Debug.Log("Player has been unrooted!");
                break;
            case "Rain":
                vitality -= buffMagnitudes[slot];
                break;
            case "Explosion":
                vitality += buffMagnitudes[slot];
                break;
            case "Snare":
                speed += buffMagnitudes[slot];
                break;
            case "Dispersion":
                focus += buffMagnitudes[slot];
                break;
            case "Fragment":
                lightRes += buffMagnitudes[slot];
                voidRes += buffMagnitudes[slot];
                fireRes += buffMagnitudes[slot];
                waterRes += buffMagnitudes[slot];
                airRes += buffMagnitudes[slot];
                earthRes += buffMagnitudes[slot];
                break;
            case "Piercing Light":
                vitality += buffMagnitudes[slot];
                break;
            case "Fireball":
                vitality += buffMagnitudes[slot];
                break;
            default:
                break;
        }

        Debug.Log("Removing " + buff + " buff from" + enemyName + "'s slot " + slot + "!");
        buffs[slot] = "None";
        buffMagnitudes[slot] = 0f;
       }

    public override void LoadBuff(string buff, float timer, float magnitude)
    {


        int slot = -1;
        int i = 0;
        //Checks for first open buff slot. If none, remains null

        while ((i < numBuffs) && (slot == -1))
        {
            if (buffs[i] == "None")
            {
                slot = i;
                Debug.Log("Loading " + buff + " buff to slot " + slot);
            }
            i++;
        }

        //If there's an available buff slot, loads the buff into that slot. Otherwise, skips
        if (slot != -1)
        {
            bool alreadyThere = false;
            switch (buff)
            {
                //Make sure you're checking to make sure non-stackables aren't stacking
                //Work out your timer business here
                case "Root":
                    for(int j = 0; j < numBuffs; j++)
                    {
                        if (buffs[j] == "Root")
                        {
                            alreadyThere = true;
                        }
                    }
                    if (alreadyThere == true)
                    {
                        slot = -1;
                    }
                    else
                    {
                        Debug.Log("Player has been rooted!");
                        rooted = true;
                    }
                    break;
                case "Rain":
                    vitality += magnitude;
                    break;
                case "Explosion":
                    vitality -= magnitude;
                    break;
                case "Snare":
                    for (int j = 0; j < numBuffs; j++)
                    {
                        if (buffs[j] == "Snare")
                        {
                            alreadyThere = true;
                        }
                    }
                    if (alreadyThere == true)
                    {
                        slot = -1;
                    }
                    else
                    {
                        speed -= magnitude;
                    }
                    break;
                case "Dispersion":
                    for (int j = 0; j < numBuffs; j++)
                    {
                        if (buffs[j] == "Dispersion")
                        {
                            alreadyThere = true;
                        }
                    }
                    if (alreadyThere == true)
                    {
                        slot = -1;
                    }
                    else
                    {
                        focus -= magnitude;
                    }
                    break;
                case "Fragment":
                    for (int j = 0; j < numBuffs; j++)
                    {
                        if (buffs[j] == "Fragment")
                        {
                            alreadyThere = true;
                        }
                    }
                    if (alreadyThere == true)
                    {
                        slot = -1;
                    }
                    else
                    {
                        lightRes -= magnitude;
                        voidRes -= magnitude;
                        fireRes -= magnitude;
                        waterRes -= magnitude;
                        airRes -= magnitude;
                        earthRes -= magnitude;
                    }
                    break;
                case "Piercing Light":
                    vitality -= magnitude;
                    break;
                case "Fireball":
                    for (int j = 0; j < numBuffs; j++)
                    {
                        if (buffs[j] == "Fireball")
                        {
                            alreadyThere = true;
                        }
                    }
                    if (alreadyThere == true)
                    {
                        slot = -1;
                    }
                    else
                    {
                        vitality -= magnitude;
                    }
                    break;
                default:
                    break;

            }





            //Checking again so that you can set slot back to -1 if it's a nonstackable above
            if (slot != -1)
            {
                buffs[slot] = buff;
                buffMagnitudes[slot] = magnitude;
                buffTimers[slot] = timer;
            }
        }
    }

    void FrameAdvance(int frames)
    {
        //Debug.Log("Beginning FrameAdvance");
        //Debug.Log("Current Action is: " + curAction);
        //if (curStartupFrame != 0) Debug.Log("Current Startup Frame is: " + curStartupFrame);
        //if (curActiveFrame != 0) Debug.Log("Current Active Frame is: " + curActiveFrame);
        //if (curEndingFrame != 0) Debug.Log("Current Ending Frame is: " + curEndingFrame);

        if((curFrame == frames)&&(curFrame != 0))
        {
            curFrame = 0;
            curAction = null;
        }

        if((curFrame != 0) && (curFrame < frames))
        {
            curFrame++;
        }
    }

    public override void TakeDamage(float incDmg, string incDmgType)
    {
        Debug.Log("Taking Damage! " + enemyName);
        Debug.Log("Incoming Damage Type is:" + incDmgType + " on " + enemyName);
        //subtracts incoming damage minus resist from hitpoints
        if (incDmgType == "Fire")
        {
            hitpoints -= incDmg - fireRes;
        }
        if (incDmgType == "Water")
        {
            hitpoints -= incDmg - waterRes;
        }
        if (incDmgType == "Air")
        {
            hitpoints -= incDmg - airRes;
        }
        if (incDmgType == "Earth")
        {
            hitpoints -= incDmg - earthRes;
        }
        if (incDmgType == "Light")
        {
            hitpoints -= incDmg - lightRes;
        }
        if (incDmgType == "Void")
        {
            WorldManager.Instance.AddToVoidPool(incDmg);
            hitpoints -= incDmg - voidRes;
        }
        //resets incDmg and incDmgType
        incDmg = 0;
        incDmgType = null;
        Debug.Log("Hit taken! Hitpoints remaining on " + enemyName + ": " + hitpoints);
    }

    public override void TakeKnockback(float incKB, Vector2 incKBDir)
    {
        myRB.AddForce(incKBDir * incKB * knockbackCoefficient);
        
        incKB = 0;
        incKBDir = Vector2.zero;
    }

    protected void OnDeath()
    {
        GameManager.SaveEvent -= SaveFunction;
        Debug.Log(enemyName + " died");
        Destroy(gameObject);
        //this.gameObject.SetActive(false);
    }

    protected void CallAction()
    {
        curAction?.Invoke();
        /*if (curAction != null)
        {
            curAction();
            switch (curAction)
            {
                case "MoveUp":
                    MoveUp();
                    break;
                case "MoveDown":
                    MoveDown();
                    break;
                case "MoveLeft":
                    MoveLeft();
                    break;
                case "MoveRight":
                    MoveRight();
                    break;
                case "ShortVoidAttack":
                    ShortVoidAttack();
                    break;
                default:
                    break;
            }
        }*/
    }

    protected void MoveUp()
    {
        //Debug.Log("Moving Up");
        int totalFrames = 2;
        if ((curFrame != 0))
        {
            //Debug.Log("Calling FrameAdvance inside MoveUp()");
            FrameAdvance(totalFrames);
        }



        //if (curFrame == 2) myRB.MovePosition(transform.position + Vector3.up * speed * Time.deltaTime);
        if (curFrame == 2) myRB.AddForce(Vector3.up * speed);
    }

    protected void MoveDown()
    {
        //Debug.Log("Moving Down");
        int totalFrames = 2;
        if ((curFrame != 0))
        {
            //Debug.Log("Calling FrameAdvance inside MoveDown()");
            FrameAdvance(totalFrames);
        }



        //if (curFrame == 2) myRB.MovePosition(transform.position + Vector3.down * speed * Time.deltaTime);
        if (curFrame == 2) myRB.AddForce(Vector3.down * speed);
    }

    protected void MoveLeft()
    {
        //Debug.Log("Moving Left");
        int totalFrames = 2;
        if ((curFrame != 0))
        {
            //Debug.Log("Calling FrameAdvance inside MoveLeft()");
            FrameAdvance(totalFrames);
        }



        //if (curFrame == 2) myRB.MovePosition(transform.position + Vector3.left * speed * Time.deltaTime);
        if (curFrame == 2) myRB.AddForce(Vector3.left * speed);
    }

    protected void MoveRight()
    {
        //Debug.Log("Moving Right");
        int totalFrames = 2;
        if ((curFrame != 0))
        {
            //Debug.Log("Calling FrameAdvance inside MoveRight()");
            FrameAdvance(totalFrames);
        }



        //if (curFrame == 2) myRB.MovePosition(transform.position + Vector3.right * speed * Time.deltaTime);
        if (curFrame == 2) myRB.AddForce(Vector3.right * speed);
    }

    protected void ShortVoidAttack() //Short claw attack, half the length of user, full width
    {
        int totalFrames = 23;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        switch (direction)
        {
            case "Up":
                if ((curFrame == 16))
                {
                    if (Player.Instance.showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(purpleHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f * xPixels / 32, .5f * yPixels / 32, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + (yPixels * .01f * .5f + yPixels * .01f * .25f), 0);
                        myHitboxScript1.framesTilGone = 8;
                    }
                    Entity.TriggerHits(this.transform.position.x - (xPixels * .01f * .5f), this.transform.position.x + (xPixels * .01f * .5f), this.transform.position.y + (yPixels * .01f + .01f), this.transform.position.y + (yPixels * .01f * .5f + .01f), voidDmg, "Void", 25, "Up");
                }
                break;
            case "Down":
                if ((curFrame == 16))
                {
                    if (Player.Instance.showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(purpleHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f * xPixels / 32, .5f * yPixels / 32, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - (yPixels * .01f * .5f + yPixels * .01f * .25f), 0);
                        myHitboxScript1.framesTilGone = 8;
                    }
                    Entity.TriggerHits(this.transform.position.x - (xPixels * .01f * .5f), this.transform.position.x + (xPixels * .01f * .5f), this.transform.position.y - (yPixels * .01f * .5f + .01f), this.transform.position.y - (yPixels * .01f + .01f), voidDmg, "Void", 25, "Down");
                }
                break;
            case "Left":
                if ((curFrame == 16))
                {
                    if (Player.Instance.showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(purpleHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(.5f * xPixels / 32, 1f * yPixels / 32, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x - (xPixels * .01f * .5f + xPixels * .01f * .25f), this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 8;
                    }
                    Entity.TriggerHits(this.transform.position.x - (xPixels * .01f + .01f), this.transform.position.x - (xPixels * .01f * .5f + .01f), this.transform.position.y + (yPixels * .01f * .5f), this.transform.position.y - (yPixels * .01f * .5f), voidDmg, "Void", 25, "Left");
                }
                break;
            case "Right":
                if ((curFrame == 16))
                {
                    if (Player.Instance.showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(purpleHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(.5f * xPixels / 32, 1f * yPixels / 32, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x + (xPixels*.01f*.5f + xPixels * .01f * .25f), this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 8;
                    }
                    Entity.TriggerHits(this.transform.position.x + (yPixels * .01f * .5f + .01f), this.transform.position.x + (xPixels * .01f + .01f), this.transform.position.y + (yPixels * .01f * .5f), this.transform.position.y - (yPixels * .01f * .5f), voidDmg, "Void", 25, "Right");
                }
                break;
            default:
                break;
        }
        //Claw. Short range melee spam.
    }

}


