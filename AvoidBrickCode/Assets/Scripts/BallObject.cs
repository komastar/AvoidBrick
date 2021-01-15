using Komastar.Interface;
using UnityEngine;

public class BallObject : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite smileSprite;
    public Sprite sadSprite;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terminator"))
        {
            PlayerObject.GetPlayer().GetDamagable().SetHp(0);
            SetSadFace();
        }
    }

    public void SetSmileFace()
    {
        spriteRenderer.sprite = smileSprite;
        spriteRenderer.color = Color.white;
    }

    public void SetSadFace()
    {
        spriteRenderer.sprite = sadSprite;
        spriteRenderer.color = Color.red;
    }
}
