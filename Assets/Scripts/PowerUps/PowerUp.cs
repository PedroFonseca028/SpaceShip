using UnityEngine;

// Power-up coletavel: ao ser pego pelo player, dispara o efeito de "bullet time"
// (ver SlowMotionController), dando vantagem ao jogador.
public class PowerUp : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public float slowMotionDuration = 4f;

    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        if (transform.position.x < CameraBounds.MinX - 2f) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerHealth>() != null)
        {
            SlowMotionController.Instance?.Trigger(slowMotionDuration);
            Destroy(gameObject);
        }
    }
}
