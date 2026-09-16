using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public float invulnerabilityDuration = 1.2f;

    private int currentHealth;
    private bool isInvulnerable;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable || currentHealth <= 0) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            GameManager.Instance?.GameOver();
        }
        else
        {
            StartCoroutine(InvulnerabilityRoutine());
        }
    }

    public void ResetHealth()
    {
        StopAllCoroutines();
        currentHealth = maxHealth;
        isInvulnerable = false;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = true;
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float elapsed = 0f;
        while (elapsed < invulnerabilityDuration)
        {
            if (sr != null) sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        if (sr != null) sr.enabled = true;
        isInvulnerable = false;
    }
}
