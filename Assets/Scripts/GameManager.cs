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

    public GameObject ball;
    public GameObject ballsGameObject;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject upgrade;
    [SerializeField] private GameObject[] box;

    private Camera mainCamera;
    private GameObject newBall;
    private Rigidbody2D ballRigitbody;

    public bool IsGameNeedStart;

    private int PointsUpgrade;
    private int PointsToUpgrade = 3;

    private int Life = 5;

    public List<GameObject> balls;

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

        newBall = Instantiate(ball, Vector2.zero, Quaternion.identity, ballsGameObject.transform);
        balls.Add(newBall);
        ballRigitbody = newBall.GetComponent<Rigidbody2D>();

        UIManager.Instance.ChangeValueBalls(Life);

        ChangeBoxSize();
    }

    private void Update()
    {
        if (IsGameNeedStart)
        {
            newBall.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1f);
        }
    }

    private void StartGame()
    {
        IsGameNeedStart = false;

        ballRigitbody.AddForce(new Vector2(0, 7f), ForceMode2D.Impulse);
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

        box[0].transform.localScale = new Vector3(cameraWidth + cameraWidth / 5, wallSize, 1);
        box[1].transform.localScale = new Vector3(cameraWidth + cameraWidth / 5, wallSize, 1);
        box[2].transform.localScale = new Vector3(wallSize, cameraHeight, 1);
        box[3].transform.localScale = new Vector3(wallSize, cameraHeight, 1);
    }

    public void ChangeBallInGame(GameObject ball)
    {
        balls.Remove(ball);

        if (balls.Count == 0)
        {
            Restart();
        }
    }

    public void Restart()
    {
        if(balls.Count > 0)
            for (int i = balls.Count - 1; i >= 0; i--)
            {
                GameObject ball = balls[i];

                balls.Remove(ball);
                Destroy(ball);
            }

        Life--;
        UIManager.Instance.ChangeValueBalls(Life);

        newBall = Instantiate(ball, Vector2.zero, Quaternion.identity, ballsGameObject.transform);
        balls.Add(newBall);
        ballRigitbody = newBall.GetComponent<Rigidbody2D>();

        IsGameNeedStart = true;
    }

    public void DestroyBlock(Transform block)
    {
        PointsUpgrade++;
        if (PointsUpgrade >= PointsToUpgrade)
        {
            PointsUpgrade = 0;
            SpawnUprade(block);
        }
    }

    public void SpawnUprade(Transform block)
    {
        GameObject upgrage = Instantiate(upgrade, block.position, Quaternion.identity, null);
        upgrage.GetComponent<Upgrade>().Initialize();
    }

    public void ChangeLife(int change)
    {
        Life += change;
        UIManager.Instance.ChangeValueBalls(Life);
    }

    public void ChangeSizePlatform(float change, bool increase)
    {
        if ((player.transform.localScale.x + change <= 2 && increase) ||( player.transform.localScale.x - change >= 0.2 && !increase))
            player.transform.localScale = new Vector3(player.transform.localScale.x + change, player.transform.localScale.y, player.transform.localScale.z);
    }

}
