using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DebrisManager : MonoBehaviour
{
    // Debris Data
    [SerializeField] GameObject _debrisPrefab;
    [SerializeField] int _debrisMaxCount;
    [SerializeField] List<Transform> _debrisSpawnList;

    // Debris List
    List<GameObject> _debrisPool;

    void Start()
    {
        // Create Debris pool
        _debrisPool = ObjectPooler.CreateObjectPool(_debrisMaxCount, _debrisPrefab);
    }

    void Update()
    {
        
    }
}
