using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }

            return _instance;
        }
    }

    [SerializeField] private TextMeshProUGUI _valueBallsText;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public void ChangeValueBalls(int valueBalls)
    {
        _valueBallsText.text = valueBalls.ToString();
    }
}
