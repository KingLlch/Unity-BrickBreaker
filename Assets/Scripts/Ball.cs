using UnityEngine;

public class Ball : MonoBehaviour
{
    private int ballDamage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Block")
        {
            collision.gameObject.GetComponent<Block>().ChangeHealth(ballDamage);
        }

        if (collision.transform.tag == "DestroyBall")
        {
            GameManager.Instance.ChangeBallInGame();
            Destroy(gameObject);
        }
    }
}
