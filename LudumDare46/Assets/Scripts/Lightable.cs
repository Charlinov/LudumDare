using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Lightable : MonoBehaviour
{
    private bool onFire;
    private GameObject childFire;
    public GameObject fire;

    private GlobalBlackBoard gbb;

    private float timeBetweenLightings = 3f;
    private float fireTimer;

    // Start is called before the first frame update
    void Start()
    {
        gbb = GameObject.FindGameObjectWithTag("GBB").GetComponent<GlobalBlackBoard>();
    }

    // Update is called once per frame
    void Update()
    {

        fireTimer -= Time.deltaTime;
    }

    public bool isOnFire()
    {
        return onFire;
    }

    public void LightFire()
    {
        if (!onFire && fireTimer < 0)
        {
            childFire = Instantiate(fire, transform);
            gbb.AddFire(childFire.transform);
            onFire = true;
        }
    }

    public void Extinguish()
    {
        childFire = null;
        onFire = false;
        fireTimer = timeBetweenLightings;
    }
}
