using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Fire : MonoBehaviour
{
    private GlobalBlackBoard gbb;
    private Light2D attachedLight;

    // responsible for fire spread and scale growth
    public float fireRadius = 0.5f;
    public float fireGrowMod = 0.2f;

    public float fireLifeTime = 10f;
    private float fireLifeTimer;

    // Start is called before the first frame update
    void Start()
    {
        attachedLight = GetComponent<Light2D>();
        gbb = GameObject.FindGameObjectWithTag("GBB").GetComponent<GlobalBlackBoard>();
        transform.localScale = new Vector3(fireRadius, fireRadius, 1f);
        fireLifeTimer = fireLifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        attachedLight.pointLightInnerRadius = (Mathf.Sin(Time.time) + 1) / 2;
        gbb.AddHeat(transform.position);

        RaycastHit2D[] hit2Ds = Physics2D.CircleCastAll(transform.position, fireRadius, Vector2.right);
        foreach (RaycastHit2D hit in hit2Ds)
        {
            if(HasComponent<Lightable>(hit.transform.gameObject))
            {
                // calculate distance to lightable
                float distToLightable = Vector2.Distance(transform.position, hit.transform.position);

                // generate chance to light
                float rand = Random.Range(0f, 1f);
                Debug.Log(rand * fireRadius / distToLightable);
                if(rand * fireRadius / distToLightable > 1)
                {
                    hit.transform.GetComponent<Lightable>().LightFire();
                }
            }
        }

        fireRadius = Mathf.Min(1.7f, fireRadius + (Time.deltaTime * fireGrowMod));
        transform.localScale = new Vector3(fireRadius, fireRadius, 1f);

        fireLifeTimer -= Time.deltaTime;
        if(fireLifeTimer <= 0)
        {
            Burn();
        }
    }

    public void Extinguish()
    {
        gbb.RemoveFire(transform);
        transform.parent.GetComponent<Lightable>().Extinguish();
        Destroy(gameObject);
    }

    public void Burn()
    {
        gbb.RemoveFire(transform);
        transform.parent.GetComponent<Lightable>().Extinguish();
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, fireRadius);
    }

    public static bool HasComponent<T> (GameObject obj)
    {
        return (obj.GetComponent(typeof(T)) != null);
    }
}