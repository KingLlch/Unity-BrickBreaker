using UnityEngine;

public class MoveManager : MonoBehaviour
{
    [SerializeField] private GameObject _handle;

    private float speed = 0.1f;
    private Camera mainCamera;
    private Vector3 screenTopRight;
    private Vector3 screenBottomLeft;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, mainCamera.transform.position.z));
        screenBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.transform.position.z));
    }

    private void Update()
    {

        if ((InputController.Instance.InputMove.x * speed + _handle.transform.position.x + _handle.transform.localScale.x/2 < screenTopRight.x) && 
            (InputController.Instance.InputMove.x * speed + _handle.transform.position.x - _handle.transform.localScale.x/2 > screenBottomLeft.x))
            _handle.transform.position += new Vector3 (InputController.Instance.InputMove.x, 0, 0) * speed;
    }
}
