using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float edgePadding = 0.3f;

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 delta = new Vector3(h, v, 0f) * moveSpeed * Time.deltaTime;
        Vector3 newPos = transform.position + delta;

        newPos.x = Mathf.Clamp(newPos.x, CameraBounds.MinX + edgePadding, CameraBounds.MaxX - edgePadding);
        newPos.y = Mathf.Clamp(newPos.y, CameraBounds.MinY + edgePadding, CameraBounds.MaxY - edgePadding);

        transform.position = newPos;
    }
}
