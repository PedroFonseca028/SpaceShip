using UnityEngine;

public static class SpriteFactory
{
    private static Sprite cachedShipSprite;
    private static Sprite cachedEnemySprite;
    private static Sprite cachedBulletSprite;
    private static Sprite cachedPowerUpSprite;

    public static Sprite GetShipSprite()
    {
        if (cachedShipSprite == null) cachedShipSprite = CreateShipSprite();
        return cachedShipSprite;
    }

    public static Sprite GetEnemySprite()
    {
        if (cachedEnemySprite == null) cachedEnemySprite = CreateEnemySprite();
        return cachedEnemySprite;
    }

    public static Sprite GetBulletSprite()
    {
        if (cachedBulletSprite == null) cachedBulletSprite = CreateBulletSprite();
        return cachedBulletSprite;
    }

    public static Sprite GetPowerUpSprite()
    {
        if (cachedPowerUpSprite == null) cachedPowerUpSprite = CreatePowerUpSprite();
        return cachedPowerUpSprite;
    }

    public static Sprite CreateShipSprite(float pixelsPerUnit = 64f)
    {
        int w = 64;
        int h = 40;
        Texture2D tex = CreateTexture(w, h);
        Color32[] pixels = new Color32[w * h];

        Color hullColor = new Color(0.75f, 0.85f, 1f);
        Color cockpitColor = new Color(0.2f, 0.9f, 1f);
        Color flameColor = new Color(1f, 0.6f, 0.1f);

        Vector2 nose = new Vector2(w - 3, h / 2f);
        Vector2 top = new Vector2(6, h * 0.12f);
        Vector2 bottom = new Vector2(6, h * 0.88f);

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                Vector2 p = new Vector2(x + 0.5f, y + 0.5f);
                Color32 c = new Color32(0, 0, 0, 0);

                if (PointInTriangle(p, nose, top, bottom))
                {
                    float distToNose = Vector2.Distance(p, nose);
                    c = distToNose < 6f
                        ? Color.Lerp(hullColor, cockpitColor, 1f - distToNose / 6f)
                        : hullColor;
                }

                if (x < 6 && Mathf.Abs(y - h / 2f) < (6 - x) * 0.9f)
                {
                    float t = x / 6f;
                    c = Color32.Lerp(flameColor, new Color32(0, 0, 0, 0), t);
                }

                pixels[y * w + x] = c;
            }
        }

        tex.SetPixels32(pixels);
        return ToSprite(tex, pixelsPerUnit, new Vector2(0.5f, 0.5f));
    }

    public static Sprite CreateEnemySprite(float pixelsPerUnit = 64f)
    {
        int size = 56;
        Texture2D tex = CreateTexture(size, size);
        Color32[] pixels = new Color32[size * size];

        Color bodyColor = new Color(0.9f, 0.25f, 0.25f);
        Color coreColor = new Color(1f, 0.85f, 0.2f);
        Vector2 center = new Vector2(size / 2f, size / 2f);
        Vector2 left = new Vector2(size * 0.08f, size / 2f);
        Vector2 right = new Vector2(size * 0.92f, size / 2f);
        Vector2 top = new Vector2(size / 2f, size * 0.08f);
        Vector2 bottom = new Vector2(size / 2f, size * 0.92f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Vector2 p = new Vector2(x + 0.5f, y + 0.5f);
                Color32 c = new Color32(0, 0, 0, 0);

                bool inDiamond = PointInTriangle(p, left, top, right) || PointInTriangle(p, left, bottom, right);
                if (inDiamond)
                {
                    float d = Vector2.Distance(p, center);
                    c = d < size * 0.18f ? coreColor : bodyColor;
                }

                pixels[y * size + x] = c;
            }
        }

        tex.SetPixels32(pixels);
        return ToSprite(tex, pixelsPerUnit, new Vector2(0.5f, 0.5f));
    }

    public static Sprite CreateBulletSprite(float pixelsPerUnit = 64f)
    {
        int size = 16;
        Texture2D tex = CreateTexture(size, size);
        Color32[] pixels = new Color32[size * size];

        Color32 core = new Color(1f, 0.95f, 0.4f);
        Color32 transparent = new Color32(0, 0, 0, 0);
        Vector2 center = new Vector2(size / 2f, size / 2f);
        float radius = size * 0.4f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Vector2 p = new Vector2(x + 0.5f, y + 0.5f);
                float d = Vector2.Distance(p, center);
                pixels[y * size + x] = d <= radius ? core : transparent;
            }
        }

        tex.SetPixels32(pixels);
        return ToSprite(tex, pixelsPerUnit, new Vector2(0.5f, 0.5f));
    }

    public static Sprite CreatePowerUpSprite(float pixelsPerUnit = 64f)
    {
        int size = 32;
        Texture2D tex = CreateTexture(size, size);
        Color32[] pixels = new Color32[size * size];

        Color32 glow = new Color(0.4f, 1f, 0.6f);
        Color32 transparent = new Color32(0, 0, 0, 0);
        Vector2 left = new Vector2(size * 0.1f, size / 2f);
        Vector2 right = new Vector2(size * 0.9f, size / 2f);
        Vector2 top = new Vector2(size / 2f, size * 0.1f);
        Vector2 bottom = new Vector2(size / 2f, size * 0.9f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Vector2 p = new Vector2(x + 0.5f, y + 0.5f);
                bool inDiamond = PointInTriangle(p, left, top, right) || PointInTriangle(p, left, bottom, right);
                pixels[y * size + x] = inDiamond ? glow : transparent;
            }
        }

        tex.SetPixels32(pixels);
        return ToSprite(tex, pixelsPerUnit, new Vector2(0.5f, 0.5f));
    }

    public static Sprite CreateStarLayerSprite(float worldWidth, float worldHeight, int starCount, Color starColor, int seed, float pixelsPerUnit = 64f)
    {
        int texWidth = Mathf.Clamp(Mathf.CeilToInt(worldWidth * pixelsPerUnit), 64, 2048);
        int texHeight = Mathf.Clamp(Mathf.CeilToInt(worldHeight * pixelsPerUnit), 64, 2048);

        Texture2D tex = CreateTexture(texWidth, texHeight);
        Color32[] pixels = new Color32[texWidth * texHeight];

        System.Random rng = new System.Random(seed);
        for (int i = 0; i < starCount; i++)
        {
            int cx = rng.Next(0, texWidth);
            int cy = rng.Next(0, texHeight);
            int r = rng.Next(0, 100) < 15 ? 2 : 1;
            float brightness = 0.5f + (float)rng.NextDouble() * 0.5f;
            Color32 c = new Color32(
                (byte)(starColor.r * 255 * brightness),
                (byte)(starColor.g * 255 * brightness),
                (byte)(starColor.b * 255 * brightness),
                (byte)(starColor.a * 255));
            DrawDot(pixels, texWidth, texHeight, cx, cy, r, c);
        }

        tex.SetPixels32(pixels);
        // Pivot central: mantido consistente com o posicionamento em GameBootstrapper
        // (tiles centrados em 0 e actualTileWidth) e com o limiar de wrap em Parallax
        // (transform.position.x < -length), que so fecha sem gaps com pivot central.
        return ToSprite(tex, pixelsPerUnit, new Vector2(0.5f, 0.5f));
    }

    private static void DrawDot(Color32[] pixels, int texWidth, int texHeight, int cx, int cy, int r, Color32 c)
    {
        for (int dy = -r; dy <= r; dy++)
        {
            int y = cy + dy;
            if (y < 0 || y >= texHeight) continue;

            for (int dx = -r; dx <= r; dx++)
            {
                int x = cx + dx;
                if (x < 0 || x >= texWidth) continue;
                if (dx * dx + dy * dy <= r * r) pixels[y * texWidth + x] = c;
            }
        }
    }

    private static bool PointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
    {
        float d1 = Sign(p, a, b);
        float d2 = Sign(p, b, c);
        float d3 = Sign(p, c, a);

        bool hasNeg = d1 < 0 || d2 < 0 || d3 < 0;
        bool hasPos = d1 > 0 || d2 > 0 || d3 > 0;

        return !(hasNeg && hasPos);
    }

    private static float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
    }

    private static Texture2D CreateTexture(int width, int height)
    {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;
        tex.wrapMode = TextureWrapMode.Clamp;
        return tex;
    }

    private static Sprite ToSprite(Texture2D tex, float pixelsPerUnit, Vector2 pivot)
    {
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), pivot, pixelsPerUnit);
    }
}
