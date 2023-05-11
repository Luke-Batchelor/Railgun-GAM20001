using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _gameOverScreen;

    void OnEnable()
    {
        Satellite.SatelliteHitEvent += SatelliteHitEventHandler;
    }

    void OnDisable()
    {
        Satellite.SatelliteHitEvent -= SatelliteHitEventHandler;
    }

    void Start()
    {
        ShowGameOverScreen(false);
    }

    public void ShowGameOverScreen(bool flag)
    {
        _gameOverScreen.SetActive(flag);
    }

    void SatelliteHitEventHandler()
    {
        ShowGameOverScreen(true);
    }
}
