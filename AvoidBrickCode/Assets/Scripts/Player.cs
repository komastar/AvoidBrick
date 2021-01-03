using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private static Player instance;

    private static int score;
    public static int Score
    {
        get => score;
        set
        {
            if (score != value)
            {
                score = value;
                instance.ScoreText.text = score.ToString();
            }
        }
    }

    public static int Hp
    {
        get => instance.hp;
        set => instance.hp = value;
    }

    public static bool IsPlay = true;

    public float speed;
    public int hp;

    public Rigidbody2D rigid;
    public Text ScoreText;
    public Text GameOverText;
    public Ball ball;

    private void Awake()
    {
        instance = this;
        speed = 15f;
    }

    void Update()
    {
        if (0 >= hp)
        {
            Time.timeScale = 0f;
            IsPlay = false;
            enabled = false;
            GameOverText.gameObject.SetActive(true);
        }

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.A))
        {
            rigid.AddForce(Vector2.left * speed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rigid.AddForce(Vector2.right * speed);
        }
#elif UNITY_ANDROID
        rigid.AddForce(Input.acceleration * speed);
#endif
    }

    public void ResetPlayer()
    {
        IsPlay = true;
        Time.timeScale = 1f;
        Score = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlayScene");
    }
}
