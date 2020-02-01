using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    ObjectPooler pooler;
    IPooledObject spawnedObj;

    public int objectsOnShelf;
    List<SpawnPoint> spawnPoints;
    
    class SpawnPoint
    {
        public Vector2 position;
        public bool slotIsEmpty;
        public float timer;
    }

    void Start()
    {
        spawnPoints = new List<SpawnPoint>();
        foreach(Transform child in transform) {
            SpawnPoint spawnPoint = new SpawnPoint();
            spawnPoint.position = child.transform.position;
            spawnPoint.slotIsEmpty = true;
            spawnPoints.Add(spawnPoint);
        }
        pooler = ObjectPooler.Instance;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        foreach(SpawnPoint spawnPoint in spawnPoints) {
            if (spawnPoint.slotIsEmpty) {
                string tag = Random.value < 0.5f ? "glass" : "glass2";
                pooler.SpawnFromPool(tag, spawnPoint.position, Quaternion.identity);
                spawnPoint.slotIsEmpty = false;
            }
        }
        // if (objectsOnShelf < 6) {

        //     string tag = Random.value < 0.5f ? "glass" : "glass2";
        //     spawnedObj = pooler.SpawnFromPool(tag, transform.position, Quaternion.identity);
        //     spawned = true;
        // }

        // if (Input.GetKey(KeyCode.I) && spawnedObj != null) {
        //     spawnedObj.DropObject();
        // }

        // if (Input.GetKey(KeyCode.D) && spawnedObj != null) {
        //     spawnedObj.DestroyObject();
        // }
        
    }
}
