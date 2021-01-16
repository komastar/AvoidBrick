using Komastar.Interface;
using UnityEngine;
using UnityEngine.Events;

public class BrickObject : MonoBehaviour
{
    public static float GlobalSpeed;

    public SpriteRenderer spriteRenderer;
    public IBrick ownBrick;
    public UnityAction onCollision;

    private void Update()
    {
        transform.position += Vector3.down * GlobalSpeed * Time.deltaTime;
        if (-8f > transform.position.y)
        {
            TakeScore();
        }
    }

    private void OnDisable()
    {
        BrickGenerator.Instance.Bricks.Enqueue(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var go = collision.gameObject;
        if (go.CompareTag("Player"))
        {
            ownBrick.Take(PlayerObject.GetPlayer());
        }
        else if (go.CompareTag("Ball"))
        {
            ownBrick.Take(PlayerObject.GetPlayer());
        }
        else if (go.CompareTag("Cover"))
        {
            TakeScore();
            collision.gameObject.SetActive(false);
        }
    }

    public void TakeScore()
    {
        PlayerObject.Score++;
        gameObject.SetActive(false);
    }
}
