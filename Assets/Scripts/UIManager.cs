using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _gameOverScreen;
    [SerializeField] TextMeshProUGUI _scoreText;

    public static Action RestartGameEvent;

    void OnEnable()
    {
        Satellite.SatelliteHitEvent += SatelliteHitEventHandler;
        ScoreManager.UpdateScoreEvent += UpdateScoreEventHandler;
    }

    void OnDisable()
    {
        Satellite.SatelliteHitEvent -= SatelliteHitEventHandler;
        ScoreManager.UpdateScoreEvent -= UpdateScoreEventHandler;
    }

    void Start()
    {
        ShowGameOverScreen(false);
    }

    public void ShowGameOverScreen(bool flag)
    {
        _gameOverScreen.SetActive(flag);

        if (flag)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
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

    void UpdateScoreEventHandler(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
    }
}
