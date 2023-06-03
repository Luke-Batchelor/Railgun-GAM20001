using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _gameOverScreen;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] GameObject _pauseScreen;

    bool _paused;
    bool _gameOver;

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
        _paused = false;
        _gameOver = false;
        ShowGameOverScreen(_gameOver);
        PauseGame(_paused);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_gameOver)
        {
            _paused = !_paused;

            PauseGame(_paused);
        }
    }

    public void ShowGameOverScreen(bool gameOver)
    {
        _gameOverScreen.SetActive(gameOver);
        _gameOver = gameOver;

        Time.timeScale = gameOver ? 0 : 1;
    }

    // Pause or unpause the game
    public void PauseGame(bool paused)
    {
        _pauseScreen.SetActive(paused);

        Time.timeScale = paused ? 0 : 1;
    }

    public void RestartGame()
    {
        _paused = false;
        _gameOver = false;
        ShowGameOverScreen(_gameOver);
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
