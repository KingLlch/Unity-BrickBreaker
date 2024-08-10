using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BlocksManager : MonoBehaviour
{
    private static BlocksManager _instance;
    public static BlocksManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BlocksManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public void Portal(Ball ball, Block block)
    {
        if ((int)block.typeBlock == 2)
        {
            ball.transform.position = block.anotherPortal.transform.position;
            Destroy(gameObject);
            Destroy(block.anotherPortal);
        }
    }

    public void ChangeHealth(int ballDamage, Block block)
    {
        block.healthBlock -= ballDamage;
        block.sprite.color = new Color(block.sprite.color.r + 0.1f * ballDamage, block.sprite.color.g + 0.1f * ballDamage, block.sprite.color.b);
        if (block.healthBlock <= 0)
        {
            DestroyBlock(block);
        }
    }
    private void DestroyBlock(Block block)
    {
        if ((int)block.typeBlock == 1 && !block.isDetonated)
        {
            block.isDetonated = true;

            Collider2D[] BlocksColliders = Physics2D.OverlapCircleAll(transform.position, (transform.localScale * transform.GetComponent<BoxCollider2D>().size).x);
            int BlocksCollidersLength = BlocksColliders.Length;

            for (int i = BlocksCollidersLength - 1; i >= 0; i--)
            {
                if (BlocksColliders[i].GetComponent<Block>() && BlocksColliders[i].GetComponent<Block>() != this)
                {
                    Block thisBlock = BlocksColliders[i].GetComponent<Block>();
                    ChangeHealth(1, thisBlock);
                }
            }
        }

        EffectsManager.Instance.Burst(transform.position);
        GameManager.Instance.DestroyBlock(transform);
        Destroy(gameObject);
    }

    public void NextLevelBlock(Block block)
    {
        if (block.healthBlock < 10 && block.typeBlock == 0)
        {
            block.healthBlock++;
            block.sprite.color = new Color(block.sprite.color.r - 0.1f, block.sprite.color.g - 0.1f, block.sprite.color.b);
        }
    }
    public void ChangeTypeBlock(Block block)
    {
        block.typeBlock++;
        block.sprite.color = new Color(1, 1, 1);

        if ((int)block.typeBlock == 1)
        {
            block.sprite.sprite = MaterialManager.Instance.TNTBlockSprite;
            block.healthBlock = 1;
        }

        else if ((int)block.typeBlock == 2)
        {
            block.sprite.sprite = MaterialManager.Instance.PortalBlockSprite;
            block.healthBlock = 1;
        }

        else if ((int)block.typeBlock > Enum.GetNames(typeof(TypeBlock)).Length - 1)
        {
            block.typeBlock = 0;
            block.sprite.sprite = MaterialManager.Instance.DefaultBlockSprite;
            block.healthBlock = 1;
        }
    }

}

public enum TypeBlock
{
    Default = 0,
    TNT = 1,
    Teleport = 2
}
