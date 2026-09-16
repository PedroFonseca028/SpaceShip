using System.Collections;
using UnityEngine;

// Desacelera o fundo e os inimigos sem tocar em Time.timeScale, para que a
// nave e os tiros do jogador continuem em velocidade normal (vantagem do player).
public class SlowMotionController : MonoBehaviour
{
    public static SlowMotionController Instance { get; private set; }

    [Range(0.05f, 1f)] public float slowScale = 0.35f;

    private float currentScale = 1f;
    private Coroutine activeRoutine;

    public static float CurrentScale => Instance != null ? Instance.currentScale : 1f;
    public bool IsActive => currentScale < 0.999f;

    void Awake()
    {
        Instance = this;
    }

    public void Trigger(float duration)
    {
        if (activeRoutine != null) StopCoroutine(activeRoutine);
        activeRoutine = StartCoroutine(SlowMotionRoutine(duration));
    }

    public void ResetState()
    {
        if (activeRoutine != null)
        {
            StopCoroutine(activeRoutine);
            activeRoutine = null;
        }
        currentScale = 1f;
    }

    private IEnumerator SlowMotionRoutine(float duration)
    {
        currentScale = slowScale;
        yield return new WaitForSeconds(duration);
        currentScale = 1f;
        activeRoutine = null;
    }
}
