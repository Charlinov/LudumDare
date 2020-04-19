using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFighterLogic : MonoBehaviour
{
    private GlobalBlackBoard gbb;

    private Transform closestFire;
    public float fireSpotRadius;

    private Vector2 moveDir;
    private Rigidbody2D rb;

    public float Speed;
    public float extinguishRadius;

    private float fireTimeout = 5f;
    private float fireTimeoutTimer;


    public float playerSpotRadius;
    public float playerStopChaseRadius;

    private Transform player;

    public LayerMask layerMask;

    public Transform path;
    private List<Transform> PatrolRoute = new List<Transform>();
    private int patrolIndex = 0;

    enum States
    {
        IDLE,
        CHASE,
        FIREFIGHTING
    }

    private States state;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gbb = GameObject.FindGameObjectWithTag("GBB").GetComponent<GlobalBlackBoard>();
        player = gbb.getPlayer();

        foreach(Transform child in path)
        {
            PatrolRoute.Add(child);
        }

        GetClosestPointOnPath();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == States.IDLE)
        {
            // do a raycast
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, player.position - transform.position, playerSpotRadius, layerMask);
            if(hit2D)
            {
                if (hit2D.transform.CompareTag("Player"))
                {
                    Debug.DrawLine(transform.position, player.position, Color.green);
                    state = States.CHASE;
                    return;
                }
            }

            Debug.DrawLine(transform.position, player.position, Color.red);

            // check closest fire distance
            closestFire = gbb.getClosestFire(transform.position);
            if (closestFire)
            {
                if (Vector2.Distance(transform.position, closestFire.position) < fireSpotRadius)
                {
                    fireTimeoutTimer = fireTimeout;
                    state = States.FIREFIGHTING;
                }
            }

            // patrol

            // get next point if we're at the current one
            if(Vector2.Distance(transform.position, PatrolRoute[patrolIndex].position) < 1)
            {
                patrolIndex++;
                if (patrolIndex >= PatrolRoute.Count)
                    patrolIndex = 0;
            }

            Debug.DrawLine(transform.position, PatrolRoute[patrolIndex].position, Color.blue);

            moveDir = (PatrolRoute[patrolIndex].position - transform.position).normalized;
            LookAtPoint(PatrolRoute[patrolIndex].position);

        }
        else if (state == States.FIREFIGHTING)
        {
            closestFire = gbb.getClosestFire(transform.position);
            if (!closestFire)
                state = States.IDLE;
            else
            {
                moveDir = (closestFire.position - transform.position).normalized;
                LookAtPoint(closestFire.position);
                if (Vector2.Distance(closestFire.position, transform.position) < extinguishRadius)
                {
                    // play an animation
                    Extinguish(); // call this from an animation event

                    state = States.IDLE;
                }
            }
        }
        else if (state == States.CHASE)
        {
            // test if we can see the player still
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, player.position - transform.position, playerStopChaseRadius, layerMask);
            if (hit2D)
            {
                if (hit2D.transform.CompareTag("Player"))
                {
                    Debug.DrawLine(transform.position, player.position, Color.magenta);
                    state = States.CHASE;
                    moveDir = (player.position - transform.position).normalized;

                    LookAtPoint(player.position);
                }
                else
                {
                    GetClosestPointOnPath();

                    state = States.IDLE;
                }
            }
            else
            {
                // get closest point on path
                GetClosestPointOnPath();

                state = States.IDLE;
            }
        }

        rb.velocity = moveDir * Speed;

        fireTimeoutTimer -= Time.deltaTime;
    }

    private void LookAtPoint(Vector3 point)
    {
        Vector3 dir = point - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void GetClosestPointOnPath()
    {
        int closestIndex = 0;

        int tempIndex = 0;
        foreach(Transform point in PatrolRoute)
        {
            if(Vector2.Distance(transform.position, PatrolRoute[closestIndex].position) > Vector2.Distance(transform.position, point.position))
            {
                closestIndex = tempIndex;
            }

            tempIndex++;
        }

        patrolIndex = closestIndex;
    }

    public void Extinguish()
    {
        closestFire.GetComponent<Fire>().Extinguish();
        moveDir = new Vector2(0, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, extinguishRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireSpotRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, playerSpotRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, playerStopChaseRadius);
    }
}