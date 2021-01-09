using Assets.Foundation.UI.PopUp;
using Komastar.Factory;
using Komastar.UI.Common;
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
    public PlayerObject player;
    public BrickObject brickPrefab;

    public Transform leftLimiter;
    public Transform rightLimiter;

    public Queue<BrickObject> Bricks;

    public Button[] buttons;
    public Sprite[] sprites;

    public float brickGenPeriod;
    public float brickSpeedInit;
    public float brickSpeed;
    public EBrickType brickTypeOverride;

    public Queue<IEnumerator> BuffQueue;

    private bool isSlow = false;

    private void Awake()
    {
        if (ReferenceEquals(null, Instance))
        {
            Instance = this;
        }
        Bricks = new Queue<BrickObject>();
        BuffQueue = new Queue<IEnumerator>();
        brickSpeed = brickSpeedInit;
    }

    private async void Start()
    {
        StartCoroutine(GenBrickCo());
        while (true)
        {
            var button = await UIButtonAsync.SelectButton<Button>(buttons);
            if (ReferenceEquals(null, button))
            {
                break;
            }

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
        if (isSlow)
        {
            isSlow = false;
            Debug.Log("Start CO");
            await SlowBrickCo(5f, 1f);
        }

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

    public IEnumerator GenBrickCo()
    {
        while (isActiveAndEnabled)
        {
            GenBrick();

            yield return new WaitForSeconds(brickGenPeriod);
        }
    }

    public void GenBrick()
    {
        if (PlayerObject.IsPlay)
        {
            var xPosition = Random.Range(leftLimiter.position.x, rightLimiter.position.x);
            var position = new Vector3(xPosition, transform.position.y, 0f);
            BrickObject newBrick;
            if (0 < Bricks.Count)
            {
                newBrick = Bricks.Dequeue();
                newBrick.transform.position = position;
                newBrick.gameObject.SetActive(true);
            }
            else
            {
                newBrick = Instantiate(brickPrefab, position, Quaternion.identity, transform);
            }

            newBrick.speed = brickSpeed;

            float randomBrick = Random.Range(0f, 100f);
            EBrickType brickType = EBrickType.Damage;
            if (EBrickType.Count != brickTypeOverride)
            {
                brickType = brickTypeOverride;
            }
            else
            {
                if (50f <= randomBrick)
                {
                    brickType = EBrickType.Slow;
                }
            }

            newBrick.ownBrick = BrickFactory.GetBrick(brickType, newBrick.gameObject);
            newBrick.spriteRenderer.sprite = sprites[(int)brickType];
        }
    }

    public void SlowBrick()
    {
        Debug.Log("SlowBrick");
        isSlow = true;
    }

    public async Task SlowBrickCo(float time, float speed)
    {
        Debug.Log("Slow");
        brickSpeed = speed;

        await Task.Delay((int)(time * 1000f));

        Debug.Log("Slow back");
        brickSpeed = brickSpeedInit;
    }
}
