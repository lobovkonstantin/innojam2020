using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public List<string> tags;
    public static ObjectPooler Instance;
    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        tags = new List<string>();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach(Pool pool in pools) {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++) {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            tags.Add(pool.tag);
            poolDictionary.Add(pool.tag, objectPool);
        }
    }


    public IPooledObject SpawnFromPool(string tag, Vector2 position, Quaternion rotation, int index) {
        if (!poolDictionary.ContainsKey(tag) || poolDictionary[tag].Count == 0) {
            return null; 
        }
        GameObject objToSpawn = poolDictionary[tag].Dequeue();
        objToSpawn.SetActive(true);

        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;

        IPooledObject pooled = objToSpawn.GetComponent<IPooledObject>();
        if (pooled != null) {
            pooled.index = index;
            pooled.OnObjectSpawn();

        }

        return pooled;
    }

    public void AddToQueue(string tag, GameObject objToSpawn) {
        objToSpawn.SetActive(false);
        poolDictionary[tag].Enqueue(objToSpawn);
    }
}
