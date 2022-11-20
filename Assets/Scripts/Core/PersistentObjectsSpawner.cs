using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Core {
public class PersistentObjectsSpawner : MonoBehaviour
{
    
    [SerializeField] GameObject persistenObjectsPrefab;
    static bool hasSpawned = false;
    private void Awake() {
        if (hasSpawned) {
            return;
        }
        else {
            hasSpawned = true;
            SpawnPersistentObjects();
        }
    }

    private void SpawnPersistentObjects() {
        GameObject persistentObject = Instantiate(persistenObjectsPrefab);
        DontDestroyOnLoad(persistentObject);
    }



}
}//namespace
