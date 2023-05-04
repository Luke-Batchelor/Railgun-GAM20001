using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : MonoBehaviour
{
    // Components
    private Rigidbody2D _rb;

    // Inputs
    private Vector3 _lookDir;
    private float _mouseDistance;

    // Target Info
    RaycastHit2D _hit;
    bool _isOnTarget;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
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

        if (_hit.collider != null)
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
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        _mouseDistance = (mousePos - transform.position).magnitude;

        _lookDir = mousePos - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(_lookDir, Vector3.back);
        _rb.MoveRotation(lookRot);
    }

    void FireGun()
    {
        if (_isOnTarget)
        {
            Debug.Log("BOOM");
            _isOnTarget = false;
        }
    }
}
