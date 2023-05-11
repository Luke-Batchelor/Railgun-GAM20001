using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Satellite : MonoBehaviour
{
    [SerializeField] private float _angularSpeed;

    public static Action SatelliteHitEvent;

    bool _isHit;

    private void OnEnable()
    {
        UIManager.RestartGameEvent += RestartGameEventHandler;
    }

    private void OnDisable()
    {
        UIManager.RestartGameEvent -= RestartGameEventHandler;
    }

    private void Start()
    {
        _isHit = false;
    }

    void Update()
    {
        if (!_isHit)
        {
            // Orbit around Earth
            transform.RotateAround(Vector3.zero, Vector3.back, _angularSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if debris hit satellite
        if(collision.gameObject.CompareTag("Debris"))
        {
            _isHit = true;
            SatelliteHitEvent?.Invoke();
        }
    }

    void RestartGameEventHandler()
    {
        _isHit = false;
    }
}
