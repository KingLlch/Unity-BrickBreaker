using UnityEngine;

public class Ball : MonoBehaviour
{
    public int ballDamage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Block")
        {
            collision.gameObject.GetComponent<Block>().EnterCollision(this);
        }

        if (collision.transform.tag == "EndScreen")
        {
            GameManager.Instance.ChangeBallInGame(gameObject);
            Destroy(gameObject);
        }
    }
}
