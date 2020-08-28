using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


[Serializable]
public class PlayerStatistics
{

    public int SceneID;
    public float PositionX, PositionY;

    public int lightLevel;
    public int voidLevel;
    public int fireLevel;
    public int waterLevel;
    public int earthLevel;
    public int airLevel;

    public int lightMastery;
    public int voidMastery;
    public int fireMastery;
    public int waterMastery;
    public int earthMastery;
    public int airMastery;

    public int lightXP;
    public int voidXP;
    public int fireXP;
    public int waterXP;
    public int earthXP;
    public int airXP;

    public int xpReservoir;
    public int maxXPReservoir;

    public bool lightUnlocked = true;
    public bool fireUnlocked = false;
    public bool waterUnlocked = false;
    public bool earthUnlocked = false;
    public bool airUnlocked = false;
    public bool voidUnlocked = false;
}

[Serializable]
public class SavedDroppableEnemy
{
    public float positionX, positionY;
    public int enemyID;
}

/*

[Serializable]
public class SavedDroppableThingy{
	public float positionX, positionY;
}

*/

[Serializable]
public class SavedDroppableList
{
    public int SceneID;
    public List<SavedDroppableEnemy> SavedEnemies;
    //public List<SavedDroppableThingy> SavedThingies;

    public SavedDroppableList(int newSceneID)
    {
        this.SceneID = newSceneID;
        this.SavedEnemies = new List<SavedDroppableEnemy>();
        //this.SavedThingies = new List<SavedDroppableThingy>();
    }
}
