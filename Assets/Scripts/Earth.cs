using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        _rb.angularVelocity = Vector3.up * _rotationSpeed;
    }
}
