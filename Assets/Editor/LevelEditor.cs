using UnityEditor;
using UnityEngine;
using UnityEditor.EditorTools;
using static Unity.Collections.AllocatorManager;
using UnityEngine.UIElements;

[EditorTool("LevelEditor")]
public class LevelEditor : EditorTool
{
    public Texture2D ImageTool;
    [SerializeField] public GameObject Block;
    [SerializeField] public GameObject Marker;

    private Vector2 selectedPoint;
    private float selectedRotation;

    private GameObject marker1;
    private GameObject marker2;
    private GameObject marker3;
    private GameObject marker4;

    private bool pointSelected = false;
    private bool isRotating = false;

    public override GUIContent toolbarIcon
    {
        get
        {
            return new GUIContent
            {
                image = ImageTool,
                text = "Custom Level Editor",
                tooltip = "Custom Level Editor"
            };
        }
    }

    public override void OnToolGUI(EditorWindow window)
    {
        Event _event = Event.current;

        RightClickMouse(_event);
        LeftClickMouse(_event);
        DrawLine(_event);
    }

    private void RightClickMouse(Event _event)
    {
        if (_event.type == EventType.MouseDown && _event.button == 1)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(_event.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.CompareTag("Block"))
            {
                BlocksManager.Instance.ChangeTypeBlock(hit.collider.gameObject.GetComponent<Block>());
            }

            else
            {
                HideMarkers();
                SceneView.RepaintAll();
                pointSelected = false;
                isRotating = false;
            }

        }
    }

    private void DrawLine(Event _event)
    {
        if (pointSelected)
        {
            Vector2 mousePosition = HandleUtility.GUIPointToWorldRay(_event.mousePosition).origin;

            Handles.DrawLine(selectedPoint, mousePosition);
            SceneView.RepaintAll();
        }
    }

    private void LeftClickMouse(Event _event)
    {
        if (_event.type == EventType.MouseDown && _event.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(_event.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.CompareTag("Block"))
            {
                BlocksManager.Instance.NextLevelBlock(hit.collider.gameObject.GetComponent<Block>());
            }
        }

        else if (_event.type == EventType.MouseDown && _event.button == 0 && !pointSelected)
        {
            selectedPoint = HandleUtility.GUIPointToWorldRay(_event.mousePosition).origin;
            pointSelected = true;
        }

        else if (_event.type == EventType.MouseDown && _event.button == 0 && pointSelected && !isRotating)
        {
            Vector2 mousePosition = HandleUtility.GUIPointToWorldRay(_event.mousePosition).origin;
            Vector2 direction = new Vector2(mousePosition.x, mousePosition.y) - selectedPoint;

            selectedRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            isRotating = true;
        }

        if (_event.type == EventType.MouseDown && _event.button == 0 && pointSelected && isRotating)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(_event.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            GameObject block = Instantiate(Block);
            block.transform.SetParent(GameObject.Find("Game/Blocks").transform);

            if (hit.collider != null && hit.collider.CompareTag("Marker"))
            {
                Vector2 point = hit.collider.gameObject.transform.position;

                block.transform.position = new Vector3(point.x, point.y, 0);
                HideMarkers();
                DrawMarkers(point, block);
            }
            else
            {
                block.transform.position = new Vector3(selectedPoint.x, selectedPoint.y, 0);
                HideMarkers();
                DrawMarkers(selectedPoint, block);
            }

            block.transform.rotation = Quaternion.Euler(0, 0, selectedRotation);

            Undo.RegisterCreatedObjectUndo(block, "Create Block");
            pointSelected = false;
        }
    }

    private void DrawMarkers(Vector2 position, GameObject block)
    {
        Vector2 blockSize = block.GetComponent<BoxCollider2D>().size * block.transform.localScale;

        marker1 = Instantiate(Marker, position + (Vector2.up * blockSize), Quaternion.identity, block.transform);
        marker2 = Instantiate(Marker, position + (Vector2.right * blockSize), Quaternion.identity, block.transform);
        marker3 = Instantiate(Marker, position + (Vector2.down * blockSize), Quaternion.identity, block.transform);
        marker4 = Instantiate(Marker, position + (Vector2.left * blockSize), Quaternion.identity, block.transform);
    }

    private void HideMarkers()
    {
        if (marker1 != null)
            DestroyImmediate(marker1);
        if (marker2 != null)
            DestroyImmediate(marker2);
        if (marker3 != null)
            DestroyImmediate(marker3);
        if (marker4 != null)
            DestroyImmediate(marker4);
    }
}
