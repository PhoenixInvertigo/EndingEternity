using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormSelectorScript : MonoBehaviour
{

     public GameObject airCircle;
    public GameObject earthCircle;
    public GameObject fireCircle;
    public GameObject lightCircle;
    public GameObject voidCircle;
    public GameObject waterCircle;
    GameObject myButton;

    private void Awake()
    {
        Debug.Log("FormSelectorScript is Awake!");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DisableFormSelector()
    {
        Destroy(gameObject);
        //this.gameObject.SetActive(false);
        Debug.Log("Disabling FormSelector");
        if(myButton != null)
        {
            Destroy(myButton);
        }
    }

    public void SelectAir()
    {
        if (myButton != null)
        {
            Destroy(myButton);
        }
        myButton = (GameObject)Instantiate(airCircle);
        myButton.transform.position = new Vector3(this.transform.position.x - .31f, this.transform.position.y + .12f, 0);
    }

    public void SelectEarth()
    {
        if (myButton != null)
        {
            Destroy(myButton);
        }
        myButton = (GameObject)Instantiate(earthCircle);
        myButton.transform.position = new Vector3(this.transform.position.x + .3f, this.transform.position.y - .12f, 0);
    }

    public void SelectFire()
    {
        if (myButton != null)
        {
            Destroy(myButton);
        }
        myButton = (GameObject)Instantiate(fireCircle);
        myButton.transform.position = new Vector3(this.transform.position.x + .3f, this.transform.position.y + .12f, 0);
    }

    public void SelectLight()
    {
        Debug.Log("Instantiating Light Button");
        if(myButton != null)
        {
            Destroy(myButton);
        }
        myButton = (GameObject)Instantiate(lightCircle);
        myButton.transform.position = new Vector3(this.transform.position.x - .01f, this.transform.position.y + .3f, 0);
        // myHitbox1.transform.localScale = new Vector3(.5f, 2.5f, 0);
        // myHitbox1.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .65f, 0);
    }

    public void SelectVoid()
    {
        {
            if (myButton != null)
            {
                Destroy(myButton);
            }
            myButton = (GameObject)Instantiate(voidCircle);
            myButton.transform.position = new Vector3(this.transform.position.x - .01f, this.transform.position.y - .29f, 0);
        }
    }

    public void SelectWater()
    {
        if (myButton != null)
        {
            Destroy(myButton);
        }
        myButton = (GameObject)Instantiate(waterCircle);
        myButton.transform.position = new Vector3(this.transform.position.x - .31f, this.transform.position.y - .12f, 0);
    }
}
