using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public readonly static HashSet<Interactable> Pool = new HashSet<Interactable>();
    public float rightBound = 0;
    public float leftBound = 0;
    public float topBound = 0;
    public float botBound = 0;
    public string interactableName = "None";

    private void OnEnable()
    {
        Interactable.Pool.Add(this);
    }
    private void OnDisable()
    {
        Interactable.Pool.Remove(this);
    }

    public static void TriggerInteractions(float leftX, float rightX, float topY, float botY)
    {
        var e = Interactable.Pool.GetEnumerator();
        while (e.MoveNext())
        {
            if (!((rightX < e.Current.transform.position.x - e.Current.leftBound) || (leftX > e.Current.transform.position.x + e.Current.rightBound) || (topY < e.Current.transform.position.y - e.Current.botBound) || (botY > e.Current.transform.position.y + e.Current.topBound)))
            {
                e.Current.RunInteraction();                
            }
        }
    }

    void RunInteraction()
    {
        //This is going to be where we send a message to some game manager telling it what interaction to run
    }
}
