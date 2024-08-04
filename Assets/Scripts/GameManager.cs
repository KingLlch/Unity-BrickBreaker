using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }

    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] box;

    private GameObject ballInGame;
    private Camera mainCamera;
    private Rigidbody2D ballRigitbody;

    public bool IsGameNeedStart;

    private int ValueBallInGame;
    private int ValueBalls = 5;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;

        InputController.Instance.StartGameEvent.AddListener(StartGame);
        mainCamera = Camera.main;
    }

    private void Start()
    {
        IsGameNeedStart = true;

        ballInGame = Instantiate(ball);
        ballRigitbody = ballInGame.GetComponent<Rigidbody2D>();

        UIManager.Instance.ChangeValueBalls(ValueBalls);

        ChangeBoxSize();
    }

    private void Update()
    {
        if (IsGameNeedStart)
        {
            ballInGame.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 0.5f);
        }
    }

    private void StartGame()
    {
        IsGameNeedStart = false;
        ValueBallInGame = 1;
        ballRigitbody.AddForce(new Vector2(0, 17f), ForceMode2D.Impulse);
    }

    private void ChangeBoxSize()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, mainCamera.transform.position.z));
        Vector3 screenBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.transform.position.z));

        float cameraWidth = screenTopRight.x - screenBottomLeft.x;
        float cameraHeight = screenTopRight.y - screenBottomLeft.y;
        float wallSize = 1f;

        box[0].transform.position = new Vector3(0, screenTopRight.y + wallSize / 2, 0);
        box[1].transform.position = new Vector3(0, screenBottomLeft.y - wallSize / 2, 0);
        box[2].transform.position = new Vector3(screenBottomLeft.x - wallSize / 2, 0, 0);
        box[3].transform.position = new Vector3(screenTopRight.x + wallSize / 2, 0, 0);

        box[0].transform.localScale = new Vector3(cameraWidth + cameraWidth/5, wallSize, 1);
        box[1].transform.localScale = new Vector3(cameraWidth + cameraWidth/5, wallSize, 1);
        box[2].transform.localScale = new Vector3(wallSize, cameraHeight, 1);
        box[3].transform.localScale = new Vector3(wallSize, cameraHeight, 1);
    }

    public void ChangeBallInGame()
    {
        ValueBallInGame--;

        if (ValueBallInGame == 0)
        {
            ValueBalls--;
            UIManager.Instance.ChangeValueBalls(ValueBalls);

            ballInGame = Instantiate(ball);
            ballRigitbody = ballInGame.GetComponent<Rigidbody2D>();

            IsGameNeedStart = true;
        }
    }
}
