using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dragSpeed = 2.0f;
    public float zoomSpeed = 2.0f;
    public float minZoom = 5.0f;
    public float maxZoom = 20.0f;
    public GameObject panelSpeaker; // PanelSpeaker 오브젝트를 인스펙터에서 할당
    public GameObject panelOption; // PanelOption 오브젝트를 인스펙터에서 할당
    public BoxCollider2D worldBoundary; // 게임 월드의 경계를 나타내는 BoxCollider2D

    private Vector3 dragOrigin;

    void Update()
    {
        HandleMouseDrag();
        HandleMouseZoom();
    }

    private void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            return;
        }

        if (!Input.GetMouseButton(1)) return;

        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        Vector3 direction = dragOrigin - currentMousePosition;
        Vector3 newPosition = Camera.main.transform.position + direction * dragSpeed;

        // 카메라 경계 제한
        newPosition = ClampCamera(newPosition);

        Camera.main.transform.position = newPosition;
    }

    private void HandleMouseZoom()
    {
        // PanelSpeaker와 PanelOption이 활성화되어 있으면 확대/축소를 비활성화합니다.
        if ((panelSpeaker != null && panelSpeaker.activeSelf) || (panelOption != null && panelOption.activeSelf))
        {
            return;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float size = Camera.main.orthographicSize;
        size -= scroll * zoomSpeed;
        size = Mathf.Clamp(size, minZoom, maxZoom);

        Camera.main.orthographicSize = size;

        // 확대/축소 후에도 카메라 경계 제한
        Camera.main.transform.position = ClampCamera(Camera.main.transform.position);
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize * 2;
        float camWidth = camHeight * cam.aspect;

        float minX = worldBoundary.bounds.min.x + camWidth / 2;
        float maxX = worldBoundary.bounds.max.x - camWidth / 2;
        float minY = worldBoundary.bounds.min.y + camHeight / 2;
        float maxY = worldBoundary.bounds.max.y - camHeight / 2;

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

        return targetPosition;
    }
}
