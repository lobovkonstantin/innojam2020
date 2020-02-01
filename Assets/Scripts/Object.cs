using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour, IPooledObject
{
    public string tag;
    public bool OnShelf { get; set; }
    public float upForce = 1f;
    public float sideForce = .15f;

    public Collider2D shelf;

    void Start()
    {
        shelf = GameObject.FindGameObjectWithTag("shelf").GetComponent<Collider2D>();
    }

    public void OnObjectSpawn() {
        OnShelf = true;
    }

    public void DropObject() {
        OnShelf = false;
        float xForce = Random.Range(-sideForce, sideForce);
        float yForce = Random.Range(-upForce/2f, upForce);

        Vector2 force = new Vector2(xForce, yForce);
        GetComponent<Rigidbody2D>().velocity = force;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shelf, true);
    }

    void Update()
    {
        if (!OnShelf) {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -2f));
        }
    }

    public void DestroyObject() {
        ObjectPooler.Instance.AddToQueue(tag, gameObject);

    }
}
