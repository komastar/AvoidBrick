using Komastar.Interface;
using UnityEngine;
using UnityEngine.Events;

public class BrickObject : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float speed;
    public IBrick ownBrick;
    public UnityAction onCollision;

    private void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
        if (-8f > transform.position.y)
        {
            TakeScore();
        }
    }

    private void OnDisable()
    {
        BrickGenerator.Instance.Bricks.Enqueue(this);
    }

    public void TakeScore()
    {
        PlayerObject.Score++;
        gameObject.SetActive(false);
    }
}
