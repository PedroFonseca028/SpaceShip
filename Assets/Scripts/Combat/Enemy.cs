using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int maxHealth = 1;
    public int scoreValue = 100;

    private int currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Multiplicado pelo SlowMotionController: inimigos desaceleram durante o efeito,
        // mas a nave e os tiros do jogador nao (ver PlayerController/Bullet).
        float scale = SlowMotionController.CurrentScale;
        transform.position += Vector3.left * moveSpeed * Time.deltaTime * scale;

        if (transform.position.x < CameraBounds.MinX - 2f) Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            ScoreManager.Instance?.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
