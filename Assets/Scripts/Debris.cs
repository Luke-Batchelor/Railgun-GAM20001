using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    public static Action<GameObject> DebrisShotEvent;

    [SerializeField] float _angularSpeed;
    [SerializeField] int _pointValue;

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

    public int GetPointValue()
    {
        return _pointValue;
    }
}
