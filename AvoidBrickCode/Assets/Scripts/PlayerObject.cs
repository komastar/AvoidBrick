using Komastar.Combat;
using Komastar.Interface;
using UnityEngine;
using UnityEngine.UI;

public class PlayerObject : MonoBehaviour, IPlayer
{
    private static PlayerObject instance;
    public static IPlayer GetPlayer()
    {
        return instance;
    }

    private IDamagable damagable = null;

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

    public static bool IsPlay = true;

    public float speed;

    public Rigidbody2D rigid;
    public Text ScoreText;
    public Text GameOverText;
    public BallObject ball;

    private void Awake()
    {
        instance = this;
        speed = 15f;
        damagable = new Damagable();
        damagable.SetHp(1);
    }

    void Update()
    {
        if (0 >= damagable.GetHp())
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

    public void SetSmileFace()
    {
        ball.SetSmileFace();
    }

    public void SetSadFace()
    {
        ball.SetSadFace();
    }

    public IDamagable GetDamagable()
    {
        return damagable;
    }
}
