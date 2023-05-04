using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector3 _lookDir;
    private float _mouseDistance;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        _mouseDistance = (mousePos - transform.position).magnitude; 

        _lookDir = mousePos - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(_lookDir, Vector3.back);
        _rb.MoveRotation(lookRot);
    }

    void FixedUpdate()
    {
        if (Physics2D.Raycast(transform.position, _lookDir, _mouseDistance))
        {
            Debug.DrawRay(transform.position, _lookDir * _mouseDistance, Color.green);
        }
        else
        {
            Debug.DrawRay(transform.position, _lookDir * _mouseDistance, Color.red);
        }
    }
}
