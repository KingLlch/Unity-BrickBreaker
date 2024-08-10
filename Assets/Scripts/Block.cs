using System;
using UnityEngine;

public class Block : MonoBehaviour
{
    public SpriteRenderer sprite;
    public int healthBlock = 1;

    public TypeBlock typeBlock;
    public GameObject anotherPortal;
    public bool isDetonated = false;

    public void EnterCollision(Ball ball)
    {
        BlocksManager.Instance.ChangeHealth(ball.ballDamage, this);

        BlocksManager.Instance.Portal(ball, this);
    }


}
