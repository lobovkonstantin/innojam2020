using UnityEngine;

public interface IPooledObject
{
    int index { get; set; }
    bool OnShelf { get; set; }
    void OnObjectSpawn();
    void DropObject();
    void DestroyObject();
}
