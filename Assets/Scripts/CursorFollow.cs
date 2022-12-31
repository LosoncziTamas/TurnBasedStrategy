using UnityEngine;

public class CursorFollow : MonoBehaviour
{
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 cursorPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;
    }
}
