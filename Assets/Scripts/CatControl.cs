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
    public float delay = 0f;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        direction = 1;
        speed = 2;
        stopMove = false;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
}
