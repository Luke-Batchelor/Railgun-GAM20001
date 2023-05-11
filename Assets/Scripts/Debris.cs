using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    public static Action DebrisCollidedEvent; 
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
        //transform.RotateAround(Vector3.zero, Vector3.back, _angularSpeed * Time.deltaTime);

        transform.Translate(Vector3.up * Mathf.Sin(Time.time) * Time.deltaTime, SpawnPoint);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if debris hit satellite
        if (collision.gameObject.CompareTag("Satellite"))
        {
            // Alert DebrisManager of collision
            DebrisCollidedEvent?.Invoke();
        }
    }

    public void OnRailgunHit()
    {
        // Alert DebrisManager of shot
        DebrisShotEvent?.Invoke(this.gameObject);
    }    
}
