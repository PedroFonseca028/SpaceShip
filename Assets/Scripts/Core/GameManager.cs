using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameObject player;
    private PlayerController playerController;
    private PlayerShooting playerShooting;
    private PlayerHealth playerHealth;
    private Vector3 playerSpawnPosition;
    private bool isGameOver;
    private bool isWon;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if ((isGameOver || isWon) && Input.GetKeyDown(KeyCode.R)) Restart();
    }

    public void RegisterPlayer(GameObject playerObject, Vector3 spawnPosition)
    {
        player = playerObject;
        playerSpawnPosition = spawnPosition;
        playerController = playerObject.GetComponent<PlayerController>();
        playerShooting = playerObject.GetComponent<PlayerShooting>();
        playerHealth = playerObject.GetComponent<PlayerHealth>();
    }

    public void GameOver()
    {
        if (isGameOver || isWon) return;
        isGameOver = true;

        if (playerController != null) playerController.enabled = false;
        if (playerShooting != null) playerShooting.enabled = false;

        SetSpawnersActive(false);

        int finalScore = ScoreManager.Instance != null ? ScoreManager.Instance.Score : 0;
        UIController.Instance?.ShowGameOver(finalScore);
    }

    public void Victory()
    {
        if (isGameOver || isWon) return;
        isWon = true;

        if (playerController != null) playerController.enabled = false;
        if (playerShooting != null) playerShooting.enabled = false;

        SetSpawnersActive(false);

        int finalScore = ScoreManager.Instance != null ? ScoreManager.Instance.Score : 0;
        UIController.Instance?.ShowVictory(finalScore);
    }

    private void Restart()
    {
        isGameOver = false;
        isWon = false;

        foreach (Bullet b in FindObjectsByType<Bullet>(FindObjectsSortMode.None)) Destroy(b.gameObject);
        foreach (Enemy e in FindObjectsByType<Enemy>(FindObjectsSortMode.None)) Destroy(e.gameObject);
        foreach (PowerUp p in FindObjectsByType<PowerUp>(FindObjectsSortMode.None)) Destroy(p.gameObject);

        if (player != null)
        {
            player.transform.position = playerSpawnPosition;
            if (playerHealth != null) playerHealth.ResetHealth();
            if (playerController != null) playerController.enabled = true;
            if (playerShooting != null) playerShooting.enabled = true;
        }

        ScoreManager.Instance?.ResetScore();
        SlowMotionController.Instance?.ResetState();
        UIController.Instance?.HideGameOver();
        UIController.Instance?.HideVictory();

        SetSpawnersActive(true);
    }

    private void SetSpawnersActive(bool active)
    {
        foreach (EnemySpawner s in FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None)) s.enabled = active;
        foreach (PowerUpSpawner s in FindObjectsByType<PowerUpSpawner>(FindObjectsSortMode.None)) s.enabled = active;
    }
}