using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    /*
    Build an architecture that has Mysteries and Stats
    Build an architecture for Mysteries that has their various levels set with an array of bools
        Should be able to drill down to Mysteries, with highest locked one presenting a clue about what you're looking for
            Set it up as a "found document" style: LightElems1, LightElems2, Ayasu, etc.
    Build a structure of input commands which let Player's PausedControls send inputs to this script which interprets them
    */

    string whichMenu = "Mysteries";

    public void MenuLeft()
    {

    }

    public void MenuRight()
    {

    }

    public void MenuUp()
    {

    }
    
    public void MenuDown()
    {

    }

    public void MenuConfirm()
    {

    }

    public void MenuCancel()
    {

    }

    public void MenuLB()
    {
        if(whichMenu == "Mysteries")
        {
            whichMenu = "Stats";
        }
        if(whichMenu == "Stats")
        {
            whichMenu = "Mysteries";
        }
    }

    public void MenuRB()
    {
        if(whichMenu == "Stats")
        {
            whichMenu = "Mysteries";
        }
        if (whichMenu == "Mysteries")
        {
            whichMenu = "Stats";
        }
    }


}
