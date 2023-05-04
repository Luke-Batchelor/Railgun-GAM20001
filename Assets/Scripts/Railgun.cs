using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : MonoBehaviour
{
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector3 lookDir = mousePos - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(lookDir, Vector3.back);
        _rb.MoveRotation(lookRot);
    }

    void FixedUpdate()
    {
        
    }
}
