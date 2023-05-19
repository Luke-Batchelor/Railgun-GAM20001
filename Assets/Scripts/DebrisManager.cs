using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

public class DebrisManager : MonoBehaviour
{
    // Debris Data
    [SerializeField] GameObject _debrisPrefab;
    [SerializeField] Transform _debrisGrouping;
    [SerializeField] int _debrisMaxCount;
    [SerializeField] List<Transform> _debrisSpawnPosList;

    // Debris List
    List<GameObject> _debrisPool;

    // Satellite Cache
    [SerializeField] GameObject _satellite;

    // Spawn Data
    [SerializeField] float _minSpawnTime;
    [SerializeField] float _maxSpawnTime;
    [SerializeField] int _changeSpawnTimeAfter;
    [SerializeField] float _changeSpawnTimeBy;
    float _curSpawnTime;

    private void OnEnable()
    {
        Debris.DebrisShotEvent += DebrisShotEventHandler;
        Satellite.SatelliteHitEvent += SatelliteHitEventHandler;
        UIManager.RestartGameEvent += StartSpawning;
    }

    private void OnDisable()
    {
        Debris.DebrisShotEvent -= DebrisShotEventHandler;
        Satellite.SatelliteHitEvent -= SatelliteHitEventHandler;
        UIManager.RestartGameEvent -= StartSpawning;
    }

    void Start()
    {
        // Create Debris pool and assign to parent object for clean hierarchy
        _debrisPool = ObjectPooler.CreateObjectPool(_debrisMaxCount, _debrisPrefab);
        ObjectPooler.AssignParentGroup(_debrisPool, _debrisGrouping);
        StartSpawning();

        // Set spawn time data
        _curSpawnTime = _maxSpawnTime;
    }

    // Spawns debris
    IEnumerator SpawnDebris()
    {
        while (true)
        {
            yield return new WaitForSeconds(_curSpawnTime);

            GameObject debris = ObjectPooler.GetPooledObject(_debrisPool);

            // Check there is available debris to spawn
            if (debris != null)
            {
                Transform spawn = SelectSpawnPoint();
                // Select a random spawn point
                if (spawn != null)
                {
                    debris.transform.position = spawn.position;
                    debris.transform.rotation = spawn.rotation;
                    debris.SetActive(true);
                }
            }
        }
    }

    // Selects spawn point for debris
    Transform SelectSpawnPoint()
    {
        // Check that spawn points exist
        if (_debrisSpawnPosList.Count != 0)
        {
            Transform spawn = _debrisSpawnPosList[Random.Range(0, _debrisSpawnPosList.Count)];

            // Ensure debris doesn't spawn within 2 units from satellite 
            while(Mathf.Abs((spawn.position - _satellite.transform.position).magnitude) < 2f)
            {
                spawn = _debrisSpawnPosList[Random.Range(0, _debrisSpawnPosList.Count)];
            }

            return spawn;
        }
        else
        {
            return null;
        }
    }

    void DebrisShotEventHandler(GameObject debrisHit)
    {
        // Deactivate debris
        debrisHit.SetActive(false);
    }

    void SatelliteHitEventHandler()
    {
        StopSpawning();
    }

    void StopSpawning()
    {
        // Stop spawning debris and return debris to object pooler
        StopAllCoroutines();
        ObjectPooler.ReturnObjectsToPool(_debrisPool);
    }

    void StartSpawning()
    {
        StartCoroutine(SpawnDebris());
    }
}
