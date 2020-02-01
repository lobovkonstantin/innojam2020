using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatControl : MonoBehaviour
{
    //fields
    public int direction;//1 - right;
    public int speed;
    public bool movingRight = true;
    public bool stopMove;
    public bool isOnUpperShelf = true;
    public float delay = 0f;
    public float timeToNextJump;
    public float collisionDisablingTime = 0f;

    private Rigidbody2D rb;
    private Animator anim;
    int catLayer, floorLayer;
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        direction = 1;
        speed = 2;
        stopMove = false;
        anim = GetComponent<Animator>();

        catLayer = LayerMask.NameToLayer("CatLayer");
        floorLayer = LayerMask.NameToLayer("Default");

        timeToNextJump = Random.Range(1, 10);
    }

    // Update is called once per frame
    void Update()
    {

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
            setIgnoringShelfCollisions(true);
            timeToNextJump = Random.Range(1, 10);
            if (isOnUpperShelf)
            {
                collisionDisablingTime = 0.5f;
            }
            else {
                collisionDisablingTime = 1f;
                rb.velocity = Vector2.up * 7.5f;
            }
            isOnUpperShelf = !isOnUpperShelf;

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
            Debug.Log("Ignore collisions!");
            Physics2D.IgnoreLayerCollision(catLayer, floorLayer, true);
        }
        else
        {
            Debug.Log("Stop ignoring collisions!");
            Physics2D.IgnoreLayerCollision(catLayer, floorLayer, false);
        }
        
    }
}
