using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float minSpawnInterval = 0.9f;
    public float maxSpawnInterval = 1.8f;
    public float minEnemySpeed = 2.5f;
    public float maxEnemySpeed = 4.5f;

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
            SpawnEnemy();
            timer = 0f;
            ScheduleNext();
        }
    }

    private void ScheduleNext()
    {
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void SpawnEnemy()
    {
        GameObject obj = new GameObject("Enemy");
        float y = Random.Range(CameraBounds.MinY + 0.5f, CameraBounds.MaxY - 0.5f);
        obj.transform.position = new Vector3(CameraBounds.MaxX + 1f, y, 0f);

        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = SpriteFactory.GetEnemySprite();
        sr.sortingOrder = 4;

        CircleCollider2D col = obj.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.4f;

        Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        // Necessario para gerar OnTriggerEnter2D contra outros corpos Kinematic (bala, jogador).
        rb.useFullKinematicContacts = true;

        Enemy enemy = obj.AddComponent<Enemy>();
        enemy.moveSpeed = Random.Range(minEnemySpeed, maxEnemySpeed);
    }
}
