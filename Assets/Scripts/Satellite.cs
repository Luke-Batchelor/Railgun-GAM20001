using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Satellite : MonoBehaviour
{
    [SerializeField] private float _angularSpeed;
    [SerializeField] private float _safeBlinkTime;
    [SerializeField] private float _alertBlinkTime;
    ParticleSystem _particleSys;
     
    bool _isHit;
    bool _alertMode;
    bool _safeMode;

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
        _safeMode = true;
        _alertMode = false;
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

        var detected = Physics2D.OverlapCircle(transform.position, 11f, LayerMask.GetMask("Debris"));

        // Change between safe and alert mode depending on if collisions occur
        if (detected == null && !_safeMode)
        {
            Debug.Log("safe");
            ModeChange(Color.green, _safeBlinkTime);
            SafeModeActive(true);
        }
        else if (detected != null && !_alertMode)
        {
            Debug.Log("alert");
            ModeChange(Color.red, _alertBlinkTime);
            SafeModeActive(false);
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

    // Change between safe mode and alert mode
    void ModeChange(Color colour, float blinkTime)
    {
        _particleSys.Stop();
        var particleSysMain = _particleSys.main;
        particleSysMain.startColor = colour;
        particleSysMain.startLifetime = blinkTime;
        _particleSys.Play();
    }

    void RestartGameEventHandler()
    {
        _isHit = false;
    }

    void SafeModeActive(bool toggle)
    {
        _safeMode = toggle;
        _alertMode = !toggle;
    }
}
