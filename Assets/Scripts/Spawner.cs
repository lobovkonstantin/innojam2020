using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    ObjectPooler pooler;
    bool spawned = false;
    IPooledObject spawnedObj;
    void Start()
    {
        pooler = ObjectPooler.Instance;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!spawned) {
            spawnedObj = pooler.SpawnFromPool("cube", transform.position, Quaternion.identity);
            spawned = true;
        }

        if (Input.GetKey(KeyCode.I) && spawnedObj != null) {
            spawnedObj.DropObject();
        }

        if (Input.GetKey(KeyCode.D) && spawnedObj != null) {
            spawned = false;
            spawnedObj.DestroyObject();
        }
        
    }
}
