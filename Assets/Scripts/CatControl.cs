using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatControl : MonoBehaviour
{

    private const int DEFAULT_SPEED = 2;
    private const float DEFAULT_RAY_LENGTH = 5f;
    private const Floors STARTING_FLOOR = Floors.GROUND;

    private ArrayList UNPASSABLE_OBJECT_TAGS = new ArrayList(new string[] { "wall" });

    //fields
    public int direction;//1 - right;
    public int speed;
    public bool movingRight = true;
    public bool stopMove;
    public float delay = 2f;
    public float timeToNextJump;
    public float collisionDisablingTime = 0f;
    public float rayLength = DEFAULT_RAY_LENGTH;
    public bool isJumping = false;
    public bool isBrushDropped = false;
    public bool isHeadDropped = false;
    public bool isBrushTorqueAdded = false;

    public AudioClip say_meow;
    public AudioClip say_murmur;
    public Floors currentFloor;

    private Rigidbody2D rb;
    private Animator anim;
    int catLayer, floorLayer, droppablesLayer, headLayer, brushLayer, deadHeadShelfLayer;
    // Start is called before the first frame update
    void Start()
    {

        currentFloor = STARTING_FLOOR;
        rb = GetComponent<Rigidbody2D>();
        direction = 1;
        speed = DEFAULT_SPEED;
        stopMove = false;
        anim = GetComponent<Animator>();

        catLayer = LayerMask.NameToLayer("CatLayer");
        floorLayer = LayerMask.NameToLayer("Default");
        droppablesLayer = LayerMask.NameToLayer("Droppables");
        brushLayer = LayerMask.NameToLayer("Brush");
        headLayer = LayerMask.NameToLayer("Head");
        deadHeadShelfLayer = LayerMask.NameToLayer("DeadHeadShelf");

        Physics2D.IgnoreLayerCollision(catLayer, droppablesLayer, true);
        Physics2D.IgnoreLayerCollision(catLayer, brushLayer, true);
        Physics2D.IgnoreLayerCollision(catLayer, headLayer, true);
        Physics2D.IgnoreLayerCollision(droppablesLayer, brushLayer, true);
        Physics2D.IgnoreLayerCollision(droppablesLayer, headLayer, true);
        Physics2D.IgnoreLayerCollision(floorLayer, brushLayer, true);
        Physics2D.IgnoreLayerCollision(floorLayer, headLayer, false);
        Physics2D.IgnoreLayerCollision(deadHeadShelfLayer, headLayer, false);

        timeToNextJump = Random.Range(1, 10);
        //sounds
        say_meow= GetComponent<AudioClip>();
        say_murmur= GetComponent<AudioClip>();
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
            if (isBrushTorqueAdded) 
            {
                GameObject.FindGameObjectWithTag("brush").GetComponent<Rigidbody2D>().AddTorque(-100f);
                isBrushTorqueAdded = false;
            }
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
            if (!UNPASSABLE_OBJECT_TAGS.Contains(hit.collider.tag) && !isJumping)
            {
                jump();
            }
        }
        if (currentFloor == Floors.TABLE && transform.position.x < 1f && !isJumping && !movingRight) {
            jump();
        }
        if (shouldDropBrush()) {
            dropBrush();
        }
        if (shouldDropDeadHead()) 
        {
            dropDeadHead();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag) 
        {
            case "uppershelf": currentFloor = Floors.UPPER_SHELF; resetMovement(); break;
            case "lowershelf": currentFloor = Floors.LOWER_SHELF; resetMovement(); break;
            case "table": currentFloor = Floors.TABLE; resetMovement(); break;
            case "ground": currentFloor = Floors.GROUND; resetMovement(); break;
            case "windowsilk": currentFloor = Floors.WINDOW_SILK; resetMovement(); break;
        }

        if (collision.gameObject.tag == "wall")
        {
            flip();
        }

        if (collision.gameObject.tag == "droppable")
        {
            collision.gameObject.layer = LayerMask.NameToLayer("Droppables");
            delay = 1f;

            Object myvase = collision.GetComponent<Object>();
            if (myvase != null)
            {
                myvase.DropObject();
                Spawner.Instance.OnItemDropped(myvase.index);
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

    private void jump() 
    {
        setIgnoringShelfCollisions(true);
        JumpStrategy jumpStrategy = getJumpStrategy();
        startJumpForStrategy(jumpStrategy);
        if (jumpStrategy != JumpStrategy.NONE) {
            isJumping = true;
        }
    }

    private JumpStrategy getJumpStrategy() {
        float catX = transform.position.x;
        switch (currentFloor) {
            case Floors.UPPER_SHELF: return JumpStrategy.DOWN;
            case Floors.LOWER_SHELF: return randomBool() ? JumpStrategy.NORMAL_UP : JumpStrategy.DOWN;
            case Floors.TABLE: return getJumpStrategyFromTable(randomBool(), catX);
            case Floors.WINDOW_SILK: return JumpStrategy.DOWN;
            case Floors.GROUND: return getJumpStrategyFromGround(randomBool(), catX);
        }
        return JumpStrategy.NONE;
    }

    private JumpStrategy getJumpStrategyFromTable(bool randomBool, float catX) {
        if (catX >= 2.7f)
        {
            return randomBool ? JumpStrategy.NORMAL_UP : JumpStrategy.DOWN;
        }
        else if (catX <= 1f && !movingRight) 
        {
            return randomBool ? JumpStrategy.FORWARD : JumpStrategy.DOWN;
        }
        return JumpStrategy.DOWN;
    }

    private JumpStrategy getJumpStrategyFromGround(bool randomBool, float catX)
    {
        if (catX <= -3.20f)
        {
            return JumpStrategy.NONE;
        }
        else if (catX <= -0.5f)
        {
            return JumpStrategy.VERY_HIGH_UP;
        }
        return JumpStrategy.HIGH_UP;
    }

    private void startJumpForStrategy(JumpStrategy strategy) 
    {
        if (strategy == JumpStrategy.NONE) {
            return;
        }
        if (strategy != JumpStrategy.FORWARD) 
        {
            speed = 0;
        }
        switch (strategy) 
        {
            case JumpStrategy.DOWN: collisionDisablingTime = 0.5f; break;
            case JumpStrategy.NORMAL_UP: collisionDisablingTime = 1f; rb.velocity = Vector2.up * 7.5f; GetComponent<AudioSource>().Play(); break;
            case JumpStrategy.HIGH_UP: collisionDisablingTime = 1.3f; rb.velocity = Vector2.up * 9f; GetComponent<AudioSource>().Play(); break;
            case JumpStrategy.VERY_HIGH_UP: collisionDisablingTime = 1.5f; rb.velocity = Vector2.up * 10f; GetComponent<AudioSource>().Play(); break;
            case JumpStrategy.FORWARD:  rb.velocity = Vector2.up * 7f; GetComponent<AudioSource>().Play(); break;
        }
    }

    private void resetMovement() 
    {
        speed = DEFAULT_SPEED;
        isJumping = false;
    }

    private bool randomBool() {
        return Random.Range(0, 10) > 5;
    }

    private bool shouldDropBrush() 
    {
        return currentFloor == Floors.GROUND && !isJumping && !isBrushDropped
            && ((transform.position.x > -4.80f && transform.position.x < -4.35f && movingRight) || (transform.position.x < -2.60f && transform.position.x > -3f && !movingRight))
            && randomBool();
    }

    private void dropBrush() 
    {
        delay = 1f;
        anim.SetBool("pushing", true);
        isBrushDropped = true;
        GameObject.FindGameObjectWithTag("brush").GetComponent<Rigidbody2D>().AddTorque(100f);
        isBrushTorqueAdded = true;
    }

    private bool shouldDropDeadHead()
    {
        return currentFloor == Floors.GROUND && !isJumping && !isHeadDropped
            && (transform.position.x > -6.80f && transform.position.x < -4.80f)
            && randomBool();
    }

    private void dropDeadHead() 
    {
        delay = 1f;
        anim.SetBool("pushing", true);
        isHeadDropped = true;
        Physics2D.IgnoreLayerCollision(deadHeadShelfLayer, headLayer, true);
    }

    public enum Floors
    {
        UPPER_SHELF,
        LOWER_SHELF,
        TABLE,
        GROUND,
        WINDOW_SILK
    }

    public enum JumpStrategy 
    { 
        NORMAL_UP, // table -> lower shelf, lower shelf -> upper shelf
        HIGH_UP, // ground -> table
        VERY_HIGH_UP, // ground -> window silk
        DOWN,
        FORWARD, // table -> window silk
        NONE
    }
}
