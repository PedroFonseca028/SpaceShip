using UnityEngine;
using UnityEngine.UI;

// Monta a UI (placar + tela de game over) inteiramente via codigo, sem depender
// de nenhuma cena/prefab pre-existente.
public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    private Text scoreText;
    private GameObject gameOverPanel;
    private Text gameOverText;

    void Awake()
    {
        Instance = this;
        BuildUI();

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnScoreChanged += HandleScoreChanged;
    }

    private void HandleScoreChanged(int newScore)
    {
        if (scoreText != null) scoreText.text = "Score: " + newScore;
    }

    public void ShowGameOver(int finalScore)
    {
        if (gameOverText != null) gameOverText.text = "GAME OVER\nScore: " + finalScore;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void HideGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    private void BuildUI()
    {
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280f, 720f);

        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (font == null) font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        scoreText = CreateText(canvasObj.transform, "ScoreText", font, 36, TextAnchor.UpperLeft,
            new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
            new Vector2(20f, -20f), new Vector2(400f, 60f));
        scoreText.text = "Score: 0";

        gameOverPanel = new GameObject("GameOverPanel");
        gameOverPanel.transform.SetParent(canvasObj.transform, false);
        RectTransform panelRect = gameOverPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Image bg = gameOverPanel.AddComponent<Image>();
        bg.color = new Color(0f, 0f, 0f, 0.75f);

        gameOverText = CreateText(gameOverPanel.transform, "GameOverText", font, 48, TextAnchor.MiddleCenter,
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector2(0f, 20f), new Vector2(700f, 100f));

        Text restartText = CreateText(gameOverPanel.transform, "RestartText", font, 24, TextAnchor.MiddleCenter,
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector2(0f, -50f), new Vector2(700f, 40f));
        restartText.text = "Pressione R para reiniciar";

        gameOverPanel.SetActive(false);
    }

    private Text CreateText(Transform parent, string name, Font font, int fontSize, TextAnchor alignment,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 anchoredPosition, Vector2 sizeDelta)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);

        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = pivot;
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = sizeDelta;

        Text text = obj.AddComponent<Text>();
        text.font = font;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = Color.white;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        return text;
    }
}
