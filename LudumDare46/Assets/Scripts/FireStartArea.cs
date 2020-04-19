using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStartArea : MonoBehaviour
{
    bool fireStarted = false;
    private GlobalBlackBoard gbb;

    private GameObject childFire;
    public GameObject fire;
    // Start is called before the first frame update
    void Start()
    {

        gbb = GameObject.FindGameObjectWithTag("GBB").GetComponent<GlobalBlackBoard>();
    }

    public void LightFire()
    {
        Debug.Log("Light FIre");
        if(!fireStarted)
        {
            childFire = Instantiate(fire, transform);
            gbb.AddFire(childFire.transform);
            fireStarted = true;
        }
    }

    
}