using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDownMovement : MonoBehaviour
{
    private Vector2 direction;
    public float speed;
    private Vector2 mousePos;

    private Rigidbody2D rb;

    private GlobalBlackBoard gbb;

    public Animator anim;

    public LightFire LF;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        gbb = GameObject.FindGameObjectWithTag("GBB").GetComponent<GlobalBlackBoard>();
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        mousePos = Input.mousePosition;

        direction.Normalize();

        rb.velocity = direction * speed;
        LF.setTDMSpeed(rb.velocity.magnitude);
        anim.SetFloat("Speed", rb.velocity.magnitude);

        // look at mouse
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // add ourselves to the heat map
        gbb.AddHeat(transform.position);
    }
}
