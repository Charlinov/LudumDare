using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFire : MonoBehaviour
{
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
            RaycastHit2D[] hit2Ds = Physics2D.CircleCastAll(transform.position, radius, Vector2.right, radius);
            foreach(RaycastHit2D hit in hit2Ds)
            {
                Debug.Log("Hit shit");
                if(hit.transform.CompareTag("FireSpot"))
                {
                    Debug.Log("FireSpot");
                    hit.transform.GetComponent<Lightable>().LightFire();
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
