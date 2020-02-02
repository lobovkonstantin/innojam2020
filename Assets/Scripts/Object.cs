﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class Object : MonoBehaviour, IPooledObject
{
    public Canvas objectCanvasPrefab;
    public Canvas objectCanvas;
    public int index { get; set; }
    public string tag;
    public bool OnShelf { get; set; }

    public float upForce = 1f;

    public float sideForce = .15f;

    public Collider2D shelf;

    private String itemName;

    void Start()
    {
        shelf = GameObject.FindGameObjectWithTag("shelf").GetComponent<Collider2D>();
        objectCanvas = Instantiate(objectCanvasPrefab, transform.position, Quaternion.identity);

        itemName = NameGenerator.nameGenerate( WorldVariablesHandler.Instance.GetPredicateList(),
            WorldVariablesHandler.Instance.GetAdjectiveList1(),
            WorldVariablesHandler.Instance.GetAdjectiveList2(),
            tag);
        WorldVariablesHandler.Instance.nameList.AddLast(itemName);
        objectCanvas.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = itemName;
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
        if (shelf == null) {
            return;
        }
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shelf, true);
    }

    void Update()
    {
        if (!OnShelf) {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -2f));
            objectCanvas.gameObject.transform.position = new Vector2(transform.position.x, transform.position.y + 0.8f);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "shelf" && !OnShelf) {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>(), true);
        }
        if (other.gameObject.tag == "floor") {

        }
    }

    public void DestroyObject() {
        OnShelf = false;
        ObjectPooler.Instance.AddToQueue(tag, gameObject);
        WorldVariablesHandler.Instance.nameList.Remove(itemName);
    }
}
