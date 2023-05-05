using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    public static Action DebrisCollidedEvent; 
    public static Action<GameObject> DebrisShotEvent;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if debris hit satellite
        if (collision.gameObject.CompareTag("Satellite"))
        {
            // Alert DebrisManager of collision
            DebrisCollidedEvent?.Invoke();
        }
    }

    public void OnRailgunHit()
    {
        // Alert DebrisManager of shot
        DebrisShotEvent?.Invoke(this.gameObject);
    }    
}
