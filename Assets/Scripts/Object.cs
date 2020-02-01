using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Object : MonoBehaviour, IPooledObject
{
    public string tag;
    public bool OnShelf { get; set; }

    public float upForce = 1f;

    public float sideForce = .15f;

    public Collider2D shelf;

    public String itemName;

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
        Debug.Log("collision");
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shelf, true);
        String itemName = NameGenerator.nameGenerate( 
            WorldVariablesHandler.Instance.GetPredicateList(),
            WorldVariablesHandler.Instance.GetAdjectiveList1(),
            WorldVariablesHandler.Instance.GetAdjectiveList2(),
            tag);
    }

    void Update()
    {
        if (!OnShelf) {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -2f));
        }
    }

    public void DestroyObject() {
        OnShelf = false;
        ObjectPooler.Instance.AddToQueue("cube", gameObject);
        WorldVariablesHandler.Instance.nameList.Remove(itemName);
    }
}
