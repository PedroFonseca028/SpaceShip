using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int Score { get; private set; }
    public int slowMotionScoreInterval = 500;
    public float slowMotionDuration = 3f;

    public event Action<int> OnScoreChanged;

    private int nextSlowMotionThreshold;

    void Awake()
    {
        Instance = this;
        nextSlowMotionThreshold = slowMotionScoreInterval;
    }

    public void AddScore(int amount)
    {
        Score += amount;
        OnScoreChanged?.Invoke(Score);

        if (slowMotionScoreInterval > 0 && Score >= nextSlowMotionThreshold)
        {
            SlowMotionController.Instance?.Trigger(slowMotionDuration);
            nextSlowMotionThreshold += slowMotionScoreInterval;
        }
    }

    public void ResetScore()
    {
        Score = 0;
        nextSlowMotionThreshold = slowMotionScoreInterval;
        OnScoreChanged?.Invoke(Score);
    }
}
