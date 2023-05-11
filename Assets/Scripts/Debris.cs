using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    public static Action<GameObject> DebrisShotEvent;

    public Transform SpawnPoint;

    [SerializeField] float _angularSpeed;

    private void Awake()
    {
        // Cache initial spawn point to return to spawnpoint list in DebrisManager
        SpawnPoint = transform;
    }

    private void Update()
    {
        // Orbit code
        transform.RotateAround(Vector3.zero, Vector3.back, _angularSpeed * Time.deltaTime);
        // Move between two points
        transform.Translate(Vector3.up * Mathf.Sin(Time.time) * Time.deltaTime * 2f, transform);
    }

    public void OnRailgunHit()
    {
        // Alert DebrisManager of shot
        DebrisShotEvent?.Invoke(this.gameObject);
    }    
}
