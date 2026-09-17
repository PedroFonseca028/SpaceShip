using UnityEngine;

// Ponto de entrada unico do jogo: coloque este componente em um GameObject vazio
// em uma cena vazia e de Play. Ele monta a camera, o fundo em parallax, a nave,
// os spawners de inimigos/power-up, o placar e a UI, tudo via codigo.
public class GameBootstrapper : MonoBehaviour
{
    [Header("Camera")]
    public float cameraSize = 5f;

    [Header("Player")]
    public float playerStartOffsetFromLeft = 2f;

    void Awake()
    {
        SetupCamera();
        CameraBounds.Recalculate();

        SpawnBackground();

        Vector3 spawnPosition = new Vector3(CameraBounds.MinX + playerStartOffsetFromLeft, 0f, 0f);
        GameObject player = SpawnPlayer(spawnPosition);

        SpawnManagers();

        // GameManager e criado dentro de SpawnManagers(), entao ja existe neste ponto.
        GameManager.Instance.RegisterPlayer(player, spawnPosition);
    }

    void LateUpdate()
    {
        CameraBounds.Recalculate();
    }

    private void SetupCamera()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            GameObject camObj = new GameObject("Main Camera");
            camObj.tag = "MainCamera";
            cam = camObj.AddComponent<Camera>();
            camObj.AddComponent<AudioListener>();
        }

        cam.orthographic = true;
        cam.orthographicSize = cameraSize;
        cam.backgroundColor = new Color(0.02f, 0.02f, 0.06f);
        cam.transform.position = new Vector3(0f, 0f, -10f);
    }

    private void SpawnBackground()
    {
        // Slide: "Crie um objeto vazio dentro do main camera... Chame este objeto de Background".
        GameObject backgroundRoot = new GameObject("Background");
        backgroundRoot.transform.SetParent(Camera.main.transform, true);

        CreateParallaxLayer(backgroundRoot.transform, "FarStars", sortingOrder: 0, starCount: 40,
            starColor: new Color(1f, 1f, 1f, 0.5f), parallaxEffect: 0.3f, baseSpeed: 2.5f, seed: 1);

        CreateParallaxLayer(backgroundRoot.transform, "NearStars", sortingOrder: 1, starCount: 80,
            starColor: new Color(1f, 1f, 1f, 0.9f), parallaxEffect: 0.7f, baseSpeed: 2.5f, seed: 2);
    }

    private void CreateParallaxLayer(Transform parent, string layerName, int sortingOrder, int starCount,
        Color starColor, float parallaxEffect, float baseSpeed, int seed)
    {
        float tileWidth = (CameraBounds.MaxX - CameraBounds.MinX) * 1.05f;
        float tileHeight = (CameraBounds.MaxY - CameraBounds.MinY) * 1.05f;
        float actualTileWidth = tileWidth;

        for (int i = 0; i < 2; i++)
        {
            GameObject tile = new GameObject(layerName + "_" + i);
            tile.transform.SetParent(parent, false);

            SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
            sr.sprite = SpriteFactory.CreateStarLayerSprite(tileWidth, tileHeight, starCount, starColor, seed + i);
            sr.sortingOrder = sortingOrder;

            if (i == 0) actualTileWidth = sr.sprite.bounds.size.x;
            tile.transform.position = new Vector3(actualTileWidth * i, 0f, 0f);

            Parallax parallax = tile.AddComponent<Parallax>();
            parallax.parallaxEffect = parallaxEffect;
            parallax.baseSpeed = baseSpeed;
            parallax.tileCount = 2;
        }
    }

    private GameObject SpawnPlayer(Vector3 spawnPosition)
    {
        GameObject player = new GameObject("Player");
        player.tag = "Player";
        player.transform.position = spawnPosition;

        SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
        sr.sprite = SpriteFactory.GetShipSprite();
        sr.sortingOrder = 10;

        CircleCollider2D col = player.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.3f;

        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        // Necessario para gerar OnTriggerEnter2D contra inimigos e power-ups (tambem Kinematic).
        rb.useFullKinematicContacts = true;

        player.AddComponent<PlayerController>();
        player.AddComponent<PlayerHealth>();
        player.AddComponent<PlayerShooting>();

        return player;
    }

    private void SpawnManagers()
    {
        GameObject scoreObj = new GameObject("ScoreManager");
        scoreObj.AddComponent<ScoreManager>();

        GameObject slowMoObj = new GameObject("SlowMotionController");
        slowMoObj.AddComponent<SlowMotionController>();

        GameObject gmObj = new GameObject("GameManager");
        gmObj.AddComponent<GameManager>();

        // UIController assina ScoreManager.OnScoreChanged no proprio Awake,
        // por isso o ScoreManager precisa ser criado antes dele.
        GameObject uiObj = new GameObject("UIController");
        uiObj.AddComponent<UIController>();

        GameObject enemySpawnerObj = new GameObject("EnemySpawner");
        enemySpawnerObj.AddComponent<EnemySpawner>();

        GameObject powerUpSpawnerObj = new GameObject("PowerUpSpawner");
        powerUpSpawnerObj.AddComponent<PowerUpSpawner>();
    }
}
