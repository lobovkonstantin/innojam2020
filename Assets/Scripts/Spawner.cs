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
        public int index;
        public float timer;
    }

    public static Spawner Instance;

    void Start()
    {
        Instance = this;
        int i = 0;
        spawnPoints = new List<SpawnPoint>();
        foreach(Transform child in transform) {
            SpawnPoint spawnPoint = new SpawnPoint();
            spawnPoint.position = child.transform.position;
            spawnPoint.slotIsEmpty = true;
            spawnPoint.index = i;
            spawnPoints.Add(spawnPoint);
            i++;
        }
        pooler = ObjectPooler.Instance;
    }

    public void OnItemDropped(int index) {
        foreach(SpawnPoint spawnPoint in spawnPoints) {
            if (spawnPoint.index == index) {
                spawnPoint.slotIsEmpty = true;
                spawnPoint.timer = 5;
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        foreach(SpawnPoint spawnPoint in spawnPoints) {
            if (spawnPoint.slotIsEmpty) {
                if (spawnPoint.timer > 0) {
                    spawnPoint.timer -= Time.fixedDeltaTime;
                    return;
                }
                pooler.SpawnFromPool(pooler.tags[Random.Range(0, pooler.tags.Count)], spawnPoint.position, Quaternion.identity, spawnPoint.index);
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
