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
    private bool extinguishing = false;

    private float fireTimeout = 5f;
    private float fireTimeoutTimer;


    public float playerSpotRadius;
    public float playerStopChaseRadius;
    public float playerCatchRadius;

    private Transform player;

    public LayerMask layerMask;

    public Transform path;
    private List<Transform> PatrolRoute = new List<Transform>();
    private int patrolIndex = 0;

    private GameControl GC;


    public Animator anim;

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
        GC = GameObject.FindGameObjectWithTag("GC").GetComponent<GameControl>();

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
                    if(!extinguishing)
                        StartCoroutine(Extinguish());
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

                    Debug.Log(Vector2.Distance(transform.position, player.position));
                    if(Vector2.Distance(transform.position, player.position) < playerCatchRadius)
                    {
                        GC.EndGame();
                    }
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
        anim.SetFloat("Speed", rb.velocity.magnitude);

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

    private IEnumerator Extinguish()
    {
        extinguishing = true;
        anim.SetTrigger("Extinguish");
        Debug.Log("Started extinguishing");
        moveDir = new Vector2(0, 0);
        yield return new WaitForSeconds(1f);
        if(closestFire)
        {
            closestFire.GetComponent<Fire>().Extinguish();
            Debug.Log("Extinguished ONE fire");
        }
        extinguishing = false;
        state = States.IDLE;
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