using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy1Script : Enemy
{
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

    // Use this for initialization
    void Start()
    {
        Debug.Log("This Enemy is adding itself to the SaveFunction: " + enemyName);
        GameManager.SaveEvent += SaveFunction;

        knockbackCoefficient = 1;
        hitpoints = 50;
        maxHP = 50;
        vitality = 3;
        spirit = 0;
        maxSpirit = 0;
        focus = 0;
        speed = 15f;
        fireDmg = 0;
        waterDmg = 0;
        airDmg = 0;
        earthDmg = 0;
        lightDmg = 0;
        voidDmg = 0;
        fireRes = 0;
        waterRes = 0;
        airRes = 0;
        earthRes = 0;
        lightRes = 0;
        voidRes = 0;
        voidPower = 0;
        aggroRange = 1;
        xPixels = 32;
        yPixels = 32;

}

    //Disable this function when you're ready to start testing saving and level transitions
    void OnDestroy()
    {
        Debug.Log("This Enemy is removing itself from the SaveFunction: " + enemyName);
        GameManager.SaveEvent -= SaveFunction;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentGameState == GameManager.GameState.PLAY)
        {
            Regen();
            Timers();
        }

        if (hitpoints <= 0)
        {
            OnDeath();
        }

        BuffManagement();
        if (curAction != null) CallAction();
        if (curAction == null) RunAI();

    }

    void Timers()
    {

        if (pulseTimer > 0)
        {
            pulseTimer -= Time.deltaTime;
        }
        if (pulseTimer <= 0)
        {
            Pulse();
            pulseTimer = 1;
        }

        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
        }
        if (stunTimer < 0)
        {
            Debug.Log("Setting stunTimer to 0 from less than 0");
            stunTimer = 0;
        }

        //Debug.Log("Length of buffTimers is: " + buffTimers.Length);
        for (int i = 0; i < numBuffs; i++)
        {
            // Debug.Log("Going into the loop. i = " + i);
            if (buffTimers[i] > 0)
            {
                buffTimers[i] -= Time.deltaTime;
            }
            if (buffTimers[i] < 0)
            {
                buffTimers[i] = 0;
            }
        }

        if (invulTimer > 0) invulTimer--;
        if (invulTimer < 0) invulTimer = 0;
    }

    void Pulse()
    {
        VoidHealing();
        CheckInRange();
    }

    void CheckInRange()
    {
        //Debug.Log("Checking if Player is in range");
        //Check if the player is the aggro range
        if((Mathf.Pow(Player.Instance.transform.position.x - this.transform.position.x, 2) + Mathf.Pow(Player.Instance.transform.position.y - this.transform.position.y, 2)) < Mathf.Pow(aggroRange, 2))
        {
            playerInRange = true;
            //Debug.Log("Player is in range");
        }
        else
        {
            playerInRange = false;
            //Debug.Log("Player is not in range");
        }
    }

    string CheckDirectionOfPlayer()
    {
        //Return "Up", "Down", "Left", "Right", "UpRight", "UpLeft", "DownRight", "DownLeft"
        return "None";
    }



    void RunAI()
    {
        if((playerInRange == true) && (rooted == false))
        {
            //Do stuff
            if ((Mathf.Abs(Player.Instance.transform.position.x - this.transform.position.x) < Mathf.Abs(Player.Instance.transform.position.y - this.transform.position.y)) && (Player.Instance.transform.position.y > this.transform.position.y))
            {
                curFrame = 1;
                curAction = new currentAction(MoveUp);
                //Debug.Log("Setting curAction to MoveUp");
            }
            if ((Mathf.Abs(Player.Instance.transform.position.x - this.transform.position.x) < Mathf.Abs(Player.Instance.transform.position.y - this.transform.position.y)) && (Player.Instance.transform.position.y < this.transform.position.y))
            {
                curFrame = 1;
                curAction = new currentAction(MoveDown);
                //Debug.Log("Setting curAction to MoveDown");
            }
            if ((Mathf.Abs(Player.Instance.transform.position.x - this.transform.position.x) > Mathf.Abs(Player.Instance.transform.position.y - this.transform.position.y)) && (Player.Instance.transform.position.x > this.transform.position.x))
            {
                curFrame = 1;
                curAction = new currentAction(MoveRight);
                //Debug.Log("Setting curAction to MoveRight");
            }
            if ((Mathf.Abs(Player.Instance.transform.position.x - this.transform.position.x) > Mathf.Abs(Player.Instance.transform.position.y - this.transform.position.y)) && (Player.Instance.transform.position.x < this.transform.position.x))
            {
                curFrame = 1;
                curAction = new currentAction(MoveLeft);
                //Debug.Log("Setting curAction to MoveLeft");
            }
        }
        else
        {
            //Idle activities
        }
    }

    /*The idea is going to be: We set up functions like the attacks on Player
     * It'll check with Pulse to see if Player is in range
     * If player is in range, it will run a heuristic to figure out its next option
     *      Movement OR Attack OR Other Behavior
     *      Run each through a FrameAdvance
     *  Check between each action that Player is still in range
     *  
     *  Build the FrameAdvance functions and the Behavior functions on Enemy
     *  Build the heuristics on the Inherited class
     *  
     *  Make the attacks take a directional input as a string, then aim them based on that
     *  
     *  Put the hitboxen on, too
    */
}
