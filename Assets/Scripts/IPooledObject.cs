using UnityEngine;

public interface IPooledObject
{ 
    string itemName { get; set; }
    int index { get; set; }
    bool OnShelf { get; set; }
    void OnObjectSpawn();
    void DropObject();
    void DestroyObject();
}
