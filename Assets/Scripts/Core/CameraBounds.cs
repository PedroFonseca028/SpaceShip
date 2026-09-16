using UnityEngine;

public static class CameraBounds
{
    public static float MinX { get; private set; }
    public static float MaxX { get; private set; }
    public static float MinY { get; private set; }
    public static float MaxY { get; private set; }

    public static void Recalculate()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;
        Vector3 pos = cam.transform.position;

        MinX = pos.x - halfWidth;
        MaxX = pos.x + halfWidth;
        MinY = pos.y - halfHeight;
        MaxY = pos.y + halfHeight;
    }
}
