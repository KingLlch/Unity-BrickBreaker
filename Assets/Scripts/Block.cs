using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private int healthBlock = 1;

    public void ChangeHealth(int ballDamage)
    {
        healthBlock -= ballDamage;
        sprite.color = new Color(sprite.color.r + 0.1f * ballDamage, sprite.color.g, sprite.color.b);

        if (healthBlock <= 0)
        {
            GameManager.Instance.DestroyBlock(transform);
            Destroy(gameObject);
        }
    }

    public void NextLevelBlock()
    {
        if (healthBlock < 10)
        {
            healthBlock++;
            sprite.color = new Color(sprite.color.r - 0.1f, sprite.color.g, sprite.color.b);
        }
    }
}
