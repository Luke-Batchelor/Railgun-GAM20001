using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Debris
{
    [SerializeField] float _lifeTime;
    float _timer;

    private void OnEnable()
    {
        _timer = 0;
    }

    public override void Update()
    {
        base.Update();

        _timer += Time.deltaTime;

        // If time is up, despawn asteroid
        if (_timer > _lifeTime)
        {
            _timer = 0;
            this.gameObject.SetActive(false);
        }
    }
}
