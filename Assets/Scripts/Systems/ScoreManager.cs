using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int Score { get; private set; }
    public int slowMotionScoreInterval = 500;
    public float slowMotionDuration = 3f;

    [Header("Vitoria")]
    public int winScore = 2000;

    public event Action<int> OnScoreChanged;

    private int nextSlowMotionThreshold;
    private bool hasWon;

    void Awake()
    {
        Instance = this;
        nextSlowMotionThreshold = slowMotionScoreInterval;
    }

    public void AddScore(int amount)
    {
        if (hasWon) return;

        Score += amount;
        OnScoreChanged?.Invoke(Score);

        if (slowMotionScoreInterval > 0 && Score >= nextSlowMotionThreshold)
        {
            SlowMotionController.Instance?.Trigger(slowMotionDuration);
            nextSlowMotionThreshold += slowMotionScoreInterval;
        }

        if (winScore > 0 && Score >= winScore)
        {
            hasWon = true;
            GameManager.Instance?.Victory();
        }
    }

    public void ResetScore()
    {
        Score = 0;
        nextSlowMotionThreshold = slowMotionScoreInterval;
        hasWon = false;
        OnScoreChanged?.Invoke(Score);
    }
}