using UnityEngine;

// Ensina o "algoritmo do parallax scrolling": cada camada tem sua propria
// velocidade (parallaxEffect) e, ao sair pela esquerda da tela, e reposicionada
// para o final da fileira de tiles, criando a ilusao de um fundo infinito.
[RequireComponent(typeof(SpriteRenderer))]
public class Parallax : MonoBehaviour
{
    [Range(0f, 1f)] public float parallaxEffect = 0.3f;
    public float baseSpeed = 2.5f;
    public int tileCount = 2;

    private float length;

    void Awake()
    {
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float speed = baseSpeed * parallaxEffect * SlowMotionController.CurrentScale;
        transform.position += Vector3.left * Time.deltaTime * speed;

        if (transform.position.x < -length)
        {
            transform.position += new Vector3(length * tileCount, 0f, 0f);
        }
    }
}
