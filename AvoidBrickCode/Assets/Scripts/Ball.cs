using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite smileSprite;
    public Sprite sadSprite;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terminator"))
        {
            Player.Hp = 0;
            spriteRenderer.sprite = sadSprite;
            spriteRenderer.color = Color.red;
        }
        else if (collision.gameObject.CompareTag("Brick"))
        {
            Player.Hp -= 1;
            spriteRenderer.sprite = sadSprite;
            spriteRenderer.color = Color.red;
        }
    }
}
