using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private int healthBlock = 1;

    public void ChangeHealth(int ballDamage)
    {
        healthBlock -= ballDamage;

        if (healthBlock <= 0)
        {
            Destroy(gameObject);
        }
    }
}
