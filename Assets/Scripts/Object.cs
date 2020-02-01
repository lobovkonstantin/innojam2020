using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Object : MonoBehaviour, IPooledObject
{
    public int index { get; set; }

    public string tag;
    public bool OnShelf { get; set; }

    public float upForce = 1f;

    public float sideForce = .15f;

    public Collider2D uppershelf;
    public Collider2D lowershelf;
    public Collider2D table;
    public Collider2D windowsilk;

    public string itemName { get; set; }

    void Start()
    {
        uppershelf = GameObject.FindGameObjectWithTag("uppershelf").GetComponent<Collider2D>();
        lowershelf = GameObject.FindGameObjectWithTag("lowershelf").GetComponent<Collider2D>();
        table = GameObject.FindGameObjectWithTag("table").GetComponent<Collider2D>();
        windowsilk = GameObject.FindGameObjectWithTag("windowsilk").GetComponent<Collider2D>();
        itemName = NameGenerator.nameGenerate( WorldVariablesHandler.Instance.GetPredicateList(),
            WorldVariablesHandler.Instance.GetAdjectiveList1(),
            WorldVariablesHandler.Instance.GetAdjectiveList2(),
            tag);
        WorldVariablesHandler.Instance.nameList.AddLast(itemName);
        Debug.Log(itemName);
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
        if (uppershelf == null) {
            return;
        }
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), uppershelf, true);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), lowershelf, true);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), table, true);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), windowsilk, true);
    }

    void Update()
    {
        if (!OnShelf) {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -2f));
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "uppershelf" && !OnShelf) {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>(), true);
        }
        if (other.gameObject.tag == "floor" && itemName == null) {

            itemName = NameGenerator.nameGenerate(WorldVariablesHandler.Instance.GetPredicateList(),
                WorldVariablesHandler.Instance.GetAdjectiveList1(),
                WorldVariablesHandler.Instance.GetAdjectiveList2(),
                tag);

            while (WorldVariablesHandler.Instance.itemDictionary.ContainsKey(itemName))
            {
                itemName = NameGenerator.nameGenerate(WorldVariablesHandler.Instance.GetPredicateList(),
                    WorldVariablesHandler.Instance.GetAdjectiveList1(),
                    WorldVariablesHandler.Instance.GetAdjectiveList2(),
                    tag);
            }
            WorldVariablesHandler.Instance.nameList.AddLast(itemName);
            WorldVariablesHandler.Instance.itemDictionary.Add(itemName, this);
            Debug.Log(itemName);
        }
    }

    public void DestroyObject() {
        OnShelf = false;
        itemName = null;
        ObjectPooler.Instance.AddToQueue(tag, gameObject);
        WorldVariablesHandler.Instance.nameList.Remove(itemName);
    }
}
