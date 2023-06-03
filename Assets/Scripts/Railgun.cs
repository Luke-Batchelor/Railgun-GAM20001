using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : MonoBehaviour
{
    // Components
    private LineRenderer _lr;
    private AudioSource _audio;

    // Inputs
    private Vector3 _lookDir;
    Quaternion _lookRot;
    private float _mouseDistance;
    private Vector3 _mousePos;

    // Target Info
    RaycastHit2D _hit;
    bool _isOnTarget;

    // Beam
    [Header("Beam Data")]
    [SerializeField] float _beamDuration;
    [SerializeField] Transform _beamStartPos;

    [Header("Railgun Movement Data")]
    [SerializeField] float _rotationSpeed;


    void Start()
    {
        _lr = GetComponent<LineRenderer>();
        _audio = GetComponent<AudioSource>();
        _lr.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Mathf.Abs(Quaternion.Angle(_lookRot, transform.rotation)) < 0.2f)
        {
            FireGun();
        }

        RotateGun();
    }

    void FixedUpdate()
    {
        _hit = Physics2D.Raycast(transform.position, _lookDir, 100f, LayerMask.GetMask("Debris"));

        if (_hit)
        {
            _isOnTarget = true;
            Debug.DrawRay(transform.position, _lookDir * _mouseDistance, Color.green);
        }
        else
        {
            _isOnTarget = false;
            Debug.DrawRay(transform.position, _lookDir * _mouseDistance, Color.red);
        }
    }

    void RotateGun()
    {
        // Get mouse position
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePos.z = 0;

        _mouseDistance = (_mousePos - transform.position).magnitude;

        _lookDir = _mousePos - transform.position;

        Quaternion fullRot = Quaternion.LookRotation(transform.forward, _lookDir);
        _lookRot = Quaternion.identity;
        _lookRot.eulerAngles = new Vector3(0, 0, fullRot.eulerAngles.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRot, _rotationSpeed * Time.deltaTime);
    }

    void FireGun()
    {
        StartCoroutine(BeamActive());
        PlayAudio();
        _lr.SetPosition(0, _beamStartPos.position);

        if (_isOnTarget && _hit)
        {
            _hit.collider.GetComponent<Debris>().OnRailgunHit();
            _isOnTarget = false;
            _lr.SetPosition(1, _hit.point);
        }
        else
        {
            _lr.SetPosition(1, _beamStartPos.position * 15);
        }
    }

    IEnumerator BeamActive()
    {
        _lr.enabled = true;
        yield return new WaitForSeconds(_beamDuration);
        _lr.enabled = false;
    }

    void PlayAudio()
    {
        _audio.Play();
    }
}
