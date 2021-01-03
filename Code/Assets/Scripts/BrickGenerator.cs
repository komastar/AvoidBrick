using Assets.Foundation.UI.Common;
using Assets.Foundation.UI.PopUp;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BrickGenerator : MonoBehaviour
{
    public static BrickGenerator Instance;

    public UIPopUpPanel uiPopUp;
    public Player player;
    public Brick brickPrefab;

    public Transform leftLimiter;
    public Transform rightLimiter;

    public Queue<Brick> Bricks;

    public Button[] buttons;
    public Sprite[] sprites;

    private void Awake()
    {
        Instance = this;
        Bricks = new Queue<Brick>();
    }

    private async void Start()
    {
        InvokeRepeating("GenBrick", 1f, 1f);
        while (true)
        {
            var button = await UIButtonAsync.SelectButton<Button>(buttons);
            if ("ExitButton" == button.name)
            {
                await ExitStage();
            }
            else if ("RestartButton" == button.name)
            {
                player.ResetPlayer();
            }
        }
    }

    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            await ExitStage();
        }
    }

    private async Task ExitStage()
    {
        Time.timeScale = 0f;
        var confirm = (UIConfirmPopUp)uiPopUp.Open("Confirm");
        var result = await confirm.GetResult();
        if (true == result)
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
        else
        {
            Time.timeScale = 1f;
            uiPopUp.TurnOff();
        }
    }

    public void GenBrick()
    {
        if (Player.IsPlay)
        {
            var xPosition = Random.Range(leftLimiter.position.x, rightLimiter.position.x);
            var position = new Vector3(xPosition, transform.position.y, 0f);
            Brick newBrick;
            if (0 < Bricks.Count)
            {
                newBrick = Bricks.Dequeue();
                newBrick.transform.position = position;
                newBrick.gameObject.SetActive(true);
                int randomIndex = Random.Range(0, sprites.Length);
                newBrick.spriteRenderer.sprite = sprites[randomIndex];
            }
            else
            {
                newBrick = Instantiate(brickPrefab, position, Quaternion.identity, transform);
            }
            newBrick.speed = 2f;
        }
    }
}
