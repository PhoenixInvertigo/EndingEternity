using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Entity
{
    public float testKnockback;
    public float easySpeed;
    public bool showHitboxen = true;
    public static Player Instance;

    public Transform playerPosition;
    public Transform groundTarget;

    public Animator animator;

    public Slider myHealthBar;
    public Slider mySpiritBar;

    public float gtSpeed = 0.05f;
    float gtRange;
    float curGTx;
    float curGTy;
    bool groundTargetting = false;
    bool selectingForm = false;
    bool dashing = false;
    string selectedForm = "None";

    public PlayerStatistics localPlayerData = new PlayerStatistics();
    /*int SceneID;
	float PositionX;
	float PositionY;*/

    //public Entity eScript = null;
    enum Direction { Up, Down, Left, Right };
    Direction myDirection;
    float dirTimer = 0;
    enum ListAttack
    {
        Void1, Void2, Void3, Void4, Light1, Light2, Light3, Light4, Fire1, Fire2, Fire3, Fire4,
        Water1, Water2, Water3, Water4, Air1, Air2, Air3, Air4, Earth1, Earth2, Earth3, Earth4
    };
    ListAttack myAttack1 = ListAttack.Light1;
    ListAttack myAttack2 = ListAttack.Light2;
    ListAttack myAttack3 = ListAttack.Light3;
    ListAttack myAttack4 = ListAttack.Light4;
    public enum CurrentForm { VoidForm, LightForm, FireForm, WaterForm, AirForm, EarthForm };
    CurrentForm myForm = CurrentForm.LightForm;

    public GameObject redHitbox;
    public GameObject orangeHitbox;
    public GameObject yellowHitbox;
    public GameObject greenHitbox;
    public GameObject blueHitbox;
    public GameObject indigoHitbox;
    public GameObject purpleHitbox;
    public GameObject formSelector;
    GameObject myFormSelector = null;
    FormSelectorScript myFormSelectorScript = null;

    public GameObject airMine;
    GameObject[] mines;
    int mineCounter = 0;
    public GameObject stunMine;
    GameObject myStunMine = null;
    public GameObject fireball;
      
    public float Hitpoints
    {
        get
        {
            return hitpoints;
        }
    }
    public float MaxHP
    {
        get
        {
            return currentForm.maxHP;
        }
    }
    public float Vitality
    {
        get
        {
            return currentForm.vitality;
        }
    }
    public float Spirit
    {
        get
        {
            return spirit;
        }
    }
    public float MaxSpirit
    {
        get
        {
            return currentForm.maxSpirit;
        }
    }
    public float Focus
    {
        get
        {
            return currentForm.focus;
        }
    }
    public float Speed
    {
        get
        {
            return currentForm.speed;
        }
    }
    public float KnockBack
    {
        get
        {
            return currentForm.knockback;
        }
    }
    public float Dmg
    {
        get
        {
            return currentForm.dmg;
        }
    }
    public float LightRes
    {
        get
        {
            return currentForm.lightRes;
        }
    }
    public int LightLevel
    {
        get
        {
            return localPlayerData.lightLevel;
        }
    }
    public int LightMastery
    {
        get
        {
            return localPlayerData.lightMastery;
        }
    }
    public float VoidRes
    {
        get
        {
            return currentForm.voidRes;
        }
    }
    public int VoidLevel
    {
        get
        {
            return localPlayerData.voidLevel;
        }
    }
    public int VoidMastery
    {
        get
        {
            return localPlayerData.voidMastery;
        }
    }
    public float WaterRes
    {
        get
        {
            return currentForm.waterRes;
        }
    }
    public int WaterLevel
    {
        get
        {
            return localPlayerData.waterLevel;
        }
    }
    public int WaterMastery
    {
        get
        {
            return localPlayerData.waterMastery;
        }
    }
    public float FireRes
    {
        get
        {
            return currentForm.fireRes;
        }
    }
    public int FireLevel
    {
        get
        {
            return localPlayerData.fireLevel;
        }
    }
    public int FireMastery
    {
        get
        {
            return localPlayerData.fireMastery;
        }
    }
    public float EarthRes
    {
        get
        {
            return currentForm.earthRes;
        }
    }
    public int EarthLevel
    {
        get
        {
            return localPlayerData.earthLevel;
        }
    }
    public int EarthMastery
    {
        get
        {
            return localPlayerData.earthMastery;
        }
    }
    public float AirRes
    {
        get
        {
            return currentForm.airRes;
        }
    }
    public int AirLevel
    {
        get
        {
            return localPlayerData.airLevel;
        }
    }
    public int AirMastery
    {
        get
        {
            return localPlayerData.airMastery;
        }
    }

    struct Form
    {
        public float maxHP;
        public float vitality;
        public float maxSpirit;
        public float focus;
        public float dmg;
        public float fireRes;
        public float waterRes;
        public float airRes;
        public float earthRes;
        public float lightRes;
        public float voidRes;
        public float speed;
        public float knockback;

        public Form(float incMaxHP, float incVitality, float incMaxSpirit, float incFocus, float incDmg, float incFireRes, float incWaterRes,
            float incAirRes, float incEarthRes, float incLightRes, float incVoidRes, float incSpeed, float incKnockback)
        {
            maxHP = incMaxHP;
            vitality = incVitality;
            maxSpirit = incMaxSpirit;
            focus = incFocus;
            dmg = incDmg;
            fireRes = incFireRes;
            waterRes = incWaterRes;
            airRes = incAirRes;
            earthRes = incEarthRes;
            lightRes = incLightRes;
            voidRes = incVoidRes;
            speed = incSpeed;
            knockback = incKnockback;
        }
    }

    Form currentForm = new Form(50, 5, 50, 5, 1, 1, 1, 1, 1, 1, 1, 1, 1);
    Form baseForm = new Form(50, 1, 50, 1, 0, 1, 1, 1, 1, 1, 1, 20f, 1);
    Form tempForm = new Form(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
    Form permForm = new Form(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
    Form fireForm = new Form(4, .4f, 5, .9f, 5, 6, 0, 3, 0, 2, 0, .75f, -.01f);
    Form waterForm = new Form(2, .3f, 3, .1f, 3, 4, 2, 1, 0, 1, 0, .75f, -.03f);
    Form airForm = new Form(2, .2f, 3, .9f, 2, 0, 1, 4, 2, 1, 0, 2f, -.01f);
    Form earthForm = new Form(6, .3f, 2, .2f, 2, 4, 4, 2, 4, 4, 3, .75f, -.09f);
    Form lightForm = new Form(6, .4f, 2, .4f, 4, 1, 1, 1, 1, 2, 0, 1.5f, -.02f);
    Form voidForm = new Form(10, .1f, 2, .4f, 4, 1, 1, 1, 1, 0, 6, 1f, -.03f);

    float hitpoints = 50;
        float spirit = 50;

    float fire1Cost = 10;
    float fire2Cost = 8;
    float fire3Cost = 4;
    float fire4Cost = 20;
    float water1Cost = 10;
    float water2Cost = 20;
    float water3Cost = 0;
    float water4Cost = 10;
    float air1Cost = 5;
    float air2Cost = 10;
    float air3Cost = 5;
    float air4Cost = 25;
    float earth1Cost = 10;
    float earth2Cost = 10;
    float earth3Cost = 20;
    float earth4Cost = 5;
    float light1Cost = 15;
    float light2Cost = 10;
    float light3Cost = 10;
    float light4Cost = 3;
    float void1Cost = 10;
    float void2Cost = 3;
    float void3Cost = 5;
    float void4Cost = 20;

    float myCost1 = 0;
    float myCost2 = 0;
    float myCost3 = 0;
    float myCost4 = 0;

    float attack1CD = 0;
    float attack2CD = 0;
    float attack3CD = 0;
    float attack4CD = 0;
    float stunTimer = 0;
    float startupTimer = 0;
    float pulseTimer = 0;

    public Rigidbody2D rb;
    public float stopRatio = 0.8f;

    public bool isInLava = false;
    public bool isAtGreaterResonanceCrystal = false;
    float dCooldown = 0;

    bool ltInUse = false;
    bool rtInUse = false;

    Vector3 moveChange = Vector3.zero; //Vector handling the normalized directional input for movement

    //Set of variables which handle the control flow for actions
        //Starts at Startup 1 and cycles through each for the actions
    int curFrame = 0;

    //string curAction = "None";
    delegate void CurrentAction();
    CurrentAction curAction = null;
    CurrentAction GTQueue = null;
    //string GTQueue = "None";
    string dashDir = "Up";

    string[] buffs;
    float[] buffTimers;
    float[] buffMagnitudes;
    public int numBuffs = 5;

    bool rooted = false;
    float dashSpeed = 0f;
    bool lightDash = false; //bool for controlling the dash portion of Light2
    bool earthDash = false; //bool for controlling the dash portion of Earth2

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (Instance != this)
            Destroy(gameObject);

        buffs = new string[numBuffs];
        buffTimers = new float[numBuffs];
        buffMagnitudes = new float[numBuffs];


        for (int i = 0; i < numBuffs; i++)
        {
            buffs[i] = "None";
            buffTimers[i] = 0f;
            buffMagnitudes[i] = 0f;
        }

    }

    void Start()
    {
        localPlayerData = GameManager.Instance.savedPlayerData;

        if (GameManager.Instance.IsSceneBeingLoaded)
        {
            localPlayerData = GameManager.Instance.LocalCopyOfData;
            transform.position = new Vector2(
                GameManager.Instance.LocalCopyOfData.PositionX,
                GameManager.Instance.LocalCopyOfData.PositionY);
            GameManager.Instance.IsSceneBeingLoaded = false;
        }

        if (GameManager.Instance.IsSceneBeingTransitioned)
        {
            localPlayerData = GameManager.Instance.LocalCopyOfData;
            transform.position = new Vector2(
                GameManager.Instance.TransitionTarget.position.x,
                GameManager.Instance.TransitionTarget.position.y);
            GameManager.Instance.IsSceneBeingTransitioned = false;

        }




       /* mines = new GameObject[3];
        mines[0] = null;
        mines[1] = null;
        mines[2] = null;*/

        InitializeStats();
        SetAirMines();
    }

    private void OnEnable()
    {
        Entity.Pool.Add(this);
        //incBuff = null;
    }
    private void OnDisable()
    {
        Entity.Pool.Remove(this);
    }

    void InitializeStats()
    {
        Debug.Log("Initializing Stats");


        localPlayerData.lightLevel = 1;
        localPlayerData.voidLevel = 1;
        localPlayerData.fireLevel = 1;
        localPlayerData.waterLevel = 1;
        localPlayerData.earthLevel = 1;
        localPlayerData.airLevel = 1;

        localPlayerData.lightMastery = 0;
        localPlayerData.voidMastery = 0;
        localPlayerData.fireMastery = 0;
        localPlayerData.waterMastery = 0;
        localPlayerData.earthMastery = 0;
        localPlayerData.airMastery = 0;

        SetForm("Light");
        myDirection = Direction.Up;
        animator.SetBool("FacingDown", false);
        animator.SetBool("FacingLeft", false);
        animator.SetBool("FacingRight", false);
        animator.SetBool("FacingUp", true);
    }

    void SetAirMines()
    {
        mines = new GameObject[localPlayerData.airLevel];

      for(int i = 0; i < localPlayerData.airLevel; i++)
        {
            mines[i] = null;
            Debug.Log("Nulling mines[" + i + "]");
        }
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.currentGameState == GameManager.GameState.PLAY)
        {
            //Regen();
            //Timers();
            if (rb.velocity.x != 0)
            {
                rb.velocity = new Vector2(rb.velocity.x * stopRatio, rb.velocity.y);
            }
            if (rb.velocity.y != 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * stopRatio);
            }
            moveChange = Vector3.zero;
            MoveChange();
            if ((moveChange != Vector3.zero) && (rooted == false) && (selectingForm == false))
            {
                MoveCharacter();
            }
            if ((dashing == true) && (rooted == false))
            {
                DashCharacter();
            }

        }       
    }

    void Update()
    {

        if (GameManager.Instance.currentGameState == GameManager.GameState.PLAY)
        {
            Regen();
            Timers();

            BuffManagement();

            CallAction();
            Controls();
            if (groundTargetting == true)
            {
                GroundTarget();
            }
            if(selectingForm == true)
            {
                SelectForm();
            }



        }
        else if (GameManager.Instance.currentGameState == GameManager.GameState.PAUSE)
        {
            PausedControls();
        }

        if (hitpoints <= 0)
        {
            OnDeath();
        }



    }

    void CallAction()
    {
        curAction?.Invoke();
        /*if(curAction != "None")
        {
            switch (curAction)
            {
                case "Fire1":
                    Fire1();
                    break;
                case "FinishFire1":
                    FinishFire1();
                    break;
                case "Fire2":
                    Fire2();
                    break;
                case "Fire3":
                    Fire3();
                    break;
                case "Fire4":
                    Fire4();
                    break;
                case "Light1":
                    Light1();
                    break;
                case "Light2":
                    Light2();
                    break;
                case "FinishLight2":
                    FinishLight2();
                    break;
                case "Light3":
                    Light3();
                    break;
                case "Light4":
                    Light4();
                    break;
                case "Water1":
                    Water1();
                    break;
                case "Water2":
                    Water2();
                    break;
                case "Water3":
                    Water3();
                    break;
                case "Water4":
                    Water4();
                    break;
                case "Earth1":
                    Earth1();
                    break;
                case "Earth2":
                    Earth2();
                    break;
                case "FinishEarth2":
                    FinishEarth2();
                    break;
                case "Earth3":
                    Earth3();
                    break;
                case "Earth4":
                    Earth4();
                    break;
                case "Air1":
                    Air1();
                    break;
                case "Air2":
                    Air2();
                    break;
                case "Air3":
                    Air3();
                    break;
                case "Air4":
                    Air4();
                    break;
                case "Void1":
                    Void1();
                    break;
                case "Void2":
                    Void2();
                    break;
                case "Void3":
                    Void3();
                    break;
                case "FinishVoid3":
                    FinishVoid3();
                    break;
                case "Void4":
                    Void4();
                    break;
                case "FinishVoid4":
                    FinishVoid4();
                    break;
                case "Dash":
                    Dash();
                    break;
                default:
                    break;
            }
        }*/
    }

    void Timers()
    {
        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime * 60;
        }
        if (startupTimer > 0)
        {
            startupTimer -= Time.deltaTime * 60;
        }
        if (invulTimer > 0)
        {
            startupTimer -= Time.deltaTime * 60;
        }
        if (attack1CD > 0)
        {
            attack1CD -= Time.deltaTime;
        }
        if (attack2CD > 0)
        {
            attack2CD -= Time.deltaTime;
        }
        if (attack3CD > 0)
        {
            attack3CD -= Time.deltaTime;
        }
        if (attack4CD > 0)
        {
            attack4CD -= Time.deltaTime;
        }
        if (attack1CD < 0)
        {
            attack1CD = 0;
        }
        if (attack2CD < 0)
        {
            attack2CD = 0;
        }
        if (attack3CD < 0)
        {
            attack3CD = 0;
        }
        if (attack4CD < 0)
        {
            attack4CD = 0;
        }

        for(int i = 0; i < numBuffs; i++)
        {
            if(buffTimers[i] > 0)
            {
                buffTimers[i] -= Time.deltaTime;
            }
            if(buffTimers[i] < 0)
            {
                buffTimers[i] = 0;
            }
        }
        if (stunTimer < 0)
        {
            stunTimer = 0;
        }
        if (startupTimer < 0)
        {
            startupTimer = 0;
        }
        if (invulTimer < 0)
        {
            invulTimer = 0;
        }
        if(dCooldown > 0)
        {
            dCooldown -= Time.deltaTime;
        }
        if(dCooldown < 0)
        {
            dCooldown = 0;
        }
        if(dirTimer > 0)
        {
            dirTimer -= Time.deltaTime;
        }
        if(dirTimer < 0)
        {
            dirTimer = 0;
        }
        if(pulseTimer > 0)
        {
            pulseTimer -= Time.deltaTime;

        }
        if (pulseTimer <= 0)
        {
            Pulse();
            pulseTimer = 1;
        }



        if (invulTimer > 0)
        {
            invulTimer--;
        }
        if (invulTimer < 0)
        {
            invulTimer = 0;
        }

    }

    void Pulse()
    {
        //Debug.Log("Pulsing");
        //Place things here that should increment once a second
        if(isInLava == true)
        {
            Debug.Log("Holy shit help me I'm on fire (pulse)");
        }

        for(int i = 0; i < numBuffs; i++)
        {
            if (buffs[i] == "Purge")
            {
                for (int j = 0; j < numBuffs; j++)
                {
                    if ((buffs[j] != "None") && (buffs[j] != "Meditation") && (buffs[j] != "Rain") && (buffs[j] != "Purge"))
                    {
                        Debug.Log("Purging slot " + j + " of buff " + buffs[j]);
                        UnloadBuff(buffs[j], j);
                    }
                }
            }
        }

        VoidHealing();
    }

    void Regen()
    {
        if (hitpoints < currentForm.maxHP)
        {
            hitpoints += Time.deltaTime * currentForm.vitality;
        }

        if (spirit < currentForm.maxSpirit)
        {
            spirit += Time.deltaTime * currentForm.focus;
        }

        myHealthBar.value = hitpoints / currentForm.maxHP;
        mySpiritBar.value = spirit / currentForm.maxSpirit;
    }

    void BuffManagement()
    {
        for(int i = 0; i < numBuffs; i++)
        {
            if((buffs[i] != "None") && (buffTimers[i] == 0))
            {
                UnloadBuff(buffs[i], i);
            }
        }
    }

    void UnloadBuff(string buff, int slot)
    {
        switch (buff)
        {
            case "Root":
                //buffs[slot] = "None";
                rooted = false;
                Debug.Log("Player has been unrooted!");
                break;
            case "Rain":
                tempForm.vitality -= buffMagnitudes[slot];
                break;
            case "Explosion":
                tempForm.vitality += buffMagnitudes[slot];
                break;
            case "Meditation":
                tempForm.focus -= buffMagnitudes[slot];
                break;
            case "Endure":
                tempForm.airRes -= buffMagnitudes[slot];
                tempForm.earthRes -= buffMagnitudes[slot];
                tempForm.fireRes -= buffMagnitudes[slot];
                tempForm.waterRes -= buffMagnitudes[slot];
                tempForm.voidRes -= buffMagnitudes[slot];
                tempForm.lightRes -= buffMagnitudes[slot];
                tempForm.speed += .25f * buffMagnitudes[slot];
                break;
            case "Windrip":
                tempForm.speed -= buffMagnitudes[slot];
                break;
            case "Snare":
                tempForm.speed += buffMagnitudes[slot];
                break;
            case "Ember":
                tempForm.dmg -= buffMagnitudes[slot];
                break;
            default:
                break;
        }

        Debug.Log("Removing " + buff + " buff from player's slot " + slot + "!");
        buffs[slot] = "None";
        buffMagnitudes[slot] = 0f;
        UpdateTempStats();
    }

    public override void LoadBuff(string buff, float timer, float magnitude)
    {


        int slot = -1;
        int i = 0;
        //Checks for first open buff slot. If none, remains null

        while((i < numBuffs) && (slot == -1))
        {
            if(buffs[i] == "None")
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
                        if(buffs[j] == "Root")
                        {
                            alreadyThere = true;
                        }
                    }
                    if (alreadyThere == true)
                    {
                        slot = -1;
                    } else
                    {
                        Debug.Log("Player has been rooted!");
                        rooted = true;
                    }
                    break;
                case "Rain":
                    tempForm.vitality += magnitude;
                    break;
                case "Explosion":
                    for (int j = 0; j < numBuffs; j++)
                    {
                        if (buffs[j] == "Explosion")
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
                        tempForm.vitality -= magnitude;
                    }
                    break;
                case "Meditation":
                    tempForm.focus += magnitude;
                    break;
                case "Endure":
                    for (int j = 0; j < numBuffs; j++)
                    {
                        if (buffs[j] == "Endure")
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
                        tempForm.airRes += magnitude;
                        tempForm.earthRes += magnitude;
                        tempForm.fireRes += magnitude;
                        tempForm.waterRes += magnitude;
                        tempForm.voidRes += magnitude;
                        tempForm.lightRes += magnitude;
                        tempForm.speed -= .25f * magnitude;
                    }
                    break;
                case "Purge":
                    for (int j = 0; j < numBuffs; j++)
                    {
                        if (buffs[j] == "Purge")
                        {
                            alreadyThere = true;
                        }
                    }
                    if (alreadyThere == true)
                    {
                        slot = -1;
                    }
                    break;
                case "Windrip":
                    for (int j = 0; j < numBuffs; j++)
                    {
                        if (buffs[j] == "Windrip")
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
                        tempForm.speed += magnitude;
                    }
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
                        tempForm.speed -= magnitude;
                    }
                    break;
                case "Ember":
                    tempForm.dmg += magnitude;
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

            UpdateTempStats();
        }
    }

    void SetForm(string form)
    {
        switch (form)
        {
            case "Light":
                Debug.Log("Setting Light Form");
                myForm = CurrentForm.LightForm;
                permForm.maxHP = baseForm.maxHP + lightForm.maxHP * localPlayerData.lightLevel;
                permForm.vitality = baseForm.vitality + lightForm.vitality * localPlayerData.lightLevel;
                permForm.maxSpirit = baseForm.maxSpirit + lightForm.maxSpirit * localPlayerData.lightLevel;
                permForm.focus = baseForm.focus + lightForm.focus * localPlayerData.lightLevel;
                permForm.speed = baseForm.speed + lightForm.speed * localPlayerData.lightLevel;
                permForm.knockback = baseForm.knockback + lightForm.knockback * localPlayerData.lightLevel;
                permForm.dmg = baseForm.dmg + lightForm.dmg * localPlayerData.lightLevel;
                permForm.fireRes = baseForm.fireRes + lightForm.fireRes * localPlayerData.lightLevel;
                permForm.waterRes = baseForm.waterRes + lightForm.waterRes * localPlayerData.lightLevel;
                permForm.airRes = baseForm.airRes + lightForm.airRes * localPlayerData.lightLevel;
                permForm.earthRes = baseForm.earthRes + lightForm.earthRes * localPlayerData.lightLevel;
                permForm.lightRes = baseForm.lightRes + lightForm.lightRes * localPlayerData.lightLevel;
                permForm.voidRes = baseForm.voidRes + lightForm.voidRes * localPlayerData.lightLevel;

                myAttack1 = ListAttack.Light1;
                myAttack2 = ListAttack.Light2;
                myAttack3 = ListAttack.Light3;
                myAttack4 = ListAttack.Light4;
                myCost1 = light1Cost;
                myCost2 = light2Cost;
                myCost3 = light3Cost;
                myCost4 = light4Cost;
                animator.SetBool("IsWater", false);
                animator.SetBool("IsFire", false);
                animator.SetBool("IsEarth", false);
                animator.SetBool("IsAir", false);
                animator.SetBool("IsVoid", false);
                animator.SetBool("IsLight", true);
                break;
            case "Void":
                Debug.Log("Setting Void Form");
                myForm = CurrentForm.VoidForm;
                permForm.maxHP = baseForm.maxHP + voidForm.maxHP * localPlayerData.voidLevel;
                permForm.vitality = baseForm.vitality + voidForm.vitality * localPlayerData.voidLevel;
                permForm.maxSpirit = baseForm.maxSpirit + voidForm.maxSpirit * localPlayerData.voidLevel;
                permForm.focus = baseForm.focus + voidForm.focus * localPlayerData.voidLevel;
                permForm.speed = baseForm.speed + voidForm.speed * localPlayerData.voidLevel;
                permForm.knockback = baseForm.knockback + voidForm.knockback * localPlayerData.voidLevel;
                permForm.dmg = baseForm.dmg + voidForm.dmg * localPlayerData.voidLevel;
                permForm.fireRes = baseForm.fireRes + voidForm.fireRes * localPlayerData.voidLevel;
                permForm.waterRes = baseForm.waterRes + voidForm.waterRes * localPlayerData.voidLevel;
                permForm.airRes = baseForm.airRes + voidForm.airRes * localPlayerData.voidLevel;
                permForm.earthRes = baseForm.earthRes + voidForm.earthRes * localPlayerData.voidLevel;
                permForm.lightRes = baseForm.lightRes + voidForm.lightRes * localPlayerData.voidLevel;
                permForm.voidRes = baseForm.voidRes + voidForm.voidRes * localPlayerData.voidLevel;
                myAttack1 = ListAttack.Void1;
                myAttack2 = ListAttack.Void2;
                myAttack3 = ListAttack.Void3;
                myAttack4 = ListAttack.Void4;
                myCost1 = void1Cost;
                myCost2 = void2Cost;
                myCost3 = void3Cost;
                myCost4 = void4Cost;
                animator.SetBool("IsLight", false);
                animator.SetBool("IsWater", false);
                animator.SetBool("IsFire", false);
                animator.SetBool("IsEarth", false);
                animator.SetBool("IsAir", false);
                animator.SetBool("IsVoid", true);
                break;
            case "Fire":
                Debug.Log("Setting Fire Form");
                myForm = CurrentForm.FireForm;
                permForm.maxHP = baseForm.maxHP + fireForm.maxHP * localPlayerData.fireLevel;
                permForm.vitality = baseForm.vitality + fireForm.vitality * localPlayerData.fireLevel;
                permForm.maxSpirit = baseForm.maxSpirit + fireForm.maxSpirit * localPlayerData.fireLevel;
                permForm.focus = baseForm.focus + fireForm.focus * localPlayerData.fireLevel;
                permForm.speed = baseForm.speed + fireForm.speed * localPlayerData.fireLevel;
                permForm.knockback = baseForm.knockback + fireForm.knockback * localPlayerData.fireLevel;
                permForm.dmg = baseForm.dmg + fireForm.dmg * localPlayerData.fireLevel;
                permForm.fireRes = baseForm.fireRes + fireForm.fireRes * localPlayerData.fireLevel;
                permForm.waterRes = baseForm.waterRes + fireForm.waterRes * localPlayerData.fireLevel;
                permForm.airRes = baseForm.airRes + fireForm.airRes * localPlayerData.fireLevel;
                permForm.earthRes = baseForm.earthRes + fireForm.earthRes * localPlayerData.fireLevel;
                permForm.lightRes = baseForm.lightRes + fireForm.lightRes * localPlayerData.fireLevel;
                permForm.voidRes = baseForm.voidRes + fireForm.voidRes * localPlayerData.fireLevel;
                myAttack1 = ListAttack.Fire1;
                myAttack2 = ListAttack.Fire2;
                myAttack3 = ListAttack.Fire3;
                myAttack4 = ListAttack.Fire4;
                myCost1 = fire1Cost;
                myCost2 = fire2Cost;
                myCost3 = fire3Cost;
                myCost4 = fire4Cost;
                animator.SetBool("IsLight", false);
                animator.SetBool("IsWater", false);
                animator.SetBool("IsEarth", false);
                animator.SetBool("IsAir", false);
                animator.SetBool("IsVoid", false);
                animator.SetBool("IsFire", true);
                break;
            case "Water":
                Debug.Log("Setting Water Form");
                myForm = CurrentForm.WaterForm;
                permForm.maxHP = baseForm.maxHP + waterForm.maxHP * localPlayerData.waterLevel;
                permForm.vitality = baseForm.vitality + waterForm.vitality * localPlayerData.waterLevel;
                permForm.maxSpirit = baseForm.maxSpirit + waterForm.maxSpirit * localPlayerData.waterLevel;
                permForm.focus = baseForm.focus + waterForm.focus * localPlayerData.waterLevel;
                permForm.speed = baseForm.speed + waterForm.speed * localPlayerData.waterLevel;
                permForm.knockback = baseForm.knockback + waterForm.knockback * localPlayerData.waterLevel;
                permForm.dmg = baseForm.dmg + waterForm.dmg * localPlayerData.waterLevel;
                permForm.fireRes = baseForm.fireRes + waterForm.fireRes * localPlayerData.waterLevel;
                permForm.waterRes = baseForm.waterRes + waterForm.waterRes * localPlayerData.waterLevel;
                permForm.airRes = baseForm.airRes + waterForm.airRes * localPlayerData.waterLevel;
                permForm.earthRes = baseForm.earthRes + waterForm.earthRes * localPlayerData.waterLevel;
                permForm.lightRes = baseForm.lightRes + waterForm.lightRes * localPlayerData.waterLevel;
                permForm.voidRes = baseForm.voidRes + waterForm.voidRes * localPlayerData.waterLevel;
                myAttack1 = ListAttack.Water1;
                myAttack2 = ListAttack.Water2;
                myAttack3 = ListAttack.Water3;
                myAttack4 = ListAttack.Water4;
                myCost1 = water1Cost;
                myCost2 = water2Cost;
                myCost3 = water3Cost;
                myCost4 = water4Cost;
                animator.SetBool("IsLight", false);
                animator.SetBool("IsFire", false);
                animator.SetBool("IsEarth", false);
                animator.SetBool("IsAir", false);
                animator.SetBool("IsVoid", false);
                animator.SetBool("IsWater", true);
                break;
            case "Air":
                Debug.Log("Setting Air Form");
                myForm = CurrentForm.AirForm;
                permForm.maxHP = baseForm.maxHP + airForm.maxHP * localPlayerData.airLevel;
                permForm.vitality = baseForm.vitality + airForm.vitality * localPlayerData.airLevel;
                permForm.maxSpirit = baseForm.maxSpirit + airForm.maxSpirit * localPlayerData.airLevel;
                permForm.focus = baseForm.focus + airForm.focus * localPlayerData.airLevel;
                permForm.speed = baseForm.speed + airForm.speed * localPlayerData.airLevel;
                permForm.knockback = baseForm.knockback + airForm.knockback * localPlayerData.airLevel;
                permForm.dmg = baseForm.dmg + airForm.dmg * localPlayerData.airLevel;
                permForm.fireRes = baseForm.fireRes + airForm.fireRes * localPlayerData.airLevel;
                permForm.waterRes = baseForm.waterRes + airForm.waterRes * localPlayerData.airLevel;
                permForm.airRes = baseForm.airRes + airForm.airRes * localPlayerData.airLevel;
                permForm.earthRes = baseForm.earthRes + airForm.earthRes * localPlayerData.airLevel;
                permForm.lightRes = baseForm.lightRes + airForm.lightRes * localPlayerData.airLevel;
                permForm.voidRes = baseForm.voidRes + airForm.voidRes * localPlayerData.airLevel;
                myAttack1 = ListAttack.Air1;
                myAttack2 = ListAttack.Air2;
                myAttack3 = ListAttack.Air3;
                myAttack4 = ListAttack.Air4;
                myCost1 = air1Cost;
                myCost2 = air2Cost;
                myCost3 = air3Cost;
                myCost4 = air4Cost;
                animator.SetBool("IsLight", false);
                animator.SetBool("IsWater", false);
                animator.SetBool("IsFire", false);
                animator.SetBool("IsEarth", false);
                animator.SetBool("IsVoid", false);
                animator.SetBool("IsAir", true);
                break;
            case "Earth":
                Debug.Log("Setting Earth Form");
                myForm = CurrentForm.EarthForm;
                permForm.maxHP = baseForm.maxHP + earthForm.maxHP * localPlayerData.earthLevel;
                permForm.vitality = baseForm.vitality + earthForm.vitality * localPlayerData.earthLevel;
                permForm.maxSpirit = baseForm.maxSpirit + earthForm.maxSpirit * localPlayerData.earthLevel;
                permForm.focus = baseForm.focus + earthForm.focus * localPlayerData.earthLevel;
                permForm.speed = baseForm.speed + earthForm.speed * localPlayerData.earthLevel;
                permForm.knockback = baseForm.knockback + earthForm.knockback * localPlayerData.earthLevel;
                permForm.dmg = baseForm.dmg + earthForm.dmg * localPlayerData.earthLevel;
                permForm.fireRes = baseForm.fireRes + earthForm.fireRes * localPlayerData.earthLevel;
                permForm.waterRes = baseForm.waterRes + earthForm.waterRes * localPlayerData.earthLevel;
                permForm.airRes = baseForm.airRes + earthForm.airRes * localPlayerData.earthLevel;
                permForm.earthRes = baseForm.earthRes + earthForm.earthRes * localPlayerData.earthLevel;
                permForm.lightRes = baseForm.lightRes + earthForm.lightRes * localPlayerData.earthLevel;
                permForm.voidRes = baseForm.voidRes + earthForm.voidRes * localPlayerData.earthLevel;
                myAttack1 = ListAttack.Earth1;
                myAttack2 = ListAttack.Earth2;
                myAttack3 = ListAttack.Earth3;
                myAttack4 = ListAttack.Earth4;
                myCost1 = earth1Cost;
                myCost2 = earth2Cost;
                myCost3 = earth3Cost;
                myCost4 = earth4Cost;
                animator.SetBool("IsLight", false);
                animator.SetBool("IsWater", false);
                animator.SetBool("IsFire", false);
                animator.SetBool("IsAir", false);
                animator.SetBool("IsVoid", false);
                animator.SetBool("IsEarth", true);
                break;
            default:
                Debug.Log("No form chosen for SetForm");
                break;
        }
        attack1CD = 0;
        attack2CD = 0;
        attack3CD = 0;
        attack4CD = 0;
        AddXP(localPlayerData.xpReservoir);
        UpdateTempStats();
    }

    public void AddXP(int incXP)
    {
        //Get incXP
        //Get current form level
        //Calculate xp to next level
        //Get current form XP
        //See if capped (level is mastery + 10)
        //if capped, add incXP to reservoir
        //if reservoir is overcapped, return to cap
        //if not capped, add xp to current form XP
        //See if current form xp > level up
        //If yes, subtract xp to next level and level up
        switch (myForm)
        {
            case CurrentForm.FireForm:
                if (localPlayerData.fireLevel >= localPlayerData.fireMastery + 5)
                {
                    localPlayerData.xpReservoir += incXP;
                    if (localPlayerData.xpReservoir > localPlayerData.maxXPReservoir)
                    {
                        localPlayerData.xpReservoir = localPlayerData.maxXPReservoir;
                    }
                    incXP = 0;
                }
                else
                {
                    localPlayerData.fireXP += incXP;
                    if (localPlayerData.fireXP >= Mathf.Pow(localPlayerData.fireLevel, 3))
                    {
                        localPlayerData.fireXP -= (int)Mathf.Pow(localPlayerData.fireLevel, 3);
                        LevelUp("Fire");
                    }
                    incXP = 0;
                    Debug.Log("Added XP to FireXP");
                }
                break;
            case CurrentForm.WaterForm:
                if (localPlayerData.waterLevel >= localPlayerData.waterMastery + 5)
                {
                    localPlayerData.xpReservoir += incXP;
                    if (localPlayerData.xpReservoir > localPlayerData.maxXPReservoir)
                    {
                        localPlayerData.xpReservoir = localPlayerData.maxXPReservoir;
                    }
                    incXP = 0;
                }
                else
                {
                    localPlayerData.waterXP += incXP;
                    if (localPlayerData.waterXP >= Mathf.Pow(localPlayerData.waterLevel, 3))
                    {
                        localPlayerData.waterXP -= (int)Mathf.Pow(localPlayerData.waterLevel, 3);
                        LevelUp("Water");
                    }
                    incXP = 0;
                    Debug.Log("Added XP to WaterXP");
                }
                break;
            case CurrentForm.AirForm:
                if (localPlayerData.airLevel >= localPlayerData.airMastery + 5)
                {
                    localPlayerData.xpReservoir += incXP;
                    if (localPlayerData.xpReservoir > localPlayerData.maxXPReservoir)
                    {
                        localPlayerData.xpReservoir = localPlayerData.maxXPReservoir;
                    }
                    incXP = 0;
                }
                else
                {
                    localPlayerData.airXP += incXP;
                    if (localPlayerData.airXP >= Mathf.Pow(localPlayerData.airLevel, 3))
                    {
                        localPlayerData.airXP -= (int)Mathf.Pow(localPlayerData.airLevel, 3);
                        LevelUp("Air");
                    }
                    incXP = 0;
                    Debug.Log("Added XP to AirXP");
                }
                break;
            case CurrentForm.EarthForm:
                if (localPlayerData.earthLevel >= localPlayerData.earthMastery + 5)
                {
                    localPlayerData.xpReservoir += incXP;
                    if (localPlayerData.xpReservoir > localPlayerData.maxXPReservoir)
                    {
                        localPlayerData.xpReservoir = localPlayerData.maxXPReservoir;
                    }
                    incXP = 0;
                }
                else
                {
                    localPlayerData.earthXP += incXP;
                    if (localPlayerData.earthXP >= Mathf.Pow(localPlayerData.earthLevel, 3))
                    {
                        localPlayerData.earthXP -= (int)Mathf.Pow(localPlayerData.earthLevel, 3);
                        LevelUp("Earth");
                    }
                    incXP = 0;
                    Debug.Log("Added XP to EarthXP");
                }
                break;
            case CurrentForm.LightForm:
                if (localPlayerData.lightLevel >= localPlayerData.lightMastery + 5)
                {
                    localPlayerData.xpReservoir += incXP;
                    if (localPlayerData.xpReservoir > localPlayerData.maxXPReservoir)
                    {
                        localPlayerData.xpReservoir = localPlayerData.maxXPReservoir;
                    }
                    incXP = 0;
                }
                else
                {
                    localPlayerData.lightXP += incXP;
                    if (localPlayerData.lightXP >= Mathf.Pow(localPlayerData.lightLevel, 3))
                    {
                        localPlayerData.lightXP -= (int)Mathf.Pow(localPlayerData.lightLevel, 3);
                        LevelUp("Light");
                    }
                    incXP = 0;
                    Debug.Log("Added XP to LightXP");
                }
                break;
            case CurrentForm.VoidForm:
                if (localPlayerData.voidLevel >= localPlayerData.voidMastery + 5)
                {
                    localPlayerData.xpReservoir += incXP;
                    if (localPlayerData.xpReservoir > localPlayerData.maxXPReservoir)
                    {
                        localPlayerData.xpReservoir = localPlayerData.maxXPReservoir;
                    }
                    incXP = 0;
                }
                else
                {
                    localPlayerData.voidXP += incXP;
                    if (localPlayerData.voidXP >= Mathf.Pow(localPlayerData.voidLevel, 3))
                    {
                        localPlayerData.voidXP -= (int)Mathf.Pow(localPlayerData.voidLevel, 3);
                        LevelUp("Void");
                    }
                    incXP = 0;
                    Debug.Log("Added XP to VoidXP");
                }
                break;
            default:
                Debug.Log("AddXP had no form to add to");
                break;
        }
    }

    void LevelUp(string formToLevel)
    {
        //run a switch on formToLevel that +1s that form's level
        //Check if form is now level capped
        //If capped, move all form xp to reservoir
        switch (formToLevel)
        {
            case "Fire":
                Debug.Log("Leveling Fire");
                localPlayerData.fireLevel++;
                Debug.Log("New level is: " + localPlayerData.fireLevel);
                if (localPlayerData.fireLevel >= 5 + localPlayerData.fireMastery)
                {
                    localPlayerData.xpReservoir += localPlayerData.fireXP;
                    if (localPlayerData.xpReservoir > localPlayerData.maxXPReservoir)
                    {
                        localPlayerData.xpReservoir = localPlayerData.maxXPReservoir;
                    }
                    localPlayerData.fireXP = 0;
                }
                break;
            case "Water":
                Debug.Log("Leveling Water");
                localPlayerData.waterLevel++;
                Debug.Log("New level is: " + localPlayerData.waterLevel);
                if (localPlayerData.waterLevel >= 5 + localPlayerData.waterMastery)
                {
                    localPlayerData.xpReservoir += localPlayerData.waterXP;
                    if (localPlayerData.xpReservoir > localPlayerData.maxXPReservoir)
                    {
                        localPlayerData.xpReservoir = localPlayerData.maxXPReservoir;
                    }
                    localPlayerData.waterXP = 0;
                }
                break;
            case "Air":
                Debug.Log("Leveling Air");
                localPlayerData.airLevel++;
                SetAirMines();
                Debug.Log("New level is: " + localPlayerData.airLevel);
                if (localPlayerData.airLevel >= 5 + localPlayerData.airMastery)
                {
                    localPlayerData.xpReservoir += localPlayerData.airXP;
                    if (localPlayerData.xpReservoir > localPlayerData.maxXPReservoir)
                    {
                        localPlayerData.xpReservoir = localPlayerData.maxXPReservoir;
                    }
                    localPlayerData.airXP = 0;
                }
                break;
            case "Earth":
                Debug.Log("Leveling Earth");
                localPlayerData.earthLevel++;
                Debug.Log("New level is: " + localPlayerData.earthLevel);
                if (localPlayerData.earthLevel >= 5 + localPlayerData.earthMastery)
                {
                    localPlayerData.xpReservoir += localPlayerData.earthXP;
                    if (localPlayerData.xpReservoir > localPlayerData.maxXPReservoir)
                    {
                        localPlayerData.xpReservoir = localPlayerData.maxXPReservoir;
                    }
                    localPlayerData.earthXP = 0;
                }
                break;
            case "Light":
                Debug.Log("Leveling Light");
                localPlayerData.lightLevel++;
                Debug.Log("New level is: " + localPlayerData.lightLevel);
                if (localPlayerData.lightLevel >= 5 + localPlayerData.lightMastery)
                {
                    localPlayerData.xpReservoir += localPlayerData.lightXP;
                    if (localPlayerData.xpReservoir > localPlayerData.maxXPReservoir)
                    {
                        localPlayerData.xpReservoir = localPlayerData.maxXPReservoir;
                    }
                    localPlayerData.lightXP = 0;
                }
                break;
            case "Void":
                Debug.Log("Leveling Void");
                localPlayerData.voidLevel++;
                Debug.Log("New level is: " + localPlayerData.voidLevel);
                if (localPlayerData.voidLevel >= 5 + localPlayerData.voidMastery)
                {
                    localPlayerData.xpReservoir += localPlayerData.voidXP;
                    if (localPlayerData.xpReservoir > localPlayerData.maxXPReservoir)
                    {
                        localPlayerData.xpReservoir = localPlayerData.maxXPReservoir;
                    }
                    localPlayerData.voidXP = 0;
                }
                break;
            default:
                Debug.Log("Form to Level had no form");
                break;
        }
    }

    void GainMastery(string formToMaster)
    {
        //run a switch on formToMaster that +1's that form's mastery
        //that form's level = that form's mastery
        //update Reservoir Cap
        switch (formToMaster)
        {
            case "Fire":
                if (localPlayerData.fireMastery < 5)
                {
                    localPlayerData.fireMastery++;
                    localPlayerData.fireLevel = localPlayerData.fireMastery;
                    Debug.Log("Fire Mastery gained");
                }
                break;
            case "Water":
                if (localPlayerData.waterMastery < 5)
                {
                    localPlayerData.waterMastery++;
                    localPlayerData.waterLevel = localPlayerData.waterMastery;
                    Debug.Log("Water Mastery gained");
                }
                break;
            case "Air":
                if (localPlayerData.airMastery < 5)
                {
                    localPlayerData.airMastery++;
                    localPlayerData.airLevel = localPlayerData.airMastery;
                    SetAirMines();
                    Debug.Log("Air Mastery gained");
                }
                break;
            case "Earth":
                if (localPlayerData.earthMastery < 5)
                {
                    localPlayerData.earthMastery++;
                    localPlayerData.earthLevel = localPlayerData.earthMastery;
                    Debug.Log("Earth Mastery gained");
                }
                break;
            case "Light":
                if (localPlayerData.lightMastery < 5)
                {
                    localPlayerData.lightMastery++;
                    localPlayerData.lightLevel = localPlayerData.lightMastery;
                    Debug.Log("Light Mastery gained");
                }
                break;
            case "Void":
                if (localPlayerData.voidMastery < 5)
                {
                    localPlayerData.voidMastery++;
                    localPlayerData.voidLevel = localPlayerData.voidMastery;
                    Debug.Log("Void Mastery gained");
                }
                break;
            default:
                Debug.Log("Form to Master had no form");
                break;

        }
        UpdateReservoirCap();
    }

    void UpdateReservoirCap()
    {
        //Reservoir Cap = x * sum of all mastery + 10 or something
        localPlayerData.maxXPReservoir = 5 * (1 + localPlayerData.airMastery + localPlayerData.earthMastery + localPlayerData.waterMastery + localPlayerData.fireMastery + localPlayerData.lightMastery + localPlayerData.voidMastery);
        Debug.Log("Reservoir updated");
    }

    void UpdateTempStats()
    {
        Debug.Log("Updating Temp Stats");
        currentForm.maxHP       = permForm.maxHP         + tempForm.maxHP;
        currentForm.vitality    = permForm.vitality      + tempForm.vitality;
        currentForm.maxSpirit   = permForm.maxSpirit     + tempForm.maxSpirit;
        currentForm.focus       = permForm.focus         + tempForm.focus;
        currentForm.speed       = permForm.speed         + tempForm.speed;
        currentForm.knockback   = permForm.knockback     + tempForm.knockback;
        currentForm.dmg         = permForm.dmg           + tempForm.dmg;
        currentForm.fireRes     = permForm.fireRes       + tempForm.fireRes;
        currentForm.waterRes    = permForm.waterRes      + tempForm.waterRes;
        currentForm.airRes      = permForm.airRes        + tempForm.airRes;
        currentForm.earthRes    = permForm.earthRes      + tempForm.earthRes;
        currentForm.lightRes    = permForm.lightRes      + tempForm.lightRes;
        currentForm.voidRes     = permForm.voidRes       + tempForm.voidRes;

    }

    public override void TakeDamage(float incDmg, string incDmgType)
    {
        Debug.Log("Player taking Damage!");
        Debug.Log("Player's incoming Damage Type is: " + incDmgType);
        //subtracts incoming damage minus resist from hitpoints. Heals are increased by resist, as they are negative damage

        if (incDmgType == "Fire")
        {
            hitpoints -= incDmg - currentForm.fireRes;
        }
        if (incDmgType == "Water")
        {
            hitpoints -= incDmg - currentForm.waterRes;
        }
        if (incDmgType == "Air")
        {
            hitpoints -= incDmg - currentForm.airRes;
        }
        if (incDmgType == "Earth")
        {
            hitpoints -= incDmg - currentForm.earthRes;
        }
        if (incDmgType == "Light")
        {
            hitpoints -= incDmg - currentForm.lightRes;
        }
        if (incDmgType == "Void")
        {
            WorldManager.Instance.AddToVoidPool(incDmg);
            hitpoints -= incDmg - currentForm.voidRes;
        }

        //resets incDmg and incDmgType
        incDmg = 0;
        incDmgType = null;
        Debug.Log("Player's incoming damage type after nulling is: " + incDmgType);
        Debug.Log("Hit taken! Player hitpoints remaining: " + hitpoints);
    }

    public override void TakeKnockback(float incKB, Vector2 incKBDir)
    {
        rb.AddForce(incKBDir * incKB * currentForm.knockback);

        /*incKB = 0;
        incKBDir = Vector2.zero;*/
    }

    void VoidHealing()
    {
        if(myForm == CurrentForm.VoidForm)
        {
            if((WorldManager.Instance.voidPool > 0) && (hitpoints < currentForm.maxHP))
            {
                if(WorldManager.Instance.voidPool > (currentForm.maxHP - hitpoints)*10/localPlayerData.voidLevel)
                {
                    float healing = (currentForm.maxHP - hitpoints) * 10 / localPlayerData.voidLevel;

                    WorldManager.Instance.RemoveFromVoidPool(healing);
                    hitpoints += healing * localPlayerData.voidLevel / 10;
                } else
                {
                    float healing = WorldManager.Instance.voidPool;

                    WorldManager.Instance.RemoveFromVoidPool(healing);
                    hitpoints += healing * localPlayerData.voidLevel / 10;
                }
            }
        }
    }

    void LoadCharacter()
    {
        localPlayerData = GameManager.Instance.savedPlayerData;
    }

    void SaveCharacter()
    {
        GameManager.Instance.savedPlayerData = localPlayerData;
    }

    void QuickSave()
    {

        Debug.Log("QuickSaving");
        localPlayerData.SceneID = SceneManager.GetActiveScene().buildIndex;
        localPlayerData.PositionX = transform.position.x;
        localPlayerData.PositionY = transform.position.y;

        SaveCharacter();
        LoadCharacter();


        GameManager.Instance.SaveData();


    }

    void QuickLoad()
    {

        GameManager.Instance.LoadData();
        GameManager.Instance.IsSceneBeingLoaded = true;

        int whichScene = GameManager.Instance.LocalCopyOfData.SceneID;

        SceneManager.LoadScene(whichScene);

    }

    void CycleForms()
   
    {
        //This function is for testing only. Disable it later.
        isAtGreaterResonanceCrystal = true;
        if (localPlayerData.fireUnlocked == false)
        {
            localPlayerData.fireUnlocked = true;
        }
        if (localPlayerData.airUnlocked == false)
        {
            localPlayerData.airUnlocked = true;
        }
        if (localPlayerData.waterUnlocked == false)
        {
            localPlayerData.waterUnlocked = true;
        }
        if (localPlayerData.earthUnlocked == false)
        {
            localPlayerData.earthUnlocked = true;
        }
        if (localPlayerData.voidUnlocked == false)
        {
            localPlayerData.voidUnlocked = true;
        }
        switch (Instance.myForm)
        {
            case CurrentForm.LightForm:
                SetForm("Fire");
                Debug.Log("Entered Fire Form");
                break;
            case CurrentForm.FireForm:
                SetForm("Water");
                Debug.Log("Entered Water Form");
                break;
            case CurrentForm.WaterForm:
                SetForm("Air");
                Debug.Log("Entered Air Form");
                break;
            case CurrentForm.AirForm:
                SetForm("Earth");
                Debug.Log("Entered Earth Form");
                break;
            case CurrentForm.EarthForm:
                SetForm("Void");
                Debug.Log("Entered Void Form");
                break;
            case CurrentForm.VoidForm:
                SetForm("Light");
                Debug.Log("Entered Light Form");
                break;
            default:
                Debug.Log("Error in Form Cycling");
                break;

        }
    }

    void ReverseCycleForms()
   
    {
        //This function is for testing only. Disable it later.
        isAtGreaterResonanceCrystal = true;
        if (localPlayerData.fireUnlocked == false)
        {
            localPlayerData.fireUnlocked = true;
        }
        if (localPlayerData.airUnlocked == false)
        {
            localPlayerData.airUnlocked = true;
        }
        if (localPlayerData.waterUnlocked == false)
        {
            localPlayerData.waterUnlocked = true;
        }
        if (localPlayerData.earthUnlocked == false)
        {
            localPlayerData.earthUnlocked = true;
        }
        if (localPlayerData.voidUnlocked == false)
        {
            localPlayerData.voidUnlocked = true;
        }
        switch (Instance.myForm)
        {
            case CurrentForm.LightForm:
                SetForm("Void");
                Debug.Log("Entered Void Form");
                break;
            case CurrentForm.FireForm:
                SetForm("Light");
                Debug.Log("Entered Light Form");
                break;
            case CurrentForm.WaterForm:
                SetForm("Fire");
                Debug.Log("Entered Fire Form");
                break;
            case CurrentForm.AirForm:
                SetForm("Water");
                Debug.Log("Entered Water Form");
                break;
            case CurrentForm.EarthForm:
                SetForm("Air");
                Debug.Log("Entered Air Form");
                break;
            case CurrentForm.VoidForm:
                SetForm("Earth");
                Debug.Log("Entered Earth Form");
                break;
            default:
                Debug.Log("Error in Reverse Form Cycling");
                break;

        }
    }

    void SelectForm()
    {
        //Set this up kinda like groundtarget
        //Shouldn't need a confirm/cancel button
        //Releasing Y will act as both
        if((Input.GetAxisRaw("LeftX") < -.65) && (Input.GetAxisRaw("LeftY") > .1) && (localPlayerData.airUnlocked == true))
        {
            selectedForm = "Air";
            myFormSelectorScript.SelectAir();
        }
        if ((Input.GetAxisRaw("LeftX") < -.65) && (Input.GetAxisRaw("LeftY") < -.1) && (localPlayerData.waterUnlocked == true))
        {
            selectedForm = "Water";
            myFormSelectorScript.SelectWater();
        }
        if ((Input.GetAxisRaw("LeftX") > .65) && (Input.GetAxisRaw("LeftY") < -.1) && (localPlayerData.earthUnlocked == true))
        {
            selectedForm = "Earth";
            myFormSelectorScript.SelectEarth();
        }
        if ((Input.GetAxisRaw("LeftX") > .65) && (Input.GetAxisRaw("LeftY") > .1) && (localPlayerData.fireUnlocked == true))
        {
            selectedForm = "Fire";
            myFormSelectorScript.SelectFire();
        }
        if((Input.GetAxisRaw("LeftX") < .4) && (Input.GetAxisRaw("LeftX") > -.4) && (Input.GetAxisRaw("LeftY") > .6) && (localPlayerData.lightUnlocked == true))
        {
            selectedForm = "Light";
            myFormSelectorScript.SelectLight();
        }
        if ((Input.GetAxisRaw("LeftX") < .4) && (Input.GetAxisRaw("LeftX") > -.4) && (Input.GetAxisRaw("LeftY") < -.6) && (localPlayerData.voidUnlocked == true))
        {
            selectedForm = "Void";
            myFormSelectorScript.SelectVoid();
        }
    }

    void SetSelectedForm()
    {
        switch (selectedForm)
        {
            case "Earth":
                if(localPlayerData.earthUnlocked == true)
                {
                    SetForm("Earth");
                    selectedForm = "None";
                }
                //else, play Nuh Uh sound
                break;
            case "Fire":
                if(localPlayerData.fireUnlocked == true)
                {
                    SetForm("Fire");
                    selectedForm = "None";
                }
                break;
            case "Light":
                if (localPlayerData.lightUnlocked == true)
                {
                    SetForm("Light");
                    selectedForm = "None";
                }
                break;
            case "Air":
                if (localPlayerData.airUnlocked == true)
                {
                    SetForm("Air");
                    selectedForm = "None";
                }
                break;
            case "Water":
                if (localPlayerData.waterUnlocked == true)
                {
                    SetForm("Water");
                    selectedForm = "None";
                }
                break;
            case "Void":
                if (localPlayerData.voidUnlocked == true)
                {
                    SetForm("Void");
                    selectedForm = "None";
                }
                break;
            default:
                Debug.Log("Couldn't SetSelectedForm()");
                break;
        }
    }

    void OnDeath()
    {
        Debug.Log("You ded");
    }

    void GroundTarget()
    {


        if ((Input.GetAxis("RightX") > 0))
        {
            if ((Mathf.Pow((groundTarget.position.x - playerPosition.position.x), 2) + Mathf.Pow((groundTarget.position.y - playerPosition.position.y), 2)) < Mathf.Pow(gtRange, 2))
            {
                groundTarget.position = new Vector2(groundTarget.position.x + gtSpeed, groundTarget.position.y);
            }
            else if ((groundTarget.position.x < playerPosition.position.x))
            {
                groundTarget.position = new Vector2(groundTarget.position.x + gtSpeed, groundTarget.position.y);
            }
        }

        if ((Input.GetAxis("RightX") < 0))
        {

            if ((Mathf.Pow((groundTarget.position.x - playerPosition.position.x), 2) + Mathf.Pow((groundTarget.position.y - playerPosition.position.y), 2)) < Mathf.Pow(gtRange, 2))
            {
                groundTarget.position = new Vector2(groundTarget.position.x - gtSpeed, groundTarget.position.y);
            }
            else if ((groundTarget.position.x > playerPosition.position.x))
            {
                groundTarget.position = new Vector2(groundTarget.position.x - gtSpeed, groundTarget.position.y);
            }

        }
        if ((Input.GetAxis("RightY") > 0))
        {
            if ((Mathf.Pow((groundTarget.position.x - playerPosition.position.x), 2) + Mathf.Pow((groundTarget.position.y - playerPosition.position.y), 2)) < Mathf.Pow(gtRange, 2))
            {
                groundTarget.position = new Vector2(groundTarget.position.x, groundTarget.position.y + gtSpeed);
            }
            else if ((groundTarget.position.y < playerPosition.position.y))
            {
                groundTarget.position = new Vector2(groundTarget.position.x, groundTarget.position.y + gtSpeed);
            }

        }
        if ((Input.GetAxis("RightY") < 0))
        {
            if ((Mathf.Pow((groundTarget.position.x - playerPosition.position.x), 2) + Mathf.Pow((groundTarget.position.y - playerPosition.position.y), 2)) < Mathf.Pow(gtRange, 2))
            {
                groundTarget.position = new Vector2(groundTarget.position.x, groundTarget.position.y - gtSpeed);
            }
            else if ((groundTarget.position.y > playerPosition.position.y))
            {
                groundTarget.position = new Vector2(groundTarget.position.x, groundTarget.position.y - gtSpeed);
            }

        }
        if (Input.GetButtonDown("A"))
        {
            Debug.Log("Groundtargetting confirmed");
            curGTx = groundTarget.position.x;
            curGTy = groundTarget.position.y;
            groundTargetting = false;
            gtRange = 0;
            groundTarget.position = new Vector2(playerPosition.position.x, playerPosition.position.y);
            NextGT();
            GTQueue = null;
        }
        if (Input.GetButtonDown("B"))
        {
            Debug.Log("Groundtargetting cancelled");
            groundTargetting = false;
            gtRange = 0;
            groundTarget.position = new Vector2(playerPosition.position.x, playerPosition.position.y);
            GTQueue = null;
        }



    }

    void PausedControls()
    {
        if (Input.GetButtonDown("Start"))
        {
            Debug.Log("Start");
            if (GameManager.Instance.currentGameState == GameManager.GameState.PAUSE)
            {
                GameManager.Instance.currentGameState = GameManager.GameState.PLAY;
                Debug.Log("Current State of Game is: " + GameManager.Instance.currentGameState as string);
            }
        }
    }
    
    void Controls()
    {
        //Left Stick is X and Y axis
        //Triggers are 3rd Axis
        //9th axis is left trigger, 10th axis is right trigger
        //Right stick is 4th and 5th axis (x and y respectively)
        //D-Pad is 6th and 7th axis (x and y)
        //A = 0; B = 1; X = 2; Y = 3
        //LB = 4; RB = 5; Back = 6; Start = 7
        //Left Stick in = 8; Right Stick in = 9

        if (Input.GetButtonDown("A"))
        {
            Debug.Log("A Button");
            Interact();
        }
        if (Input.GetButtonDown("B"))
        {
            Debug.Log("B Button");
            if ((groundTargetting == false) && (selectingForm == false) && (curAction == null) && (stunTimer == 0))
            {
                NextAction("Dash");
            }
        }
        if (Input.GetButtonDown("X"))
        {
            Debug.Log("X Button");
        }

        if (Input.GetButtonDown("Y"))
        {
            Debug.Log("Y Button");
            if((selectingForm == false) && (curAction == null) && (groundTargetting == false) && (stunTimer == 0))
            {
                selectingForm = true;
                selectedForm = "Light";
                Debug.Log("SelectingForm == true. Selected form == " + selectedForm);
                myFormSelector = (GameObject)Instantiate(formSelector);
                myFormSelectorScript = myFormSelector.GetComponent<FormSelectorScript>();
                myFormSelector.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
            }
        }
        if (Input.GetButtonUp("Y"))
        {
            Debug.Log("Y Released");
            if((selectingForm == true) && (curAction == null))
            {
                if(isAtGreaterResonanceCrystal == true)
                {
                    SetSelectedForm();

                }
                //if can't shift, play Nuh Uh sound
                selectingForm = false;
                myFormSelectorScript.DisableFormSelector();
                myFormSelector = null;
                myFormSelectorScript = null;
                Debug.Log("FormSelector should be destroyed and null");
            }
        }
        if (Input.GetButtonDown("LB"))
        {
            //Debug.Log("Left Bumper");
            //QuickSave();
            if ((groundTargetting == false) && (selectingForm == false) && (curAction == null) && (stunTimer == 0) && (attack4CD == 0) && (spirit >= myCost4))
            {
                //QueueAttack4();
                spirit -= myCost4;
                NextAction("Attack4");
            }
        }
        if (Input.GetButtonDown("RB"))
        {
            //Debug.Log("Right Bumper");
            //QuickLoad();
            if ((groundTargetting == false) && (selectingForm == false) && (curAction == null) && (stunTimer == 0) && (attack3CD == 0) && (spirit >= myCost3))
            {
                //QueueAttack3();
                spirit -= myCost3;
                NextAction("Attack3");
            }
        }
        if (Input.GetButtonDown("Back"))
        {
            Debug.Log("Back");

        }
        if (Input.GetButtonDown("Start"))
        {
            Debug.Log("Start");
            if (GameManager.Instance.currentGameState == GameManager.GameState.PLAY)
            {
                GameManager.Instance.currentGameState = GameManager.GameState.PAUSE;
                Debug.Log("Current State of Game is: " + GameManager.Instance.currentGameState as string);
            }
        }
        if (Input.GetButtonDown("LeftStick"))
        {
            Debug.Log("Left Stick");
        }
        if (Input.GetButtonDown("RightStick"))
        {
            Debug.Log("Right Stick");
        }
        if ((Input.GetAxisRaw("LeftX") > 0) && (selectingForm == false) && ((curAction == null) || (dashing == true)) && ((Mathf.Abs(Input.GetAxisRaw("LeftX"))) > (Mathf.Abs(Input.GetAxisRaw("LeftY")))))
        {
            if (dirTimer == 0)
            {
                //Debug.Log("Player Facing Right");
                Player.Instance.myDirection = Direction.Right;
                animator.SetBool("FacingUp", false);
                animator.SetBool("FacingLeft", false);
                animator.SetBool("FacingDown", false);
                animator.SetBool("FacingRight", true);
                dirTimer += 0.025f;
            }
        }
        if ((Input.GetAxisRaw("LeftX") < 0) && (selectingForm == false) && ((curAction == null) || (dashing == true)) && ((Mathf.Abs(Input.GetAxisRaw("LeftX"))) > (Mathf.Abs(Input.GetAxisRaw("LeftY")))))
        {
            if (dirTimer == 0)
            {
                //Debug.Log("Player Facing Left");
                Player.Instance.myDirection = Direction.Left;
                animator.SetBool("FacingUp", false);
                animator.SetBool("FacingRight", false);
                animator.SetBool("FacingDown", false);
                animator.SetBool("FacingLeft", true);
                dirTimer += 0.025f;
            }
        }
        if ((Input.GetAxisRaw("LeftY") > 0) && (selectingForm == false) && ((curAction == null) || (dashing == true)) && ((Mathf.Abs(Input.GetAxisRaw("LeftY"))) > (Mathf.Abs(Input.GetAxisRaw("LeftX")))))
        {
         
            if (dirTimer == 0)
            {
                //Debug.Log("Player Facing Up");
                Player.Instance.myDirection = Direction.Up;
                animator.SetBool("FacingLeft", false);
                animator.SetBool("FacingRight", false);
                animator.SetBool("FacingDown", false);
                animator.SetBool("FacingUp", true);
                dirTimer += 0.025f;
            }
        }
        if ((Input.GetAxisRaw("LeftY") < 0) && (selectingForm == false) && ((curAction == null) || (dashing == true)) && ((Mathf.Abs(Input.GetAxisRaw("LeftY"))) > (Mathf.Abs(Input.GetAxisRaw("LeftX")))))
        {
 
            if (dirTimer == 0)
            {
                //Debug.Log("Player Facing Down");
                Player.Instance.myDirection = Direction.Down;
                animator.SetBool("FacingUp", false);
                animator.SetBool("FacingLeft", false);
                animator.SetBool("FacingRight", false);
                animator.SetBool("FacingDown", true);
                dirTimer += 0.025f;
            }
        }
        if ((Input.GetAxis("LT")) > 0 && (ltInUse == false))
        {
            ltInUse = true;
            Debug.Log("Left Trigger");
            //incXP = 10;
            //AddXP();
            if ((groundTargetting == false) && (selectingForm == false) && (curAction == null) && (stunTimer == 0) && (attack2CD == 0) && (spirit >= myCost2))
            {
                //QueueAttack2();
                spirit -= myCost2;
                NextAction("Attack2");
            }
        }
        if((Input.GetAxis("LT") == 0) && (ltInUse == true))
        {
            Debug.Log("Setting LT In Use to false");
            ltInUse = false;
        }
        if ((Input.GetAxis("RT") > 0) && (rtInUse == false))
        {
            rtInUse = true;
            Debug.Log("Right Trigger");
            //localPlayerData.speed = easySpeed;
            if ((groundTargetting == false) && (selectingForm == false) && (curAction == null) && (stunTimer == 0) && (attack1CD == 0) && (spirit >= myCost1))
            {
                //QueueAttack1();
                spirit -= myCost1;
                NextAction("Attack1");
            }
        }
        if((Input.GetAxis("RT") == 0) && (rtInUse == true))
        {
            Debug.Log("Setting RT In Use to false");
            rtInUse = false;
        }
        if (Input.GetAxis("RightX") > 0)
        {
            //Debug.Log("Positive RightX Axis");
        }
        if (Input.GetAxis("RightX") < 0)
        {
            //Debug.Log("Negative RightX Axis");
        }
        if (Input.GetAxis("RightY") > 0)
        {
            //Debug.Log("Positive RightY Axis");
        }
        if (Input.GetAxis("RightY") < 0)
        {
            //Debug.Log("Negative RightY Axis");
        }
        if ((Input.GetAxis("DX") > 0) && (dCooldown <= 0))
        {
            CycleForms();
            Debug.Log("Positive DX Axis");
            dCooldown = 0.3f;
        }
        if ((Input.GetAxis("DX") < 0) && (dCooldown <= 0))
        {
            ReverseCycleForms();
            Debug.Log("Negative DX Axis");
            dCooldown = 0.3f;
        }
        if ((Input.GetAxis("DY") > 0) && (dCooldown <= 0))
        {
            Debug.Log("Positive DY Axis");
            dCooldown = 0.3f;
        }
        if ((Input.GetAxis("DY") < 0) && (dCooldown <= 0))
        {
            Debug.Log("Negative DY Axis");
            dCooldown = 0.3f;
        }

    }

    void MoveChange()
    {if ((curAction == null) || (dashing = true))
        {
            //Debug.Log("MoveChange() is firing");
            moveChange.x = Input.GetAxisRaw("LeftX");
            moveChange.y = Input.GetAxisRaw("LeftY");

            //if (moveChange == Vector3.zero) Debug.Log("moveChange == Vector3.zero");
        }
    }

    void MoveCharacter()
    {
        //Debug.Log("currentForm.speed is: " + currentForm.speed as string);
        //rb.MovePosition(transform.position + moveChange.normalized * currentForm.speed * Time.deltaTime);
        rb.AddForce(moveChange.normalized * currentForm.speed); 
        //Debug.Log("transform.position.x is " + transform.position.x);
        
    }

    void DashCharacter()
    {
        switch (dashDir)
        {
            case "Up":
                //rb.MovePosition(transform.position + Vector3.up * dashSpeed * Time.deltaTime);
                rb.AddForce(Vector3.up * dashSpeed);
                break;
            case "Down":
                //rb.MovePosition(transform.position + Vector3.down * dashSpeed * Time.deltaTime);
                rb.AddForce(Vector3.down * dashSpeed);
                break;
            case "Left":
                //rb.MovePosition(transform.position + Vector3.left * dashSpeed * Time.deltaTime);
                rb.AddForce(Vector3.left * dashSpeed);
                break;
            case "Right":
                //rb.MovePosition(transform.position + Vector3.right * dashSpeed * Time.deltaTime);
                rb.AddForce(Vector3.right * dashSpeed);
                break;
            default:
                break;
        }
    }

    void NextAction(string next)
    {
        switch (next)
        {
            case "Attack1":
                Attack1();
                break;
            case "Attack2":
                Attack2();
                break;
            case "Attack3":
                Attack3();
                break;
            case "Attack4":
                Attack4();
                break;
            case "Dash":
                curFrame = 1;
                switch (myDirection)
                {
                    case Direction.Up:
                        dashDir = "Up";
                        break;
                    case Direction.Down:
                        dashDir = "Down";
                        break;
                    case Direction.Left:
                        dashDir = "Left";
                        break;
                    case Direction.Right:
                        dashDir = "Right";
                        break;
                    default:
                        break;
                }
                curAction = new CurrentAction(Dash);
                dashing = true;
                break;
            default:
                Debug.Log("No Next Action");
                break;
        }
    }

    void NextGT()
    {
        curAction = GTQueue;
        /*switch (next)
        {
            //These need to set Cur Action to what they're finishing
            //Then iniatialize the Frame counter by setting Current Startup Frame to 1
            case "Fire1":
                Debug.Log("NextGT is setting FinishFire1 as curAction");
                curAction = "FinishFire1";
                break;
            case "Void3":
                Debug.Log("NextGT is setting FinishVoid3 as curAction");
                curAction = "FinishVoid3";
                break;
            case "Void4":
                Debug.Log("NextGT is setting FinishVoid4 as curAction");
                curAction = "FinishVoid4";
                break;
            default:
                Debug.Log("No GT Queue");
                break;
        }*/
    }

    void FrameAdvance(int frames)//Pushes the frames forward by 1, iterating through different frame types
    {
        if ((curFrame == frames) && (curFrame != 0))
        {
            curFrame = 0;
            Debug.Log("Attempting to null curAction");
            curAction = null;
            if (curAction == null) Debug.Log("curAction is null");
        }

        if ((curFrame != 0) && (curFrame < frames))
        {
            curFrame++;
        }

    }

    void Dash()
    {
        /*int startupFrames = 9;
        int activeFrames = 20;
        int endingFrames = 20;*/
        int totalFrames = 49;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        if(curFrame == 2)
        {
            dashSpeed = currentForm.speed * .5f;
        }
        if(curFrame == 4)
        {
            dashSpeed = currentForm.speed * 1f;
        }
        if(curFrame == 6)
        {
            dashSpeed = currentForm.speed * 1.5f;
        }
        if(curFrame == 8)
        {
            dashSpeed = currentForm.speed * 2f;
        }
        if(curFrame == 10)
        {
            dashSpeed = currentForm.speed * 2.5f;
        }
        if(curFrame == 30)
        {
            dashSpeed = currentForm.speed * 2.4f;
        }
        if(curFrame == 32)
        {
            dashSpeed = currentForm.speed * 2.3f;
        }
        if(curFrame == 34)
        {
            dashSpeed = currentForm.speed * 2.2f;
        }
        if(curFrame == 36)
        {
            dashSpeed = currentForm.speed * 2.1f;
        }
        if(curFrame == 38)
        {
            dashSpeed = currentForm.speed * 2f;
        }
        if(curFrame == 40)
        {
            dashSpeed = currentForm.speed * 1.8f;
        }
        if(curFrame == 42)
        {
            dashSpeed = currentForm.speed * 1.5f;
        }
        if(curFrame == 44)
        {
            dashSpeed = currentForm.speed * 1.1f;
        }
        if(curFrame == 46)
        {
            dashSpeed = currentForm.speed * .6f;
        }
        if(curFrame == 48)
        {
            dashSpeed = 0f;
        }
        if(curFrame == 49)
        {
            if(earthDash == true)
            {
                //stuff
                curFrame = 1;
                curAction = new CurrentAction(FinishEarth2);
                earthDash = false;
            }

            if(lightDash == true)
            {
                //stuff
                curFrame = 1;
                curAction = new CurrentAction(FinishLight2);
                lightDash = false;
            }
            dashing = false;
        }
    }
    
    void Interact()//Interacts with an interactable up to 32 pixels in the direction we're facing
    {
        switch (myDirection)
        {
            case Direction.Up:
                Interactable.TriggerInteractions(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y + .49f, this.transform.position.y + .17f);
                Debug.Log("Interacting Up");
                break;
            case Direction.Down:
                Interactable.TriggerInteractions(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y - .17f, this.transform.position.y - .49f);
                Debug.Log("Interacting Down");
                break;
            case Direction.Right:
                Interactable.TriggerInteractions(this.transform.position.x + .17f, this.transform.position.x + .49f, this.transform.position.y + .16f, this.transform.position.y - .16f);
                Debug.Log("Interacting Right");
                break;
            case Direction.Left:
                Interactable.TriggerInteractions(this.transform.position.x - .49f, this.transform.position.x - .17f, this.transform.position.y + .16f, this.transform.position.y - .16f);
                Debug.Log("Interacting Left");
                break;

        }
    }

    void Attack1()
    {
        curFrame = 1;
        switch (myAttack1)
        {
            case ListAttack.Fire1:
                //Fire1();
                curAction = new CurrentAction(Fire1);
                break;
            case ListAttack.Light1:
                //Light1();
                curAction = new CurrentAction(Light1);
                break;
            case ListAttack.Void1:
                //Void1();
                curAction = new CurrentAction(Void1);
                break;
            case ListAttack.Earth1:
                //Earth1();
                curAction = new CurrentAction(Earth1);
                break;
            case ListAttack.Water1:
                //Water1();
                curAction = new CurrentAction(Water1);
                break;
            case ListAttack.Air1:
                //Air1();
                curAction = new CurrentAction(Air1);
                break;
            default:
                Debug.Log("Error in Attack1");
                break;

        }
    }

    void Attack2()
    {
        curFrame = 1;
        switch (myAttack2)
        {
            case ListAttack.Fire2:
                //Fire2();
                curAction = new CurrentAction(Fire2);
                break;
            case ListAttack.Light2:
                //Light2();
                curAction = new CurrentAction(Light2);
                break;
            case ListAttack.Void2:
                //Void2();
                curAction = new CurrentAction(Void2);
                break;
            case ListAttack.Earth2:
                //Earth2();
                curAction = new CurrentAction(Earth2);
                break;
            case ListAttack.Water2:
                //Water2();
                curAction = new CurrentAction(Water2);
                break;
            case ListAttack.Air2:
                //Air2();
                curAction = new CurrentAction(Air2);
                break;
            default:
                Debug.Log("Error in Attack2");
                break;

        }
    }

    void Attack3()
    {
       curFrame = 1;
        switch (myAttack3)
        {
            case ListAttack.Fire3:
                //Fire3();
                curAction = new CurrentAction(Fire3);
                break;
            case ListAttack.Light3:
                //Light3();
                curAction = new CurrentAction(Light3);
                break;
            case ListAttack.Void3:
                //Void3();
                curAction = new CurrentAction(Void3);
                break;
            case ListAttack.Earth3:
                //Earth3();
                curAction = new CurrentAction(Earth3);
                break;
            case ListAttack.Water3:
                //Water3();
                curAction = new CurrentAction(Water3);
                break;
            case ListAttack.Air3:
                //Air3();
                curAction = new CurrentAction(Air3);
                break;
            default:
                Debug.Log("Error in Attack3");
                break;

        }
    }

    void Attack4()
    {
        curFrame = 1;
        switch (myAttack4)
        {
            case ListAttack.Fire4:
                //Fire4();
                curAction = new CurrentAction(Fire4);
                break;
            case ListAttack.Light4:
                //Light4();
                curAction = new CurrentAction(Light4);
                break;
            case ListAttack.Void4:
                //Void4();
                curAction = new CurrentAction(Void4);
                break;
            case ListAttack.Earth4:
                //Earth4();
                curAction = new CurrentAction(Earth4);
                break;
            case ListAttack.Water4:
                //Water4();
                curAction = new CurrentAction(Water4);
                break;
            case ListAttack.Air4:
                //Air4();
                curAction = new CurrentAction(Air4);
                break;
            default:
                Debug.Log("Error in Attack4");
                break;

        }
    }

    void Fire1() //Fire explosion
    {
        //This isn't finished.
        Debug.Log("Fire1");
        //Fire explosion attack, ground targets
        //bounds declared for sprite facing up

        float range = 1.5f;

        //Clears the GTQueue, puts Fire1 on top of it, sets the GT range, then turns on groundtargetting
        //GTQueue.Clear();
        //GTQueue.Push("Fire1");
        GTQueue = new CurrentAction(FinishFire1);
        gtRange = range;
        groundTargetting = true;
        Debug.Log("Groundtargetting started. Range: " + gtRange);
        curAction = null;

    }

    void FinishFire1() //Finishing Fire explosion
    {
        int totalFrames = 14;

        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        if ((curFrame == 12))
        {
            //Make one square hitbox with heavy knockback, then two rectangles with medium knockback
            if (showHitboxen == true)
            {
                Debug.Log("Beginning the Fire1 Hitboxen");
                GameObject myHitbox1 = (GameObject)Instantiate(orangeHitbox);
                GameObject myHitbox2 = (GameObject)Instantiate(orangeHitbox);
                GameObject myHitbox3 = (GameObject)Instantiate(orangeHitbox);
                HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                HitboxScript myHitboxScript2 = myHitbox2.GetComponent<HitboxScript>();
                HitboxScript myHitboxScript3 = myHitbox3.GetComponent<HitboxScript>();
                myHitbox1.transform.localScale = new Vector3(3f, 3f, 0);
                myHitbox1.transform.position = new Vector3(curGTx, curGTy, 0);
                myHitboxScript1.framesTilGone = 200;
                myHitbox2.transform.localScale = new Vector3(2f, 4f, 0);
                myHitbox2.transform.position = new Vector3(curGTx, curGTy, 0);
                myHitboxScript2.framesTilGone = 200;
                myHitbox3.transform.localScale = new Vector3(4f, 2f, 0);
                myHitbox3.transform.position = new Vector3(curGTx, curGTy, 0);
                myHitboxScript3.framesTilGone = 200;
                Debug.Log("Fire1 Hitboxen should be visually placed");
            }

            Debug.Log("Beginning Fire1 Triggerhits");
            //hitbox 1's triggerhit
            Entity.TriggerHitsBuff(curGTx - .48f, curGTx + .48f, curGTy + .48f, curGTy - .48f, currentForm.dmg, "Fire", "Explosion", 5, .1f * currentForm.dmg, testKnockback * 3f, "Directional");
            //hitbox 2's triggerhit (vertical small)
            Entity.TriggerHitsBuff(curGTx - .32f, curGTx + .32f, curGTy + .64f, curGTy - .64f, currentForm.dmg, "Fire", "Explosion", 5, .1f * currentForm.dmg, testKnockback * 2f, "Directional");
            //hitbox 3's triggerhit (horizontal small)
            Entity.TriggerHitsBuff(curGTx - .64f, curGTx + .64f, curGTy + .32f, curGTy - .32f, currentForm.dmg, "Fire", "Explosion", 5, .1f * currentForm.dmg, testKnockback * 2f, "Directional");
            Debug.Log("Fire1's Triggerhits should be complete");

        }
        //Explosion
        //Add a high damage dot to this
        Debug.Log("Finishing Fire1");
        } 

    void Fire2() //Fireball. Projectile + Dot
    {
        int totalFrames = 11;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        if (curFrame == 6)
        {
            GameObject myFireball = (GameObject)Instantiate(fireball);
            FireballScript myFireballScript = myFireball.GetComponent<FireballScript>();
            myFireballScript.damage = currentForm.dmg;
            myFireballScript.isWhat = "Fireball";
            switch (myDirection)
            {
                case Direction.Up:
                    myFireballScript.direction = "Up";
                    myFireball.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .49f, 0);
                    break;
                case Direction.Down:
                    myFireballScript.direction = "Down";
                    myFireball.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - .49f, 0);
                    break;
                case Direction.Left:
                    myFireballScript.direction = "Left";
                    myFireball.transform.position = new Vector3(this.transform.position.x - .49f, this.transform.position.y, 0);
                    break;
                case Direction.Right:
                    myFireballScript.direction = "Right";
                    myFireball.transform.position = new Vector3(this.transform.position.x + .49f, this.transform.position.y, 0);
                    break;
                default:
                    break;
            }
            myFireballScript.directionSet = true;
        }

        //Make sure to spawn the Fireball, set it as a Fireball, set the Direction, and pass through Damage
    }

    void Fire3() //Ember. Tiny projectile. Grants a stacking fire damage buff when you cast it
    {
        //Spark. Tiny fire projectile. Grants a stacking fire damage buff.
        int totalFrames = 11;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        if (curFrame == 6)
        {
            GameObject myFireball = (GameObject)Instantiate(fireball);
            myFireball.transform.localScale = new Vector3(.5f, .5f, 0);
            FireballScript myFireballScript = myFireball.GetComponent<FireballScript>();
            myFireballScript.damage = currentForm.dmg;
            myFireballScript.isWhat = "Ember";
            switch (myDirection)
            {
                case Direction.Up:
                    myFireballScript.direction = "Up";
                    myFireball.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .49f, 0);
                    break;
                case Direction.Down:
                    myFireballScript.direction = "Down";
                    myFireball.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - .49f, 0);
                    break;
                case Direction.Left:
                    myFireballScript.direction = "Left";
                    myFireball.transform.position = new Vector3(this.transform.position.x - .49f, this.transform.position.y, 0);
                    break;
                case Direction.Right:
                    myFireballScript.direction = "Right";
                    myFireball.transform.position = new Vector3(this.transform.position.x + .49f, this.transform.position.y, 0);
                    break;
                default:
                    break;
            }
            myFireballScript.directionSet = true;

            LoadBuff("Ember", 10f, .1f * localPlayerData.fireLevel);
        }

        //Make sure to buff self, spawn the Fireball, shrink its transform, set it as an Ember, set the Direction, and pass through Damage
    }

    void Fire4() //Fire breath
    {
        //Not done
        //Fire breath. Holdable spirit dump for mass damage.
        int totalFrames = 23;

        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }


        switch (myDirection) {
            case Direction.Up:
                if ((curFrame == 6))
                {
                    if (showHitboxen == true)
                    { 
                        GameObject myHitbox1 = (GameObject)Instantiate(orangeHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1.5f, 1.0f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .33f, 0);
                        myHitboxScript1.framesTilGone = 5;
                    }
                    Entity.TriggerHits(this.transform.position.x - .24f, this.transform.position.x + .24f, this.transform.position.y + .49f, this.transform.position.y + .17f, 1, "Fire", 50, "Up");
                }
                if ((curFrame == 11))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(orangeHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1.25f, 1.25f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .37f, 0);
                        myHitboxScript1.framesTilGone = 5;
                    }
                    Entity.TriggerHits(this.transform.position.x - .20f, this.transform.position.x + .20f, this.transform.position.y + .57f, this.transform.position.y + .17f, 1, "Fire", 75, "Up");
                }
                if ((curFrame == 16))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(orangeHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, 1.5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .41f, 0);
                        myHitboxScript1.framesTilGone = 5;
                    }
                    Entity.TriggerHits(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y + .65f, this.transform.position.y + .17f, 1, "Fire", 100, "Up");
                }
                break;
            case Direction.Down:
                if ((curFrame == 6))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(orangeHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1.5f, 1.0f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - .33f, 0);
                        myHitboxScript1.framesTilGone = 5;
                    }
                    Entity.TriggerHits(this.transform.position.x - .24f, this.transform.position.x + .24f, this.transform.position.y - .17f, this.transform.position.y - .49f, 1, "Fire", 50, "Down");
                }
                if ((curFrame == 11))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(orangeHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1.25f, 1.25f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - .37f, 0);
                        myHitboxScript1.framesTilGone = 5;
                    }
                    Entity.TriggerHits(this.transform.position.x - .20f, this.transform.position.x + .20f, this.transform.position.y - .17f, this.transform.position.y - .57f , 1, "Fire", 75, "Down");
                }
                if ((curFrame == 16))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(orangeHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, 1.5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - .41f, 0);
                        myHitboxScript1.framesTilGone = 5;
                    }
                    Entity.TriggerHits(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y - .17f, this.transform.position.y - .65f, 1, "Fire", 100, "Down");
                }
                break;
            case Direction.Left:
                if ((curFrame == 6))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(orangeHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, 1.5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x - .33f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 5;
                    }
                    Entity.TriggerHits(this.transform.position.x - .49f, this.transform.position.x -.17f, this.transform.position.y + .24f, this.transform.position.y - .24f, 1, "Fire", 50, "Left");
                }
                if ((curFrame == 11))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(orangeHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1.25f, 1.25f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x - .37f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 5;
                    }
                    Entity.TriggerHits(this.transform.position.x - .57f, this.transform.position.x - .17f, this.transform.position.y + .20f, this.transform.position.y - .20f, 1, "Fire", 75, "Left");
                }
                if ((curFrame == 16))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(orangeHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1.5f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x - .41f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 5;
                    }
                    Entity.TriggerHits(this.transform.position.x - .65f, this.transform.position.x -.17f, this.transform.position.y + .16f, this.transform.position.y - .16f, 1, "Fire", 100, "Left");
                }
                break;
            case Direction.Right:
                if ((curFrame == 6))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(orangeHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, 1.5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x + .33f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 5;
                    }
                    Entity.TriggerHits(this.transform.position.x + .17f, this.transform.position.x + .49f, this.transform.position.y + .24f, this.transform.position.y - .24f, 1, "Fire", 50, "Right");
                }
                if ((curFrame == 11))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(orangeHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1.25f, 1.25f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x + .37f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 5;
                    }
                    Entity.TriggerHits(this.transform.position.x + .17f, this.transform.position.x + .57f, this.transform.position.y + .20f, this.transform.position.y - .20f, 1, "Fire", 75, "Right");
                }
                if ((curFrame == 16))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(orangeHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1.5f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x + .41f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 5;
                    }
                    Entity.TriggerHits(this.transform.position.x + .17f, this.transform.position.x + .65f, this.transform.position.y + .16f, this.transform.position.y - .16f, 1, "Fire", 100, "Right");
                }
                break;
            default:
                break;
        }
    }

    void Earth1() //Thud. PAWNCH
    {
        int totalFrames = 22;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        switch (myDirection)
        {
            case Direction.Up:
                if ((curFrame == 17))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(greenHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, .5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .25f, 0);
                        myHitboxScript1.framesTilGone = 5;
                    }
                    Entity.TriggerHits(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y + .33f, this.transform.position.y + .17f, 1, "Earth", 150, "Up");
                }
                break;
            case Direction.Down:
                if ((curFrame == 17))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(greenHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, .5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - .25f, 0);
                        myHitboxScript1.framesTilGone = 5;
                    }
                    Entity.TriggerHits(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y - .17f, this.transform.position.y - .33f, 1, "Earth", 150, "Down");
                }
                break;
            case Direction.Left:
                if ((curFrame == 17))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(greenHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(.5f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x - .25f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 5;
                    }
                    Entity.TriggerHits(this.transform.position.x - .33f, this.transform.position.x - .17f, this.transform.position.y + .16f, this.transform.position.y - .16f, 1, "Earth", 150, "Left");
                }
                break;
            case Direction.Right:
                if ((curFrame == 17))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(greenHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(.5f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x + .25f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 5;
                    }
                    Entity.TriggerHits(this.transform.position.x + .17f, this.transform.position.x + .33f, this.transform.position.y + .16f, this.transform.position.y - .16f, 1, "Earth", 150, "Right");
                }
                break;
            default:
                break;
        }
        Debug.Log("Earth1");
    }

    void Earth2() //Starts the Tackle Dash
    {
        earthDash = true;
        NextAction("Dash");
    }

    void FinishEarth2() //Roots on hit
    {
        Debug.Log("Finishing Earth2");
        /*int startupFrames = 1;
        int activeFrames = 1;
        int endingFrames = 5;*/
        int totalFrames = 7;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        if ((curFrame == 2))
        {
            //Make one square hitbox with heavy knockback, then two rectangles with medium knockback
            if (showHitboxen == true)
            {
                Debug.Log("Beginning the Earth2 Hitboxen");
                GameObject myHitbox1 = (GameObject)Instantiate(greenHitbox);
                GameObject myHitbox2 = (GameObject)Instantiate(greenHitbox);
                GameObject myHitbox3 = (GameObject)Instantiate(greenHitbox);
                HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                HitboxScript myHitboxScript2 = myHitbox2.GetComponent<HitboxScript>();
                HitboxScript myHitboxScript3 = myHitbox3.GetComponent<HitboxScript>();
                myHitbox1.transform.localScale = new Vector3(2f, 2f, 0);
                myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                myHitboxScript1.framesTilGone = 60;
                myHitbox2.transform.localScale = new Vector3(1.25f, 2.25f, 0);
                myHitbox2.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                myHitboxScript2.framesTilGone = 60;
                myHitbox3.transform.localScale = new Vector3(2.25f, 1.25f, 0);
                myHitbox3.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                myHitboxScript3.framesTilGone = 60;
                Debug.Log("Earth2 Hitboxen should be visually placed");
            }

            invulTimer = 2;
            Debug.Log("Beginning Earth2 Triggerhits");
            //hitbox 1's triggerhit
            Entity.TriggerHitsBuff(this.transform.position.x - .32f, this.transform.position.x + .32f, this.transform.position.y + .32f, this.transform.position.y - .32f, 0, "Earth", "Root", localPlayerData.earthLevel, 0, 0, "Directional");
            //hitbox 2's triggerhit (vertical small)
            Entity.TriggerHitsBuff(this.transform.position.x - .20f, this.transform.position.x + .20f, this.transform.position.y + .40f, this.transform.position.y - .40f, 0, "Earth", "Root", localPlayerData.earthLevel, 0, 0, "Directional");
            //hitbox 3's triggerhit (horizontal small)
            Entity.TriggerHitsBuff(this.transform.position.x - .40f, this.transform.position.x + .40f, this.transform.position.y + .20f, this.transform.position.y - .20f, 0, "Earth", "Root", localPlayerData.earthLevel, 0, 0, "Directional");
            Debug.Log("Earth2's Triggerhits should be complete");

        }
    }

    void Earth3() //Earthquake. PBAE knockback
    {
        //When you code this, make it circular around you, but throw a *tiny* hitbox that will give you immunity in the first lines
        /*int startupFrames = 20;
        int activeFrames = 2;
        int endingFrames = 2;*/
        int totalFrames = 24;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        if ((curFrame == 22))
        {
            //Make one square hitbox with heavy knockback, then two rectangles with medium knockback
            if (showHitboxen == true)
            {
                Debug.Log("Beginning the Earth3 Hitboxen");
                GameObject myHitbox1 = (GameObject)Instantiate(redHitbox);
                GameObject myHitbox2 = (GameObject)Instantiate(redHitbox);
                GameObject myHitbox3 = (GameObject)Instantiate(redHitbox);
                HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                HitboxScript myHitboxScript2 = myHitbox2.GetComponent<HitboxScript>();
                HitboxScript myHitboxScript3 = myHitbox3.GetComponent<HitboxScript>();
                myHitbox1.transform.localScale = new Vector3(3f, 3f, 0);
                myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                myHitboxScript1.framesTilGone = 20;
                myHitbox2.transform.localScale = new Vector3(2f, 4f, 0);
                myHitbox2.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                myHitboxScript2.framesTilGone = 20;
                myHitbox3.transform.localScale = new Vector3(4f, 2f, 0);
                myHitbox3.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                myHitboxScript3.framesTilGone = 20;
                Debug.Log("Earth3 Hitboxen should be visually placed");
            }

            invulTimer = 2;
            Debug.Log("Beginning Earth3 Triggerhits");
            //hitbox 1's triggerhit
            Entity.TriggerHits(this.transform.position.x - .48f, this.transform.position.x + .48f, this.transform.position.y + .48f, this.transform.position.y - .48f, 0, "Earth", testKnockback * 2f, "Directional");
            //hitbox 2's triggerhit (vertical small)
            Entity.TriggerHits(this.transform.position.x - .32f, this.transform.position.x + .32f, this.transform.position.y + .64f, this.transform.position.y - .64f, 0, "Earth", testKnockback * 1f, "Directional");
            //hitbox 3's triggerhit (horizontal small)
            Entity.TriggerHits(this.transform.position.x - .64f, this.transform.position.x + .64f, this.transform.position.y + .32f, this.transform.position.y - .32f, 0, "Earth", testKnockback * 1f, "Directional");
            Debug.Log("Earth3's Triggerhits should be complete");

        }
    }

    void Earth4() //Endure. Self resist buff and speed debuff
    {
        //Endure. Self buff that ups resists and lowers movement speed.
        /*int startupFrames = 1;
        int activeFrames = 1;
        int endingFrames = 5;*/
        int totalFrames = 7;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        if (curFrame == 2)
        {
            LoadBuff("Endure", 10f, localPlayerData.earthLevel);
            //do stuff
        }
    }

    void Light1() //Dispersion. Frontal Arc. Focus debuff + Damage.
    {
        /*int startupFrames = 5;
        int activeFrames = 9;
        int endingFrames = 10;*/
        int totalFrames = 24;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }


        switch (myDirection)
        {
            case Direction.Up:
                if ((curFrame == 6))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, .5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .25f, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y + .33f, this.transform.position.y + .17f, 1, "Light", "Dispersion", 5f, .1f * localPlayerData.lightLevel, 50, "Up");
                }
                if ((curFrame == 9))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1.5f, .5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .41f, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x - .24f, this.transform.position.x + .24f, this.transform.position.y + .49f, this.transform.position.y + .33f, 1, "Light", "Dispersion", 5f, .1f * localPlayerData.lightLevel, 50, "Up");
                }
                if ((curFrame == 12))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(2f, .5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .57f, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x - .32f, this.transform.position.x + .32f, this.transform.position.y + .65f, this.transform.position.y + .49f, 1, "Light", "Dispersion", 5f, .1f * localPlayerData.lightLevel, 50, "Up");
                }
                break;
            case Direction.Down:
                if ((curFrame == 6))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, .5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - .25f, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y - .17f, this.transform.position.y - .33f, 1, "Light", "Dispersion", 5f, .1f * localPlayerData.lightLevel, 50, "Down");
                }
                if ((curFrame == 9))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1.5f, .5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - .41f, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x - .24f, this.transform.position.x + .24f, this.transform.position.y - .33f, this.transform.position.y - .49f, 1, "Light", "Dispersion", 5f, .1f * localPlayerData.lightLevel, 50, "Down");
                }
                if ((curFrame == 12))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(2f, .5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - .57f, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x - .32f, this.transform.position.x + .32f, this.transform.position.y - .49f, this.transform.position.y - .65f, 1, "Light", "Dispersion", 5f, .1f * localPlayerData.lightLevel, 50, "Down");
                }
                break;
            case Direction.Left:
                if ((curFrame == 6))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(.5f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x - .25f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x - .33f, this.transform.position.x - .17f, this.transform.position.y + .16f, this.transform.position.y - .16f, 1, "Light", "Dispersion", 5f, .1f * localPlayerData.lightLevel, 50, "Left");
                }
                if ((curFrame == 9))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(.5f, 1.5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x - .41f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x - .49f, this.transform.position.x - .33f, this.transform.position.y + .24f, this.transform.position.y - .24f, 1, "Light", "Dispersion", 5f, .1f * localPlayerData.lightLevel, 50, "Left");
                }
                if ((curFrame == 12))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(.5f, 2f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x - .57f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x - .65f, this.transform.position.x - .49f, this.transform.position.y + .32f, this.transform.position.y - .32f, 1, "Light", "Dispersion", 5f, .1f * localPlayerData.lightLevel, 50, "Left");
                }
                break;
            case Direction.Right:
                if ((curFrame == 6))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(.5f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x + .25f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x + .17f, this.transform.position.x + .33f, this.transform.position.y + .16f, this.transform.position.y - .16f, 1, "Light", "Dispersion", 5f, .1f * localPlayerData.lightLevel, 50, "Right");
                }
                if ((curFrame == 9))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(.5f, 1.5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x + .41f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x + .33f, this.transform.position.x + .49f, this.transform.position.y + .24f, this.transform.position.y - .24f, 1, "Light", "Dispersion", 5f, .1f * localPlayerData.lightLevel, 50, "Right");
                }
                if ((curFrame == 12))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(.5f, 2f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x + .57f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x + .49f, this.transform.position.x + .65f, this.transform.position.y + .32f, this.transform.position.y - .32f, 1, "Light", "Dispersion", 5f, .1f * localPlayerData.lightLevel, 50, "Right");
                }
                break;
            default:
                break;
        }
    }

    void Light2() //Starts the Dash
    {
        lightDash = true;
        NextAction("Dash");
    }

    void FinishLight2() //Apply the hitbox to the Light Charge
    {
        //This should extend a hitbox behind the dash
        Debug.Log("Finishing Light2");
        /*int startupFrames = 1;
        int activeFrames = 1;
        int endingFrames = 10;*/
        int totalFrames = 12;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }


        switch (dashDir)
        {
            case "Up":
                if ((curFrame == 2))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, 2f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - .49f, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHits(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y - .81f, this.transform.position.y - .17f, 1, "Light", 75, "Directional");
                }
                break;
            case "Down":
                if ((curFrame == 2))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, 2f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .49f, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHits(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y + .17f, this.transform.position.y + .81f, 1, "Light", 75, "Directional");
                }
                break;
            case "Left":
                if ((curFrame == 2))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(2f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x + .49f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHits(this.transform.position.x + .81f, this.transform.position.x + .17f, this.transform.position.y + .16f, this.transform.position.y - .16f, 1, "Light", 75, "Directional");
                }
                break;
            case "Right":
                if ((curFrame == 2))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(2f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x - .49f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHits(this.transform.position.x - .17f, this.transform.position.x - .81f, this.transform.position.y + .16f, this.transform.position.y - .16f, 1, "Light",  75, "Directional");
                }
                break;
            default:
                break;
        }
    }

    void Light3() //Fragment. Lowers targets' resistances
    {
        /*int startupFrames = 4;
        int activeFrames = 3;
        int endingFrames = 15;*/
        int totalFrames = 22;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }


        switch (myDirection)
        {
            case Direction.Up:
                if ((curFrame == 5))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .33f, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y + .49f, this.transform.position.y + .17f, 1, "Light", "Fragment", 15f, .25f * localPlayerData.lightLevel, 50, "Up");
                }
                break;
            case Direction.Down:
                if ((curFrame == 5))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - .33f, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y - .17f, this.transform.position.y - .49f, 1, "Light", "Fragment", 15f, .25f * localPlayerData.lightLevel, 50, "Down");
                }
                break;
            case Direction.Left:
                if ((curFrame == 5))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x - .25f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x - .49f, this.transform.position.x - .17f, this.transform.position.y + .16f, this.transform.position.y - .16f, 1, "Light", "Fragment", 15f, .25f * localPlayerData.lightLevel, 50, "Left");
                }
                break;
            case Direction.Right:
                if ((curFrame == 5))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x + .25f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x + .17f, this.transform.position.x + .49f, this.transform.position.y + .16f, this.transform.position.y - .16f, 1, "Light", "Fragment", 15f, .25f * localPlayerData.lightLevel, 50, "Right");
                }
                break;
            default:
                break;
        }
    }

    void Light4() //Piercing Light. Very quick, short, frontal that has a tiny stacking vitality debuff
    {
        /*int startupFrames = 4;
        int activeFrames = 3;
        int endingFrames = 15;*/
        int totalFrames = 22;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }


        switch (myDirection)
        {
            case Direction.Up:
                if ((curFrame == 5))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(.25f, 2f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .49f, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x - .04f, this.transform.position.x + .04f, this.transform.position.y + .81f, this.transform.position.y + .17f, 1, "Light", "Piercing Light", 8f, .1f * localPlayerData.lightLevel, 25, "Up");
                }
                break;
            case Direction.Down:
                if ((curFrame == 5))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(.25f, 2f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - .49f, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x - .04f, this.transform.position.x + .04f, this.transform.position.y - .17f, this.transform.position.y - .81f, 1, "Light", "Piercing Light", 8f, .1f * localPlayerData.lightLevel, 25, "Down");
                }
                break;
            case Direction.Left:
                if ((curFrame == 5))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(2f, .25f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x - .49f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x - .81f, this.transform.position.x - .17f, this.transform.position.y + .04f, this.transform.position.y - .04f, 1, "Light", "Piercing Light", 8f, .1f * localPlayerData.lightLevel, 25, "Left");
                }
                break;
            case Direction.Right:
                if ((curFrame == 5))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(yellowHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(2f, .25f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x + .49f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHitsBuff(this.transform.position.x + .17f, this.transform.position.x + .81f, this.transform.position.y + .04f, this.transform.position.y - .04f, 1, "Light", "Piercing Light", 8f, .1f * localPlayerData.lightLevel, 25, "Right");
                }
                break;
            default:
                break;
        }
    }

    void Void1() //Void bomb. PBAE void nuke.
    {
        /*int startupFrames = 5;
        int activeFrames = 3;
        int endingFrames = 20;*/
        int totalFrames = 28;

        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        if ((curFrame == 6))
        {
            //Make one square hitbox with heavy knockback, then two rectangles with medium knockback
            if (showHitboxen == true)
            {
                Debug.Log("Beginning the Void3 Hitboxen");
                GameObject myHitbox1 = (GameObject)Instantiate(purpleHitbox);
                GameObject myHitbox2 = (GameObject)Instantiate(purpleHitbox);
                GameObject myHitbox3 = (GameObject)Instantiate(purpleHitbox);
                HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                HitboxScript myHitboxScript2 = myHitbox2.GetComponent<HitboxScript>();
                HitboxScript myHitboxScript3 = myHitbox3.GetComponent<HitboxScript>();
                myHitbox1.transform.localScale = new Vector3(3f, 3f, 0);
                myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                myHitboxScript1.framesTilGone = 23;
                myHitbox2.transform.localScale = new Vector3(2f, 4f, 0);
                myHitbox2.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                myHitboxScript2.framesTilGone = 23;
                myHitbox3.transform.localScale = new Vector3(4f, 2f, 0);
                myHitbox3.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                myHitboxScript3.framesTilGone = 23;
                Debug.Log("Void3 Hitboxen should be visually placed");
            }

            Debug.Log("Beginning Void3 Triggerhits");
            //hitbox 1's triggerhit
            Entity.TriggerHits(this.transform.position.x - .48f, this.transform.position.x + .48f, this.transform.position.y + .48f, this.transform.position.y - .48f, 0, "Void", 50, "Directional");
            //hitbox 2's triggerhit (vertical small)
            Entity.TriggerHits(this.transform.position.x - .32f, this.transform.position.x + .32f, this.transform.position.y + .64f, this.transform.position.y - .64f, 0, "Void", 25, "Directional");
            //hitbox 3's triggerhit (horizontal small)
            Entity.TriggerHits(this.transform.position.x - .64f, this.transform.position.x + .64f, this.transform.position.y + .32f, this.transform.position.y - .32f, 0, "Void", 25, "Directional");
            Debug.Log("Void3's Triggerhits should be complete");

        }
        //This isn't finished
        Debug.Log("Void1");
        
    }

    void Void2() //Claw. Short range melee spam
    {
        /*int startupFrames = 3;
        int activeFrames = 3;
        int endingFrames = 5;*/
        int totalFrames = 11;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        switch (myDirection)
        {
            case Direction.Up:
                if ((curFrame == 4))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(purpleHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, .5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .25f, 0);
                        myHitboxScript1.framesTilGone = 8;
                    }
                    Entity.TriggerHits(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y + .33f, this.transform.position.y + .17f, 1, "Void", 25, "Up");
                }
                break;
            case Direction.Down:
                if ((curFrame == 4))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(purpleHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, .5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - .25f, 0);
                        myHitboxScript1.framesTilGone = 8;
                    }
                    Entity.TriggerHits(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y - .17f, this.transform.position.y - .33f, 1, "Void", 25, "Down");
                }
                break;
            case Direction.Left:
                if ((curFrame == 4))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(purpleHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(.5f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x - .25f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 8;
                    }
                    Entity.TriggerHits(this.transform.position.x - .33f, this.transform.position.x - .17f, this.transform.position.y + .16f, this.transform.position.y - .16f, 1, "Void", 25, "Left");
                }
                break;
            case Direction.Right:
                if ((curFrame == 4))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(purpleHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(.5f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x + .25f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 8;
                    }
                    Entity.TriggerHits(this.transform.position.x + .17f, this.transform.position.x + .33f, this.transform.position.y + .16f, this.transform.position.y - .16f, 1, "Void", 25, "Right");
                }
                break;
            default:
                break;
        }
        //Claw. Short range melee spam.
    }

    void Void3() //Gravity Well
    {
        //Gravity Well with reverse Knockback (for stacking). Ground Targetted.

        //This isn't finished.
        Debug.Log("Void3");

        float range = 1.5f;

        GTQueue = new CurrentAction(FinishVoid3);
        gtRange = range;
        groundTargetting = true;
        Debug.Log("Groundtargetting started. Range: " + gtRange);
        curAction = null;

    }

    void FinishVoid3() //Finishing Gravity Well
    {
        /*int startupFrames = 10;
        int activeFrames = 2;
        int endingFrames = 2;*/
        int totalFrames = 14;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        if ((curFrame == 12))
        {
            //Make one square hitbox with heavy knockback, then two rectangles with medium knockback
            if (showHitboxen == true)
            {
                Debug.Log("Beginning the Void3 Hitboxen");
                GameObject myHitbox1 = (GameObject)Instantiate(purpleHitbox);
                GameObject myHitbox2 = (GameObject)Instantiate(purpleHitbox);
                GameObject myHitbox3 = (GameObject)Instantiate(purpleHitbox);
                HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                HitboxScript myHitboxScript2 = myHitbox2.GetComponent<HitboxScript>();
                HitboxScript myHitboxScript3 = myHitbox3.GetComponent<HitboxScript>();
                myHitbox1.transform.localScale = new Vector3(3f, 3f, 0);
                myHitbox1.transform.position = new Vector3(curGTx, curGTy, 0);
                myHitboxScript1.framesTilGone = 25;
                myHitbox2.transform.localScale = new Vector3(2f, 4f, 0);
                myHitbox2.transform.position = new Vector3(curGTx, curGTy, 0);
                myHitboxScript2.framesTilGone = 25;
                myHitbox3.transform.localScale = new Vector3(4f, 2f, 0);
                myHitbox3.transform.position = new Vector3(curGTx, curGTy, 0);
                myHitboxScript3.framesTilGone = 25;
                Debug.Log("Void3 Hitboxen should be visually placed");
            }

            Debug.Log("Beginning Void3 Triggerhits");
            //hitbox 1's triggerhit
            Entity.TriggerHits(curGTx - .48f, curGTx + .48f, curGTy + .48f, curGTy - .48f, 0, "Void", testKnockback*-2f, "Directional");
            //hitbox 2's triggerhit (vertical small)
            Entity.TriggerHits(curGTx - .32f, curGTx + .32f, curGTy + .64f, curGTy - .64f, 0, "Void", testKnockback*-1f, "Directional");
            //hitbox 3's triggerhit (horizontal small)
            Entity.TriggerHits(curGTx - .64f, curGTx + .64f, curGTy + .32f, curGTy - .32f, 0, "Void", testKnockback*-1f, "Directional");
            Debug.Log("Void3's Triggerhits should be complete");

        }
    }

    void Void4() //Gravity Bomb. GTAOE Root.
    {
        Debug.Log("Void4");

        float range = 1.5f;

        GTQueue = new CurrentAction(FinishVoid4);
        gtRange = range;
        groundTargetting = true;
        Debug.Log("Groundtargetting started. Range: " + gtRange);
        curAction = null;
        //Gravity Bomb: Aoe root. GT. Cooldown.
    }

    void FinishVoid4() //Finishing Gravity Bomb
    {
        /*int startupFrames = 10;
        int activeFrames = 2;
        int endingFrames = 2;*/
        int totalFrames = 14;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        if ((curFrame == 12))
        {
            //Make one square hitbox with heavy knockback, then two rectangles with medium knockback
            if (showHitboxen == true)
            {
                Debug.Log("Beginning the Void3 Hitboxen");
                GameObject myHitbox1 = (GameObject)Instantiate(redHitbox);
                GameObject myHitbox2 = (GameObject)Instantiate(redHitbox);
                GameObject myHitbox3 = (GameObject)Instantiate(redHitbox);
                HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                HitboxScript myHitboxScript2 = myHitbox2.GetComponent<HitboxScript>();
                HitboxScript myHitboxScript3 = myHitbox3.GetComponent<HitboxScript>();
                myHitbox1.transform.localScale = new Vector3(3f, 3f, 0);
                myHitbox1.transform.position = new Vector3(curGTx, curGTy, 0);
                myHitboxScript1.framesTilGone = 25;
                myHitbox2.transform.localScale = new Vector3(2f, 4f, 0);
                myHitbox2.transform.position = new Vector3(curGTx, curGTy, 0);
                myHitboxScript2.framesTilGone = 25;
                myHitbox3.transform.localScale = new Vector3(4f, 2f, 0);
                myHitbox3.transform.position = new Vector3(curGTx, curGTy, 0);
                myHitboxScript3.framesTilGone = 25;
                Debug.Log("Void4 Hitboxen should be visually placed");
            }

            Debug.Log("Beginning Void4 Triggerhits");
            //hitbox 1's triggerhit
            Entity.TriggerHitsBuff(curGTx - .48f, curGTx + .48f, curGTy + .48f, curGTy - .48f, 0, "Void", "Root", 3f, 0, 0, "Directional");
            //hitbox 2's triggerhit (vertical small)
            Entity.TriggerHitsBuff(curGTx - .32f, curGTx + .32f, curGTy + .64f, curGTy - .64f, 0, "Void", "Root", 3f, 0, 0, "Directional");
            //hitbox 3's triggerhit (horizontal small)
            Entity.TriggerHitsBuff(curGTx - .64f, curGTx + .64f, curGTy + .32f, curGTy - .32f, 0, "Void", "Root", 3f, 0, 0, "Directional");
            Debug.Log("Void4's Triggerhits should be complete");
        }
    }

    void Water1() //Geyser. Kamehameha.
    {
        /*int startupFrames = 3;
        int activeFrames = 9;
        int endingFrames = 10;*/
        int totalFrames = 22;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        switch (myDirection)
        {
            case Direction.Up:
                if(curFrame == 4)
                //if ((curActiveFrame >= 1) && (curActiveFrame < 4))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(indigoHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, 1.0f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .33f, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHits(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y + .49f, this.transform.position.y + .17f, 1, "Water", 100, "Up");
                }
                //if ((curActiveFrame >= 4) && (curActiveFrame < 7))
                if(curFrame == 7)
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(indigoHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, 2f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .49f, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHits(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y + .81f, this.transform.position.y + .17f, 1, "Water", 50, "Up");
                }
                //if ((curActiveFrame >= 7) && (curActiveFrame < 10))
                if (curFrame == 10)
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(indigoHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, 3f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .65f, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHits(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y + 1.13f, this.transform.position.y + .17f, 1, "Water", 25, "Up");
                }
                break;
            case Direction.Down:
                //if ((curActiveFrame >= 1) && (curActiveFrame < 4))
                if(curFrame == 4)
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(indigoHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, 1.0f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - .33f, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHits(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y - .17f, this.transform.position.y - .49f, 1, "Water", 100, "Down");
                }
                //if ((curActiveFrame >= 4) && (curActiveFrame < 7))
                if(curFrame == 7)
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(indigoHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, 2f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - .49f, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHits(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y - .17f, this.transform.position.y - .81f, 1, "Water", 50, "Down");
                }
                //if ((curActiveFrame >= 7) && (curActiveFrame < 10))
                if(curFrame == 10)
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(indigoHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, 3f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - .65f, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHits(this.transform.position.x - .16f, this.transform.position.x + .16f, this.transform.position.y - .17f, this.transform.position.y - 1.13f, 1, "Water", 25, "Down");
                }
                break;
            case Direction.Left:
                //if ((curActiveFrame >= 1) && (curActiveFrame < 4))
                if(curFrame == 4)
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(indigoHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x - .33f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHits(this.transform.position.x - .49f, this.transform.position.x - .17f, this.transform.position.y + .16f, this.transform.position.y - .16f, 1, "Water", 100, "Left");
                }
                //if ((curActiveFrame >= 4) && (curActiveFrame < 7))
                if(curFrame == 7)
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(indigoHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(2f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x - .49f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHits(this.transform.position.x - .81f, this.transform.position.x - .17f, this.transform.position.y + .16f, this.transform.position.y - .16f, 1, "Water", 50, "Left");
                }
                //if ((curActiveFrame >= 7) && (curActiveFrame < 10))
                if(curFrame == 10)
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(indigoHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(3f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x - .65f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHits(this.transform.position.x - 1.13f, this.transform.position.x - .17f, this.transform.position.y + .16f, this.transform.position.y - .16f, 1, "Water", 25, "Left");
                }
                break;
            case Direction.Right:
                //if ((curActiveFrame >= 1) && (curActiveFrame < 4))
                if(curFrame == 4)
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(indigoHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(1f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x + .33f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHits(this.transform.position.x + .17f, this.transform.position.x + .49f, this.transform.position.y + .16f, this.transform.position.y - .16f, 1, "Water", 100, "Right");
                }
                //if ((curActiveFrame >= 4) && (curActiveFrame < 7))
                if(curFrame == 7)
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(indigoHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(2f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x + .49f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHits(this.transform.position.x + .17f, this.transform.position.x + .81f, this.transform.position.y + .16f, this.transform.position.y - .16f, 1, "Water", 50, "Right");
                }
                //if ((curActiveFrame >= 7) && (curActiveFrame < 10))
                if(curFrame == 10)
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(indigoHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(3f, 1f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x + .65f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 3;
                    }
                    Entity.TriggerHits(this.transform.position.x + .17f, this.transform.position.x + 1.13f, this.transform.position.y + .16f, this.transform.position.y - .16f, 1, "Water", 25, "Right");
                }
                break;
            default:
                break;
        }
    }

    void Water2() //Healing Rain. Aoe Vitality Buff.
    {
        /*int startupFrames = 3;
        int activeFrames = 2;
        int endingFrames = 5;*/
        int totalFrames = 10;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        if ((curFrame == 4))
        {
            //Make one square hitbox with heavy knockback, then two rectangles with medium knockback
            if (showHitboxen == true)
            {
                Debug.Log("Beginning the Void3 Hitboxen");
                GameObject myHitbox1 = (GameObject)Instantiate(indigoHitbox);
                GameObject myHitbox2 = (GameObject)Instantiate(indigoHitbox);
                GameObject myHitbox3 = (GameObject)Instantiate(indigoHitbox);
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
                Debug.Log("Water2 Hitboxen should be visually placed");
            }

            Debug.Log("Beginning Void3 Triggerhits");
            //hitbox 1's triggerhit
            Entity.TriggerHitsBuff(this.transform.position.x - .48f, this.transform.position.x + .48f, this.transform.position.y + .48f, this.transform.position.y - .48f, 0, "Water", "Rain", localPlayerData.waterLevel * 3, currentForm.dmg, 0, "Directional");
            //hitbox 2's triggerhit (vertical small)
            Entity.TriggerHitsBuff(this.transform.position.x - .32f, this.transform.position.x + .32f, this.transform.position.y + .64f, this.transform.position.y - .64f, 0, "Water", "Rain", localPlayerData.waterLevel * 3, currentForm.dmg, 0, "Directional");
            //hitbox 3's triggerhit (horizontal small)
            Entity.TriggerHitsBuff(this.transform.position.x - .64f, this.transform.position.x + .64f, this.transform.position.y + .32f, this.transform.position.y - .32f, 0, "Water", "Rain", localPlayerData.waterLevel * 3, currentForm.dmg, 0, "Directional");
            Debug.Log("Water2's Triggerhits should be complete");

        }
        //Healing rain. AoE vitality buff.
    }

    void Water3() //Meditation. Self Focus buff.
    {
        /*int startupFrames = 5;
        int activeFrames = 1;
        int endingFrames = 2;*/
        int totalFrames = 8;

        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        if (curFrame == 6)
        {
            //do stuff
            LoadBuff("Meditation", 15, localPlayerData.waterLevel * .5f);
        }
        //Meditation. Self-focus buff.
    }

    void Water4() //Purge. Debuff purger.
    {
        /*int startupFrames = 1;
        int activeFrames = 1;
        int endingFrames = 10;*/
        int totalFrames = 12;

        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        if (curFrame == 2)
        {
            //do stuff
            LoadBuff("Purge", 5f, 0f);
        }
        //Purge. Removes buffs until it's all that's left, then removes itself.
    }

    void Air1() //Damage Mine. Set up to 3, then detonate with Lightning.
    {
        /*int startupFrames = 5;
        int activeFrames = 1;
        int endingFrames = 3;*/
        int totalFrames = 9;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        //Make an array of GameObjects in Player
        //Load the mines to that
        //Then detonate them all by calling to them and nulling the array out

        if(curFrame == 6)
        {
            if(mines[mineCounter] == null)
            {
                Debug.Log("mineCounter equal to: " + mineCounter + ". Spawning mine " + mineCounter);
                GameObject myAirMine = (GameObject)Instantiate(airMine);
                mines[mineCounter] = myAirMine;
                myAirMine.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
            }

            if(mineCounter < localPlayerData.airLevel - 1)
            {
                Debug.Log("Incrementing mineCounter from " + mineCounter + " to " + mineCounter + 1);
                mineCounter++;
            } else
            {
                Debug.Log("Setting mineCounter to 0");
                mineCounter = 0;
            }
        }
    }

    void Air2() //Stun Mine.
    {
        //Make this single-placement with no explosion but a trigger enter
        /*int startupFrames = 5;
        int activeFrames = 1;
        int endingFrames = 3;*/
        int totalFrames = 9;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        if (curFrame == 6)
        {
            Debug.Log("Spawning StunMine");
            if(myStunMine != null)
            {
                Debug.Log("myStunMine != null");
                StunMineScript myStunScript = myStunMine.GetComponent<StunMineScript>();
                myStunScript.Deactivate();
            }
            GameObject mySpawningStunMine = (GameObject)Instantiate(stunMine);
            myStunMine = mySpawningStunMine;
            myStunMine.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
        }
    }

    void Air3() //Windrip. PBAE snare + personal speed boost.
    {
        /*int startupFrames = 10;
        int activeFrames = 2;
        int endingFrames = 2;*/
        int totalFrames = 14;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        if ((curFrame == 12))
        {
            //Make one square hitbox with heavy knockback, then two rectangles with medium knockback
            if (showHitboxen == true)
            {
                Debug.Log("Beginning the Air3 Hitboxen");
                GameObject myHitbox1 = (GameObject)Instantiate(blueHitbox);
                GameObject myHitbox2 = (GameObject)Instantiate(blueHitbox);
                GameObject myHitbox3 = (GameObject)Instantiate(blueHitbox);
                HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                HitboxScript myHitboxScript2 = myHitbox2.GetComponent<HitboxScript>();
                HitboxScript myHitboxScript3 = myHitbox3.GetComponent<HitboxScript>();
                myHitbox1.transform.localScale = new Vector3(3f, 3f, 0);
                myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                myHitboxScript1.framesTilGone = 60;
                myHitbox2.transform.localScale = new Vector3(2f, 4f, 0);
                myHitbox2.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                myHitboxScript2.framesTilGone = 60;
                myHitbox3.transform.localScale = new Vector3(4f, 2f, 0);
                myHitbox3.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                myHitboxScript3.framesTilGone = 60;
                Debug.Log("Air3 Hitboxen should be visually placed");
            }

            LoadBuff("Windrip", 1.0f * localPlayerData.airLevel, .5f * localPlayerData.airLevel);
            invulTimer = 2;
            Debug.Log("Beginning Air3 Triggerhits");
            //hitbox 1's triggerhit
            Entity.TriggerHitsBuff(this.transform.position.x - .48f, this.transform.position.x + .48f, this.transform.position.y + .48f, this.transform.position.y - .48f, 0, "Air", "Snare", 1f * localPlayerData.airLevel, .1f * localPlayerData.airLevel, 0, "Directional");
            //hitbox 2's triggerhit (vertical small)
            Entity.TriggerHitsBuff(this.transform.position.x - .32f, this.transform.position.x + .32f, this.transform.position.y + .64f, this.transform.position.y - .64f, 0, "Air", "Snare", 1f * localPlayerData.airLevel, .1f * localPlayerData.airLevel, 0, "Directional");
            //hitbox 3's triggerhit (horizontal small)
            Entity.TriggerHitsBuff(this.transform.position.x - .64f, this.transform.position.x + .64f, this.transform.position.y + .32f, this.transform.position.y - .32f, 0, "Air", "Snare", 1f * localPlayerData.airLevel, .1f * localPlayerData.airLevel, 0, "Directional");
            Debug.Log("Air3's Triggerhits should be complete");

        }
    }

    void Air4() //Lightning. High-spirit nuke that sets off mines.
    {
        /*int startupFrames = 2;
        int activeFrames = 4;
        int endingFrames = 12;*/
        int totalFrames = 18;
        if ((curFrame != 0))
        {
            FrameAdvance(totalFrames);
        }

        if(curFrame == 3)
        {
            DetonateMines();
        }

        switch (myDirection)
        {
            case Direction.Up:
                if ((curFrame == 3))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(blueHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(.5f, 2.5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .65f, 0);
                        myHitboxScript1.framesTilGone = 10;
                    }
                    Entity.TriggerHits(this.transform.position.x - .08f, this.transform.position.x + .08f, this.transform.position.y + .97f, this.transform.position.y + .17f, 1, "Air", 100, "Directional");
                }
               break;
            case Direction.Down:
                if ((curFrame == 3))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(blueHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(.5f, 2.5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - .65f, 0);
                        myHitboxScript1.framesTilGone = 10;
                    }
                    Entity.TriggerHits(this.transform.position.x - .08f, this.transform.position.x + .08f, this.transform.position.y - .17f, this.transform.position.y - .97f, 1, "Air", 100, "Directional");
                }                
                break;
            case Direction.Left:
                if ((curFrame == 3))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(blueHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(2.5f, .5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x - .65f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 10;
                    }
                    Entity.TriggerHits(this.transform.position.x - .97f, this.transform.position.x - .17f, this.transform.position.y + .08f, this.transform.position.y - .08f, 1, "Air", 100, "Directional");
                }
                break;
            case Direction.Right:
                if ((curFrame == 3))
                {
                    if (showHitboxen == true)
                    {
                        GameObject myHitbox1 = (GameObject)Instantiate(blueHitbox);
                        HitboxScript myHitboxScript1 = myHitbox1.GetComponent<HitboxScript>();
                        myHitbox1.transform.localScale = new Vector3(2.5f, .5f, 0);
                        myHitbox1.transform.position = new Vector3(this.transform.position.x + .65f, this.transform.position.y, 0);
                        myHitboxScript1.framesTilGone = 10;
                    }
                    Entity.TriggerHits(this.transform.position.x + .17f, this.transform.position.x + .97f, this.transform.position.y + .08f, this.transform.position.y - .08f, 1, "Air", 100, "Directional");
                }
                break;
            default:
                break;
        }
        //Lightning. High spirit nuke that sets off mines.
    }

    void DetonateMines() //Handles the explosion of the AirMines
        {
        Debug.Log("Proccing DetonateMines()");
        /*if(mines[0] != null)
        {
            AirMineScript myMineScript0 = mines[0].GetComponent<AirMineScript>();
            myMineScript0.airDmg = localPlayerData.airDmg;
            myMineScript0.Detonate();
            mines[0] = null;
        }
        if (mines[1] != null)
        {
            AirMineScript myMineScript1 = mines[1].GetComponent<AirMineScript>();
            myMineScript1.airDmg = localPlayerData.airDmg;
            myMineScript1.Detonate();
            mines[1] = null;
        }
        if (mines[2] != null)
        {
            AirMineScript myMineScript2 = mines[2].GetComponent<AirMineScript>();
            myMineScript2.airDmg = localPlayerData.airDmg;
            myMineScript2.Detonate();
            mines[2] = null;
        }*/
        for(int i = 0; i < localPlayerData.airLevel; i++)
        {
            if(mines[i] != null)
            {
                AirMineScript myMineScript = mines[i].GetComponent<AirMineScript>();
                myMineScript.airDmg = currentForm.dmg;
                myMineScript.Detonate();
                mines[i] = null;
            }
        }
    }
}
