using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFire : MonoBehaviour
{
    public float radius;

    public GameObject canvas;
    public Animator anim;
    public TopDownMovement tdm;

    private float tdmSpeed = 0;

    private GameObject lightingObject = null;

    private bool startFire;

    public float lightTime;
    private float lightTimer;
    private bool lighting;
    private bool cancelled;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit2D[] hit2Ds = Physics2D.CircleCastAll(transform.position, radius, Vector2.right, radius);
            foreach(RaycastHit2D hit in hit2Ds)
            {
                if(hit.transform.CompareTag("FireSpot"))
                {
                    lightingObject = hit.transform.gameObject;
                }
                else if(hit.transform.CompareTag("Stairs"))
                {
                    canvas.GetComponent<Animator>().SetTrigger("UseStairs");
                    StartCoroutine(useStairs(hit.transform.gameObject));
                }
            }
        }

        // this has to be here and not in a function due to it being run-time
        if(lightingObject != null)
        {
            TryLightFire();
        }
    }

    private IEnumerator useStairs(GameObject stairs)
    {
        yield return new WaitForSeconds(1.0f);

        // go to the point
        transform.parent.parent.position = stairs.GetComponent<Stairs>().TravelPoint.position;
    }

    private void TryLightFire()
    {
        if (lighting)
        {
            if (lightTimer < 0 && !cancelled)
            {
                lightingObject.transform.GetComponent<Lightable>().LightFire();
                lighting = false;
                cancelled = false;
                lightingObject = null;
            }
            else if (lightTimer < 0)
            {
                lighting = false;
                cancelled = false;
                lightingObject = null;
            }
            else
            {
                lightTimer -= Time.deltaTime;
                if (tdmSpeed > 0)
                    cancelled = true;
            }
        }
        else
        {
            lightTimer = lightTime;
            lighting = true;
            cancelled = false;
            anim.SetTrigger("LightFire");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void setTDMSpeed(float speed)
    {
        tdmSpeed = speed;
    }
}
