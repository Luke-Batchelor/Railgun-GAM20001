using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _gameOverScreen;

    public static Action RestartGameEvent;

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

    public void RestartGame()
    {
        ShowGameOverScreen(false);
        RestartGameEvent?.Invoke();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void SatelliteHitEventHandler()
    {
        ShowGameOverScreen(true);
    }
}
