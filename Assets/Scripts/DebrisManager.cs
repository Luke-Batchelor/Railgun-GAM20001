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
    [SerializeField] List<Transform> _debrisSpawnList;

    // Debris List
    List<GameObject> _debrisPool;

    // Satellite Cache
    [SerializeField] GameObject _satellite;

    private void OnEnable()
    {
        Debris.DebrisShotEvent += DebrisShotEventHandler;
    }

    private void OnDisable()
    {
        Debris.DebrisShotEvent -= DebrisShotEventHandler;
    }

    void Start()
    {
        // Create Debris pool and assign to parent object for clean hierarchy
        _debrisPool = ObjectPooler.CreateObjectPool(_debrisMaxCount, _debrisPrefab);
        ObjectPooler.AssignParentGroup(_debrisPool, _debrisGrouping);
        InvokeRepeating("SpawnDebris", 2f, 5f);
    }

    // Spawns debris
    void SpawnDebris()
    {
        GameObject debris = ObjectPooler.GetPooledObject(_debrisPool);

        // Check there is available debris to spawn
        if (debris != null)
        {
            Transform spawn = SelectSpawnPoint();
            // Select a random spawn point
            if (spawn != null)
            {
                debris.GetComponent<Debris>().SpawnPoint = spawn;
                debris.transform.position = spawn.position;
                debris.transform.rotation = spawn.rotation;
                debris.SetActive(true);
            }
        }
    }

    // Selects spawn point for debris
    Transform SelectSpawnPoint()
    {
        // Check that all spawn points are not taken up already
        if (_debrisSpawnList.Count != 0)
        {
            Transform spawn = _debrisSpawnList[Random.Range(0, _debrisSpawnList.Count)];

            // Ensure debris doesn't spawn within 2 units from satellite 
            while(Mathf.Abs((spawn.position - _satellite.transform.position).magnitude) < 2f)
            {
                spawn = _debrisSpawnList[Random.Range(0, _debrisSpawnList.Count)];
            }

            _debrisSpawnList.Remove(spawn);
            return spawn;
        }
        else
        {
            return null;
        }
    }

    void DebrisShotEventHandler(GameObject debrisHit)
    {
        // Deactivate debris and return spawn position to spawn list
        debrisHit.SetActive(false);
        _debrisSpawnList.Add(debrisHit.GetComponent<Debris>().SpawnPoint);
    }
}
