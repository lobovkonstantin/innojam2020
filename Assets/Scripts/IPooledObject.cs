using UnityEngine;

public interface IPooledObject
{
    bool OnShelf { get; set; }
    void OnObjectSpawn();
    void DropObject();
    void DestroyObject();
}
