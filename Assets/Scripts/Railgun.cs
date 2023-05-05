using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : MonoBehaviour
{
    // Components
    private Rigidbody2D _rb;
    private LineRenderer _lr;

    // Inputs
    private Vector3 _lookDir;
    private float _mouseDistance;
    private Vector3 _mousePos;

    // Target Info
    RaycastHit2D _hit;
    bool _isOnTarget;

    // Beam
    [SerializeField] float _beamDuration;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _lr = GetComponent<LineRenderer>();
        _lr.enabled = false;
    }

    void Update()
    {
        RotateGun();

        if (Input.GetMouseButtonDown(0))
        {
            FireGun();
        }
    }

    void FixedUpdate()
    {
        _hit = Physics2D.Raycast(transform.position, _lookDir, _mouseDistance);

        if (_hit)
        {
            if (_hit.collider.CompareTag("Debris"))
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
    }

    void RotateGun()
    {
        // Get mouse position
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePos.z = 0;

        _mouseDistance = (_mousePos - transform.position).magnitude;

        _lookDir = _mousePos - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(_lookDir, Vector3.back);
        _rb.MoveRotation(lookRot);
    }

    void FireGun()
    {
        StartCoroutine(BeamActive());

        _lr.SetPosition(0, transform.position);

        if (_isOnTarget)
        {
            _hit.collider.GetComponent<Debris>().OnRailgunHit();
            _isOnTarget = false;
            _lr.SetPosition(1, _hit.point);
        }
        else
        {
            _lr.SetPosition(1, _mousePos);
        }
    }

    IEnumerator BeamActive()
    {
        _lr.enabled = true;
        yield return new WaitForSeconds(_beamDuration);
        _lr.enabled = false;
    }
}
