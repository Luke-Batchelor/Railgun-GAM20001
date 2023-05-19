using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

public class DebrisManager : MonoBehaviour
{
    // Satellite Cache
    [Header("Satellite")]
    [SerializeField] GameObject _satellite;

    // Debris Data
    [Header("Debris Prefabs")]
    [SerializeField] GameObject _commonDebrisPrefab;
    [SerializeField] GameObject _uncommonDebrisPrefab;
    [SerializeField] GameObject _rareDebrisPrefab;
    [SerializeField] GameObject _asteroidPrefab;

    // Debris Grouping
    [Header("Debris Spawning Data")]
    [SerializeField] int _debrisMaxPoolCount;
    [SerializeField] Transform _commonDebrisGrouping;
    [SerializeField] Transform _uncommonDebrisGrouping;
    [SerializeField] Transform _rareDebrisGrouping;
    [SerializeField] Transform _asteroidGrouping;
    [SerializeField] List<Transform> _debrisSpawnPosList;
    [SerializeField] List<Transform> _asteroidSpawnPosList;

    // Debris List
    List<GameObject> _commonDebrisPool;
    List<GameObject> _uncommonDebrisPool;
    List<GameObject> _rareDebrisPool;
    List<GameObject> _asteroidPool;

    // Spawn Data
    [Header("Spawn Time Data")]
    [SerializeField] float _minSpawnTime;
    [SerializeField] float _maxSpawnTime;
    [SerializeField] int _changeSpawnTimeAfter;
    [SerializeField] float _changeSpawnTimeBy;
    float _curSpawnTime;

    #region Setup
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
        // Set spawn time data
        _curSpawnTime = _maxSpawnTime;

        // Create object pools
        _commonDebrisPool = CreateDebrisPools(_commonDebrisPrefab, _commonDebrisGrouping);
        _uncommonDebrisPool = CreateDebrisPools(_uncommonDebrisPrefab, _uncommonDebrisGrouping);
        _rareDebrisPool = CreateDebrisPools(_rareDebrisPrefab, _rareDebrisGrouping);
        _asteroidPool = CreateDebrisPools(_asteroidPrefab, _asteroidGrouping);

        // Start spawning
        StartSpawning(); 
    }

    // Create Debris/Asteroid pools and assign to parent object for clean hierarchy
    List<GameObject> CreateDebrisPools(GameObject prefab, Transform parent)
    {
        List<GameObject> pool = ObjectPooler.CreateObjectPool(_debrisMaxPoolCount, prefab);
        ObjectPooler.AssignParentGroup(pool, parent);
        return pool;
    }

    #endregion
    #region Spawning
    void StartSpawning()
    {
        StartCoroutine(SpawnDebris());
    }

    void StopSpawning()
    {
        // Stop spawning debris and return debris to object pools
        StopAllCoroutines();
        ObjectPooler.ReturnObjectsToPool(_commonDebrisPool);
        ObjectPooler.ReturnObjectsToPool(_uncommonDebrisPool);
        ObjectPooler.ReturnObjectsToPool(_rareDebrisPool);
        ObjectPooler.ReturnObjectsToPool(_asteroidPool);
    }

    // Spawns debris
    IEnumerator SpawnDebris()
    {
        while (true)
        {
            yield return new WaitForSeconds(_curSpawnTime);

            GameObject debris = ObjectPooler.GetPooledObject(_commonDebrisPool);

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
    #endregion

    // Event Handlers
    #region EventHandlers
    void DebrisShotEventHandler(GameObject debrisHit)
    {
        // Deactivate debris
        debrisHit.SetActive(false);
    }

    void SatelliteHitEventHandler()
    {
        StopSpawning();
    }
    #endregion
}
