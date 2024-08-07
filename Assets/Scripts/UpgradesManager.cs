using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    private static UpgradesManager _instance;
    public static UpgradesManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UpgradesManager>();
            }

            return _instance;
        }
    }

    public Sprite[] SpritesUpdrages;
 
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public void PickUpgrade(int typeUpgrade)
    {
        switch (typeUpgrade)
        {
            case (int)Upgrades.AddLife:
                AddLife();
                break;

            case (int)Upgrades.Dead:
                Dead();
                break;

            case (int)Upgrades.MultiplyBalls:
                MultiplyBalls();
                break;

            case (int)Upgrades.IncreaseSizeBalls:
                ChangeSizeBalls(0.01f, true);
                break;

            case (int)Upgrades.DecreaseSizeBalls:
                ChangeSizeBalls(0.01f, false);
                break;

            case (int)Upgrades.IncreaseSizePlatform:
                ChangeSizePlatform(0.1f, true);
                break;

            case (int)Upgrades.DecreaseSizePlatform:
                ChangeSizePlatform(0.1f, false);
                break;
        }
    }

    public void AddLife()
    {
        GameManager.Instance.ChangeLife(1);
    }

    public void Dead()
    {
        int ballsCount = GameManager.Instance.balls.Count;

        for (int i = ballsCount - 1; i >= 0; i--)
        {
            GameObject destroyBall = GameManager.Instance.balls[i];
            GameManager.Instance.ChangeBallInGame(destroyBall);
            Destroy(destroyBall);
        }

    }

    public void MultiplyBalls()
    {
        int ballsCount = GameManager.Instance.balls.Count;

        for (int i = ballsCount - 1; i >= 0; i--)
        {
            GameObject newBall = Instantiate(GameManager.Instance.ball, GameManager.Instance.balls[i].transform.position, Quaternion.identity, GameManager.Instance.ballsGameObject.transform);
            Vector2 force = GameManager.Instance.balls[i].GetComponent<Rigidbody2D>().velocity;
            if (force == Vector2.one)
                force = Vector2.up * 7;

            newBall.transform.localScale = GameManager.Instance.balls[0].transform.localScale;
            newBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(-force.x, force.y), ForceMode2D.Impulse);

            GameManager.Instance.balls.Add(newBall);
        }
    }

    public void ChangeSizeBalls(float size, bool increase)
    {
        foreach (GameObject ball in GameManager.Instance.balls)
        {
            if ((ball.transform.localScale.x + size <= 0.05 && increase) || (ball.transform.localScale.x - size >= 0.01 && !increase))

            if (increase)
                ball.transform.localScale = new Vector3(ball.transform.localScale.x + size, ball.transform.localScale.y + size, ball.transform.localScale.z + size);
            else
                ball.transform.localScale = new Vector3(ball.transform.localScale.x - size, ball.transform.localScale.y - size, ball.transform.localScale.z - size);
        }
    }

    public void ChangeSizePlatform(float size, bool increase)
    {
        if (increase)
            GameManager.Instance.ChangeSizePlatform(size, increase);
        else
            GameManager.Instance.ChangeSizePlatform(-size, increase);
    }
}

public enum Upgrades
{
    AddLife = 0,
    Dead = 1,

    MultiplyBalls = 2,
    IncreaseSizeBalls = 3,
    DecreaseSizeBalls = 4,
    IncreaseSizePlatform = 5,
    DecreaseSizePlatform = 6
}
