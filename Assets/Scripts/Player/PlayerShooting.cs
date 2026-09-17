using UnityEngine;

// Capacidade da nave de atirar, sempre para frente (eixo +X local).
public class PlayerShooting : MonoBehaviour
{
    public float fireRate = 5f;
    public float bulletSpeed = 14f;
    public Vector3 muzzleOffset = new Vector3(0.5f, 0f, 0f);

    private float cooldown;

    void Update()
    {
        cooldown -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && cooldown <= 0f)
        {
            Shoot();
            cooldown = 1f / fireRate;
        }
    }

    private void Shoot()
    {
        GameObject obj = new GameObject("Bullet");
        obj.transform.position = transform.position + muzzleOffset;

        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = SpriteFactory.GetBulletSprite();
        sr.sortingOrder = 6;

        CircleCollider2D col = obj.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.1f;

        Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        // Sem isso, dois Rigidbody2D Kinematic (bala e inimigo) nao geram OnTriggerEnter2D entre si.
        rb.useFullKinematicContacts = true;

        Bullet bullet = obj.AddComponent<Bullet>();
        bullet.speed = bulletSpeed;
    }
}