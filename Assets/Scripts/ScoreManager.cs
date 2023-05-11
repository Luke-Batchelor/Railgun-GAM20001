using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static Action<int> UpdateScoreEvent;

    [SerializeField] int _score;

    private void OnEnable()
    {
        Debris.DebrisShotEvent += DebrisShotEventHandler;
        UIManager.RestartGameEvent += RestartGameEventHandler;
    }

    private void OnDisable()
    {
        Debris.DebrisShotEvent -= DebrisShotEventHandler;
        UIManager.RestartGameEvent -= RestartGameEventHandler;
    }

    private void Start()
    {
        ResetScore();
        UpdateScore();
    }

    void ResetScore()
    {
        _score = 0;
        UpdateScore();
    }

    void UpdateScore()
    {
        UpdateScoreEvent?.Invoke(_score);
    }

    void DebrisShotEventHandler(GameObject debris)
    {
        _score += debris.GetComponent<Debris>().GetPointValue();
        UpdateScore();
    }

    void RestartGameEventHandler()
    {
        ResetScore();
    }
}
