using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    private static MaterialManager _instance;
    public static MaterialManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MaterialManager>();
            }

            return _instance;
        }
    }

    public Sprite DefaultBlockSprite;
    public Sprite TNTBlockSprite;
    public Sprite PortalBlockSprite;

    public Sprite DefaultBallSprite;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
}
