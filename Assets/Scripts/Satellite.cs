using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Satellite : MonoBehaviour
{
    [SerializeField] private float _angularSpeed;
    [SerializeField] private float _blinkTime;
    [SerializeField] private float _alertBlinkTime;
    ParticleSystem _particleSys;
     
    bool _isHit;

    public static Action SatelliteHitEvent;

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
        _particleSys = GetComponent<ParticleSystem>();
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

    // Red alert if Satellite gets close to Debris
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Debris"))
        {
            var particleSysMain = _particleSys.main;
            particleSysMain.startColor = Color.red;
            particleSysMain.startLifetime = _alertBlinkTime;
        }
    }

    // Satellite goes back to normal when Debris is gone
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Debris"))
        {
            var particleSysMain = _particleSys.main;
            particleSysMain.startColor = Color.green;
            particleSysMain.startLifetime = _blinkTime;
        }
    }

    void RestartGameEventHandler()
    {
        _isHit = false;
    }
}
