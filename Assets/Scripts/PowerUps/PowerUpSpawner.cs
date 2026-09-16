using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public float minSpawnInterval = 8f;
    public float maxSpawnInterval = 14f;
    public float powerUpSpeed = 2.5f;
    public float slowMotionDuration = 4f;

    private float timer;
    private float nextSpawnTime;

    void OnEnable()
    {
        timer = 0f;
        ScheduleNext();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextSpawnTime)
        {
            SpawnPowerUp();
            timer = 0f;
            ScheduleNext();
        }
    }

    private void ScheduleNext()
    {
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void SpawnPowerUp()
    {
        GameObject obj = new GameObject("PowerUp");
        float y = Random.Range(CameraBounds.MinY + 0.5f, CameraBounds.MaxY - 0.5f);
        obj.transform.position = new Vector3(CameraBounds.MaxX + 1f, y, 0f);

        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = SpriteFactory.GetPowerUpSprite();
        sr.sortingOrder = 4;

        CircleCollider2D col = obj.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.25f;

        Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        PowerUp powerUp = obj.AddComponent<PowerUp>();
        powerUp.moveSpeed = powerUpSpeed;
        powerUp.slowMotionDuration = slowMotionDuration;
    }
}
