using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FireStartArea : MonoBehaviour
{
    private bool lit = false;
    private Light2D attachedLight;
    private SpriteRenderer spriteRenderer;

    private GlobalBlackBoard gbb;

    // Start is called before the first frame update
    void Start()
    {
        attachedLight = GetComponent<Light2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        attachedLight.enabled = false;
        spriteRenderer.enabled = false;

        gbb = GameObject.FindGameObjectWithTag("GBB").GetComponent<GlobalBlackBoard>();
    }

    // Update is called once per frame
    void Update()
    {
        if(lit)
        {
            attachedLight.pointLightInnerRadius = (Mathf.Sin(Time.time) + 1) /2;
            gbb.AddHeat(transform.position);
        }
    }

    public void LightFire()
    {
        lit = true;
        attachedLight.enabled = true;
        spriteRenderer.enabled = true;
    }
}