using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Upgrade : MonoBehaviour
{
    private SpriteRenderer spriteUpgrade;
    private int TypeUpgrade = 0;

    private void Awake()
    {
        spriteUpgrade = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.position += Vector3.down * 0.02f;
    }

    public void Initialize()
    {
        int typeUpgrade = Random.Range(0, Enum.GetNames(typeof(Upgrades)).Length);
        TypeUpgrade = typeUpgrade;
        spriteUpgrade.sprite = UpgradesManager.Instance.SpritesUpdrages[typeUpgrade];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            UpgradesManager.Instance.PickUpgrade(TypeUpgrade);
            Destroy(gameObject);
        }

        else if (collision.transform.tag == "EndScreen")
        {
            Destroy(gameObject);
        }
    }
}
