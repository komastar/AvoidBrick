using UnityEngine;

public class Brick : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float speed;

    private void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terminator"))
        {
            Player.Score++;
            gameObject.SetActive(false);
            BrickGenerator.Instance.Bricks.Enqueue(this);
        }
    }
}
