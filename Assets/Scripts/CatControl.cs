using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatControl : MonoBehaviour
{

    private const int DEFAULT_SPEED = 2;
    private const float DEFAULT_RAY_LENGTH = 5f;
    private ArrayList UNPASSABLE_OBJECT_TAGS = new ArrayList(new string[] { "wall" });

    //fields
    public int direction;//1 - right;
    public int speed;
    public bool movingRight = true;
    public bool stopMove;
    public bool isOnUpperShelf = true;
    public float delay = 2f;
    public float timeToNextJump;
    public float collisionDisablingTime = 0f;
    public float rayLength = DEFAULT_RAY_LENGTH;

    private Rigidbody2D rb;
    private Animator anim;
    int catLayer, floorLayer;
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        direction = 1;
        speed = DEFAULT_SPEED;
        stopMove = false;
        anim = GetComponent<Animator>();

        catLayer = LayerMask.NameToLayer("CatLayer");
        floorLayer = LayerMask.NameToLayer("Default");

        timeToNextJump = Random.Range(1, 10);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.localScale.x * Vector2.right, rayLength);
        if (collisionDisablingTime <= 0)
        {
            setIgnoringShelfCollisions(false);
        }
        else 
        {
            collisionDisablingTime -= Time.deltaTime;
            setIgnoringShelfCollisions(true);

        }

        //transform.position = new Vector2(transform.position.x + direction * speed*Time.deltaTime, transform.position.y);   
        if (delay <= 0)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            anim.SetBool("pushing", false);
        }
        else
        {
            delay -= Time.deltaTime;
            anim.SetBool("pushing", true);
        }

        timeToNextJump -= Time.deltaTime;
        if (timeToNextJump <= 0) 
        {
            timeToNextJump = Random.Range(1, 10);
            if (!UNPASSABLE_OBJECT_TAGS.Contains(hit.collider.tag))
            {
                setIgnoringShelfCollisions(true);
                
                if (isOnUpperShelf)
                {
                    collisionDisablingTime = 0.5f;
                }
                else
                {
                    collisionDisablingTime = 1f;
                    rb.velocity = Vector2.up * 7.5f;
                }
                isOnUpperShelf = !isOnUpperShelf;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            flip();
        }

        if (collision.gameObject.tag == "droppable")
        {
            delay = 1f;

            Object myvase = collision.GetComponent<Object>();
            if (myvase != null)
            {
                myvase.DropObject();
            }

            anim.SetBool("pushing", true);
        }

    }

    void flip()
    {
        movingRight = !movingRight;
        transform.Rotate(0f, 180, 0f);
    }

    void setIgnoringShelfCollisions(bool value)
    {
        if (value) 
        {
            // Debug.Log("Ignore collisions!");
            Physics2D.IgnoreLayerCollision(catLayer, floorLayer, true);
        }
        else
        {
            // Debug.Log("Stop ignoring collisions!");
            Physics2D.IgnoreLayerCollision(catLayer, floorLayer, false);
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.localScale.x * Vector3.right * rayLength);
    }
}
