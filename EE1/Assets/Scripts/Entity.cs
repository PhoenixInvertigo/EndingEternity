using UnityEngine;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{

    public readonly static HashSet<Entity> Pool = new HashSet<Entity>();
    /*public float incDmg = 0;
    public string incDmgType = null;*/
    protected int invulTimer = 3;
    public float rightBound;
    public float leftBound;
    public float topBound;
    public float botBound;
    /*public string incBuff = null;
    public float incBuffTimer = 0;
    public float incBuffMagnitude = 0;
    public float incKnockback = 0;*/
    //public Vector2 incKnockbackDir = Vector2.zero;
    public float incCenterX = 0;
    public float incCenterY = 0;

    //public Player myPlayerScript = null;
    //public Enemy myEnemyScript = null;


    /*private void OnEnable()
    {
        Entity.Pool.Add(this);
        //incBuff = null;
    }
    private void OnDisable()
    {
        Entity.Pool.Remove(this);
    }*/

    void FixedUpdate()
    {

    }

    //Add Knockback
    public static void TriggerHits(float leftX, float rightX, float topY, float botY, float iDmg, string iDmgType, float iKnockback, string iKnockbackDir)
    {
        Debug.Log("TriggerHits has been activated");
        //checks everything with the Entity script on it to see if they fall inside these coordinates and triggers GetHit on each of those entities
        var e = Entity.Pool.GetEnumerator();
        while (e.MoveNext())
        {
            Debug.Log("Checking an e in Entity.Pool");
            if (!((rightX < e.Current.transform.position.x - e.Current.leftBound) || (leftX > e.Current.transform.position.x + e.Current.rightBound) || (topY < e.Current.transform.position.y - e.Current.botBound) || (botY > e.Current.transform.position.y + e.Current.topBound)))
            {
                Debug.Log("This e was in bounds of collision check");
                if (e.Current.invulTimer == 0)
                {
                    e.Current.incCenterX = (leftX + rightX) / 2;
                    e.Current.incCenterY = (topY + botY) / 2;
                    Vector2 incKnockbackDir = Vector2.zero;
                    if (iKnockbackDir == "Directional")
                    {
                        incKnockbackDir = e.Current.DetermineDirection(e.Current.incCenterX, e.Current.incCenterY);
                    }
                    else
                    {
                        incKnockbackDir = e.Current.DetermineInputDirection(iKnockbackDir);
                    }
                    /*if (e.Current.incKnockbackDir != Vector2.zero)
                    {
                        e.Current.incKnockback = iKnockback;
                    }*/
                    //e.Current.GetHit(iDmg, iDmgType);
                    /*if (e.Current.myPlayerScript != null)
                    {
                        e.Current.myPlayerScript.TakeDamage(iDmg, iDmgType);                       
                        e.Current.myPlayerScript.TakeKnockback(iKnockback, incKnockbackDir);
                    }

                    if (e.Current.myEnemyScript != null)
                    {
                        e.Current.myEnemyScript.TakeDamage(iDmg, iDmgType);
                        e.Current.myEnemyScript.TakeKnockback(iKnockback, incKnockbackDir);
                    }*/

                    Debug.Log("Calling TakeDamage and TakeKnockback from Entity.Pool");
                    e.Current.TakeDamage(iDmg, iDmgType);
                    e.Current.TakeKnockback(iKnockback, incKnockbackDir);

                    e.Current.invulTimer = 3;
                }
            }

        }
    }

    public static void TriggerHitsBuff(float leftX, float rightX, float topY, float botY, float iDmg, string iDmgType, string buff, float buffTimer, float buffMagnitude, float iKnockback, string iKnockbackDir)
    {
        Debug.Log("TriggerHitsBuff has been activated");
        //checks everything with the Entity script on it to see if they fall inside these coordinates and triggers GetHit on each of those entities
        var e = Entity.Pool.GetEnumerator();
        while (e.MoveNext())
        {

            Debug.Log("Checking an e in Entity.Pool");
            if (!((rightX < e.Current.transform.position.x - e.Current.leftBound) || (leftX > e.Current.transform.position.x + e.Current.rightBound) || (topY < e.Current.transform.position.y - e.Current.botBound) || (botY > e.Current.transform.position.y + e.Current.topBound)))
            {
                Debug.Log("This e was in bounds of collision check");
                if (e.Current.invulTimer == 0)
                {
                    e.Current.incCenterX = (leftX + rightX) / 2;
                    e.Current.incCenterY = (topY + botY) / 2;
                    Vector2 incKnockbackDir = Vector2.zero;
                    if (iKnockbackDir == "Directional")
                    {
                        incKnockbackDir = e.Current.DetermineDirection(e.Current.incCenterX, e.Current.incCenterY);
                    } else
                    {
                        incKnockbackDir = e.Current.DetermineInputDirection(iKnockbackDir);
                    }
                    /*if(e.Current.incKnockbackDir != Vector2.zero)
                    {
                        e.Current.incKnockback = iKnockback;
                    }*/
                    //e.Current.GetHitBuff(iDmg, iDmgType, buff, buffTimer, buffMagnitude);
                    /*if(e.Current.myPlayerScript != null)
                    {
                        e.Current.myPlayerScript.TakeDamage(iDmg, iDmgType);
                        e.Current.myPlayerScript.LoadBuff(buff, buffTimer, buffMagnitude);
                        e.Current.myPlayerScript.TakeKnockback(iKnockback, incKnockbackDir);
                    }

                    if (e.Current.myEnemyScript != null)
                    {
                        e.Current.myEnemyScript.TakeDamage(iDmg, iDmgType);
                        e.Current.myEnemyScript.LoadBuff(buff, buffTimer, buffMagnitude);
                        e.Current.myEnemyScript.TakeKnockback(iKnockback, incKnockbackDir);
                    }*/
                    Debug.Log("Calling TakeDamage, LoadBuff, and TakeKnockback from Entity.Pool");
                    e.Current.TakeDamage(iDmg, iDmgType);
                    e.Current.LoadBuff(buff, buffTimer, buffMagnitude);
                    e.Current.TakeKnockback(iKnockback, incKnockbackDir);

                    e.Current.invulTimer = 3;
                }
            }

        }
    }

    public virtual void TakeDamage(float incDmg, string incDmgType)
    {
        Debug.Log("TakeDamage in Entity");
    }

    public virtual void TakeKnockback(float incKnockback, Vector2 incKnockbackDir)
    {
        Debug.Log("TakeKnockback in Entity");
    }

    public virtual void LoadBuff(string buff, float timer, float magnitude)
    {

    }

    //saves the incoming damage and type of all hit entities
    /*void GetHit(float iDmg, string iDmgType)
    {
        Debug.Log("This was hit! (on eScript)");
        incDmg = iDmg;
        incDmgType = iDmgType;
        invulTimer = .1f;
    }*/

    /*void GetHitBuff(float iDmg, string iDmgType, string iBuff, float iBuffTimer, float ibuffMagnitude)
    {
        Debug.Log("This was hit and given a buff! (on eScript)");
        incDmg = iDmg;
        incDmgType = iDmgType;
        incBuff = iBuff;
        incBuffTimer = iBuffTimer;
        incBuffMagnitude = ibuffMagnitude;
        invulTimer = .1f;
    }*/

    public Vector2 DetermineDirection(float centX, float centY)
    {
        Vector2 outDir = Vector2.zero;

        outDir.x = this.transform.position.x - centX;
        outDir.y = this.transform.position.y - centY;


        return outDir.normalized;
    }

    public Vector2 DetermineInputDirection(string incKBDir)
    {
        Vector2 outDir = Vector2.zero;

        switch (incKBDir)
        {
            case "Up":
                outDir = new Vector2(0f, 1f);
                break;
            case "Down":
                outDir = new Vector2(0f, -1f);
                break;
            case "Left":
                outDir = new Vector2(-1f, 0f);
                break;
            case "Right":
                outDir = new Vector2(1f, 0f);
                break;
            case "UpRight":
                outDir = new Vector2(1f, 1f);
                break;
            case "DownRight":
                outDir = new Vector2(1f, -1f);
                break;
            case "UpLeft":
                outDir = new Vector2(-1f, 1f);
                break;
            case "DownLeft":
                outDir = new Vector2(-1f, -1f);
                break;
            default:
                Debug.Log("Knockback was not Directional, nor did it have a Direction listed");
                break;
        }

        return outDir.normalized;
    }
}

