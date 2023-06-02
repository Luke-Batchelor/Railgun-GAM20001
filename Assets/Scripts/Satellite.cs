using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class Satellite : MonoBehaviour
{
    [SerializeField] private float _angularSpeed;
    [SerializeField] private float _detectionRadius;
    [SerializeField] private float _safeBlinkTime;
    [SerializeField] private float _safeAudioTime;
    [SerializeField] private float _alertBlinkTime;
    [SerializeField] private float _alertAudioTime;
    ParticleSystem _particleSys;
    AudioSource _audioSource;
     
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
        _audioSource = GetComponent<AudioSource>();
        ModeChange(Color.green, _safeBlinkTime, _safeAudioTime);
    }

    void Update()
    {
        if (!_isHit)
        {
            // Orbit around Earth
            transform.RotateAround(Vector3.zero, Vector3.back, _angularSpeed * Time.deltaTime);
        }

        var detected = Physics2D.OverlapCircle(transform.position, _detectionRadius, LayerMask.GetMask("Debris"));

        // Change between safe and alert mode depending on if collisions occur
        if (detected == null && !_safeMode)
        {
            ModeChange(Color.green, _safeBlinkTime, _safeAudioTime);
            SafeModeActive(true);
        }
        else if (detected != null && !_alertMode)
        {
            ModeChange(Color.red, _alertBlinkTime, _alertAudioTime);
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
    void ModeChange(Color colour, float blinkTime, float audioTime)
    {
        _particleSys.Stop();
        CancelInvoke();
        var particleSysMain = _particleSys.main;
        particleSysMain.startColor = colour;
        particleSysMain.startLifetime = blinkTime;
        InvokeRepeating("PlayAudio", 1f, audioTime);
        _particleSys.Play();
    }

    void PlayAudio()
    {
        _audioSource.Play();
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

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}
